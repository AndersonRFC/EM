using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using EM.Domain.Models;

namespace EM.Domain.Utilitary;

public static class iText
{

	public static string GerarRelatorioPDF(int qtdAlunos, List<Aluno> alunos)
	{
		var alunosSelecionados = alunos.Take(qtdAlunos).ToList();

		if (alunosSelecionados.Count > 0)
		{
			//calculo do total de páginas
			//1a página - 24 registros
			//2a em diante - 29 registros
			//100 registros -> -24 = 76 -> / 29 = 2.alguma coisa
			int totalPaginas = 1;
			if (alunosSelecionados.Count > 24)
				totalPaginas += (int)Math.Ceiling(
					(alunosSelecionados.Count - 24) / 29F);

			//configurar dados do PDF
			var pxPorMm = 72 / 25.2F;
			var pdf = new Document(PageSize.A4, 15 * pxPorMm, 15 * pxPorMm,
				15 * pxPorMm, 20 * pxPorMm);
			var nomeArquivo = $"alunos.{DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss")}.pdf";
			var arquivo = new FileStream(nomeArquivo, FileMode.Create);
			var writer = PdfWriter.GetInstance(pdf, arquivo);
			writer.PageEvent = new RodapeRelatorioPDF(totalPaginas);
			pdf.Open();

			var fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

			//adiciona um título
			var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 32,
				iTextSharp.text.Font.NORMAL, BaseColor.Black);
			var titulo = new Paragraph("Relatório de Alunos\n\n", fonteParagrafo);
			titulo.Alignment = Element.ALIGN_LEFT;
			titulo.SpacingAfter = 4;
			pdf.Add(titulo);

			////adiciona um link                
			//var fonteLink = new iTextSharp.text.Font(
			//	fonteBase, 9.9f, Font.NORMAL, iTextSharp.text.BaseColor.Blue);
			//var link = new Chunk("Link do meu GitHub, rs", fonteLink);
			//var larguraTexto = fonteBase.GetWidthPoint(link.Content, fonteLink.Size);
			//link.SetAnchor("https://github.com/AndersonRFC");
			//var caixaTexto = new ColumnText(writer.DirectContent);
			//caixaTexto.AddElement(link);
			//caixaTexto.SetSimpleColumn(
			//	pdf.PageSize.Width - pdf.RightMargin - larguraTexto,
			//	pdf.PageSize.Height - pdf.TopMargin - (30 * pxPorMm),
			//	pdf.PageSize.Width - pdf.RightMargin,
			//	pdf.PageSize.Height - pdf.TopMargin - (18 * pxPorMm));
			//caixaTexto.Go();

			////adiciona uma imagem
			//var caminhoImagem = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
			//	"img\\gitHub.png");
			//if (File.Exists(caminhoImagem))
			//{
			//	iTextSharp.text.Image logo =
			//		iTextSharp.text.Image.GetInstance(caminhoImagem);
			//	float razaoLarguraAltura = logo.Width / logo.Height;
			//	float alturaLogo = 32;
			//	float larguraLogo = alturaLogo * razaoLarguraAltura;
			//	logo.ScaleToFit(larguraLogo, alturaLogo);
			//	var margemEsquerda = pdf.PageSize.Width - pdf.RightMargin - larguraLogo;
			//	var margemTopo = pdf.PageSize.Height - pdf.TopMargin - 54;
			//	logo.SetAbsolutePosition(margemEsquerda, margemTopo);
			//	writer.DirectContent.AddImage(logo, false);
			//}

			//adiciona uma tabela
			var tabela = new PdfPTable(5);
			float[] larguras = { 0.6f, 2f, 1.5f, 1f, 1f };
			tabela.SetWidths(larguras);
			tabela.DefaultCell.BorderWidth = 0;
			tabela.WidthPercentage = 100;

			//adiciona os títulos das colunas
			CriarCelulaTexto(tabela, "Matricula", PdfPCell.ALIGN_CENTER, true);
			CriarCelulaTexto(tabela, "Nome", PdfPCell.ALIGN_LEFT, true);
			CriarCelulaTexto(tabela, "CPF", PdfPCell.ALIGN_CENTER, true);
			CriarCelulaTexto(tabela, "Nascimento", PdfPCell.ALIGN_CENTER, true);
			CriarCelulaTexto(tabela, "Sexo", PdfPCell.ALIGN_CENTER, true);

			foreach (var a in alunosSelecionados)
			{
				CriarCelulaTexto(tabela, ((int)a.Matricula).ToString("D6"), PdfPCell.ALIGN_CENTER);
				CriarCelulaTexto(tabela, a.Nome);
				CriarCelulaTexto(tabela, a.CPF, PdfPCell.ALIGN_CENTER);
				CriarCelulaTexto(tabela, a.Nascimento.ToString("dd-MM-yyyy"), PdfPCell.ALIGN_RIGHT);
				CriarCelulaTexto(tabela, a.Sexo == EnumeradorSexo.Masculino ? "Masculino" : "Feminino", PdfPCell.ALIGN_RIGHT);

				//CriarCelulaTexto(tabela, a.Matriculado ? "Sim" : "Não", PdfPCell.ALIGN_CENTER);
				//var caminhoImagemCelula = a.Empregado ?
				//	"img\\emoji_feliz.png" : "img\\emoji_triste.png";
				//caminhoImagemCelula = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
				//	caminhoImagemCelula);
				//criarCelulaImagem(tabela, caminhoImagemCelula, 20, 20);
			}



			pdf.Add(tabela);

			pdf.Close();
			arquivo.Close();

			var caminhoPDF = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);
			if (File.Exists(caminhoPDF))
			{
				Process.Start(new ProcessStartInfo()
				{
					//Arguments = $"/c start firefox {caminhoPDF}",
					Arguments = $"/c start {caminhoPDF}",
					CreateNoWindow = true,
					FileName = "cmd.exe"
				});
			}
			return nomeArquivo;
		}
		else
		{
			Console.WriteLine("Nenhum produto foi retornado.");
			return "";
		}

	}

	static void CriarCelulaTexto(PdfPTable tabela, string texto,
		int alinhamento = PdfPCell.ALIGN_LEFT,
		bool negrito = false, bool italico = false,
		int tamanhoFonte = 12, int alturaCelula = 25)
	{
		int estilo = iTextSharp.text.Font.NORMAL;
		if (negrito && italico)
		{
			estilo = iTextSharp.text.Font.BOLDITALIC;
		}
		else if (negrito)
		{
			estilo = iTextSharp.text.Font.BOLD;
		}
		else if (italico)
		{
			estilo = iTextSharp.text.Font.ITALIC;
		}

		BaseFont fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
		iTextSharp.text.Font fonte = new iTextSharp.text.Font(fonteBase, tamanhoFonte,
			estilo, iTextSharp.text.BaseColor.Black);

		//cor de fundo diferente para linhas pares e ímpares
		var bgColor = iTextSharp.text.BaseColor.White;
		if (tabela.Rows.Count % 2 == 1)
			bgColor = new BaseColor(0.95f, 0.95f, 0.95f);

		PdfPCell celula = new PdfPCell(new Phrase(texto, fonte));
		celula.HorizontalAlignment = alinhamento;
		celula.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
		celula.Border = 0;
		celula.BorderWidthBottom = 1;
		celula.PaddingBottom = 5; //pra alinhar melhor verticalmente
		celula.FixedHeight = alturaCelula;
		celula.BackgroundColor = bgColor;
		tabela.AddCell(celula);
	}

	static void criarCelulaImagem(PdfPTable tabela, string caminhoImagem,
		int larguraImagem, int alturaImagem, int alturaCelula = 25)
	{
		//cor de fundo diferente para linhas pares e ímpares
		var bgColor = iTextSharp.text.BaseColor.White;
		if (tabela.Rows.Count % 2 == 1)
			bgColor = new BaseColor(0.95f, 0.95f, 0.95f);

		if (File.Exists(caminhoImagem))
		{
			iTextSharp.text.Image imagem =
				iTextSharp.text.Image.GetInstance(caminhoImagem);
			imagem.ScaleToFit(larguraImagem, alturaImagem);
			PdfPCell celula = new PdfPCell(imagem);
			celula.HorizontalAlignment = PdfCell.ALIGN_CENTER;
			celula.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			celula.Border = 0;
			celula.BorderWidthBottom = 1;
			celula.FixedHeight = alturaCelula;
			celula.BackgroundColor = bgColor;
			tabela.AddCell(celula);
		}
		else
		{
			tabela.AddCell("ERRO");
		}
	}
}
