using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Models;

public class AlunoViewModel
{
    public List<Aluno>? Alunos { get; set; }

    [StringLength(50)]
    public string? SearchString { get; set; }

    public string? SearchType { get; set; }
}
