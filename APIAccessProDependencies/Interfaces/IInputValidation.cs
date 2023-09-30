using APIAccessProDependencies.Helpers.DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Interfaces
{
    public interface IInputValidation
    {
        Task<MethodReturnResponse<bool>> ValidatePhone(string phoneNo);
        Task<MethodReturnResponse<string>> ValidateInput(string input);
        void ProcessObjectAgainstInputThreats(object input);
    }
}
