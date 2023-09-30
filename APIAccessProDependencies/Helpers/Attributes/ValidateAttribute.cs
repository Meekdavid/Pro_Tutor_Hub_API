using APIAccessProDependencies.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ValidateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            try
            {
                //if (string.IsNullOrWhiteSpace(value?.ToString()))
                //{
                //    return new ValidationResult($"The {validationContext.DisplayName} Field is Required");
                //}

                SanitizeInputs.ProcessObjectAgainstInputThreats(value);

                /* TO DO: Add other validations and input operations later*/

                validationContext.ObjectType.GetProperty(validationContext.MemberName).SetValue(validationContext.ObjectInstance, value, null);
            }
            catch (Exception ex)
            {
                return new ValidationResult("An Unknown Error Occurred, Please Try Again!");
            }
            return ValidationResult.Success;
        }
    }
}
