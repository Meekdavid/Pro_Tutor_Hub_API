using ProgramsTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Helpers;

namespace ProgramsTask.Interfaces
{
    public interface IApplicationForm
    {
        Task<List<applicationFormDTO>> retrieveForms(string sqlCosmosQuery);
        Task<List<applicationFormDTO>> retrieveFormsByUserID(string sqlCosmosQuery);
        Task<applicationFormDTO> Update(applicationFormDTO programToUpdate);
        Task Delete(string id, string userID);

        //Task<List<applicationFormModel>> AddFormAsync(applicationFormModel newForm);
    }
}
