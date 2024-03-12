using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Models;

public class Aluno : IEntidade
{
    [Key]
    public int? Matricula { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    [RegularExpression(@"^[^\s]+.*[^\s]$", ErrorMessage = "Nome inválido.")]
    public string? Nome { get; set; }

    [StringLength(14)]
    [CPFValidation(ErrorMessage = "CPF inválido")]
    public string? CPF { get; set; }

    [DataType(DataType.Date)]
    public DateTime Nascimento { get; set; }

    public EnumeradorSexo Sexo { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Aluno other = (Aluno)obj;
        return  Matricula == other.Matricula && Nome == other.Nome && CPF == other.CPF && Nascimento == other.Nascimento && Sexo == other.Sexo;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Matricula.GetHashCode();
            hash = hash * 23 + (Nome != null? Nome.GetHashCode(): 0);
            hash = hash * 23 + (CPF != null? CPF.GetHashCode() : 0);
            hash = hash * 23 + Nascimento.GetHashCode();
            hash = hash * 23 + Sexo.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return $"Matrícula: {Matricula}, Nome: {Nome}, CPF: {CPF}, Nascimento: {Nascimento.ToShortDateString()}, Sexo:{Sexo}";
    }

}
