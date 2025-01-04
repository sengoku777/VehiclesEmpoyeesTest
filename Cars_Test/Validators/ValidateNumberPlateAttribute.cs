using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Cars_Test.Validators
{
    public class ValidateNumberPlateAttribute : ValidationAttribute
    {
        private const string Pattern = @"^[АВЕКМНОРСТУХ]{1}\d{3}[АВЕКМНОРСТУХ]{2}\d{2,3}$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string plate)
            {
                if (Regex.IsMatch(plate, Pattern))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Номерной знак не соответствует формату РФ.");
                }
            }

            return new ValidationResult("Некорректный формат входных данных.");
        }
    }
}
