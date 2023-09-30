using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Models;
using ProgramsTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Interfaces
{
    public interface IApplicationForm
    {
        Task<MethodReturnResponse<List<ApplicationFormDTO>>> RetrieveFormsAsync(string sqlCosmosQuery);
        Task<MethodReturnResponse<ApplicationFormDTO>> UpdateFormAsync(ApplicationFormDTO programToUpdate);
        Task<MethodReturnResponse<ApplicationFormDTO>> AmendFormAsync(AddQuestion arrayToDelete);
        Task<MethodReturnResponse<ApplicationFormDTO>> AddFormAsync(ApplicationFormDTO newForm);
    }
}
