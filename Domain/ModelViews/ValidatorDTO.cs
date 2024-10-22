
using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Interface;

namespace DioVeiculos101.Domain.ModelViews;
public class ValidatorDTO
{
    public ValidationErrors ValidateDto<T>(T dto) where T : IValidatable
    {
        var validator = new ValidationErrors
        {
            Messages = new List<string> { }
        };

        // Validação comum a todos que implementam IValidatable
        if (string.IsNullOrEmpty(dto.Name))
            validator.Messages.Add("Nome não pode ser vazio.");

        // Validações específicas para VehicleDTO
        if (dto is VehicleDTO vehicleDTO)
        {
            if (string.IsNullOrEmpty(vehicleDTO.Brand))
                validator.Messages.Add("Marca não pode ser vazio.");

            if (vehicleDTO.Year <= 0)
                validator.Messages.Add("Ano não pode estar nulo ou igual a zero.");
        }

        // Validações específicas para AdminDTO
        if (dto is AdminDTO adminDTO)
        {
            if (string.IsNullOrEmpty(adminDTO.Email))
                validator.Messages.Add("Email não pode ser vazio.");

            if (string.IsNullOrEmpty(adminDTO.Password))
                validator.Messages.Add("Senha não pode ser vazia.");
        }

        return validator;
    }
}