using EM.Domain.Utilitary;
using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Models;

public class CPFValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var cpf = value as string;

        if (string.IsNullOrWhiteSpace(cpf))
            return ValidationResult.Success;

        if(!cpf.ValidarCPF())
           return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}
