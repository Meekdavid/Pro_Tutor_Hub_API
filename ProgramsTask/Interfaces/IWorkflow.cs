using ProgramsTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Helpers;

namespace ProgramsTask.Interfaces
{
    public interface IWorkflow
    {
        Task<List<workflowDTO>> retrieveWorkflows(string sqlCosmosQuery);
        Task<List<workflowDTO>> retrieveWorkflowsByUserID(string sqlCosmosQuery);
        Task<workflowDTO> Update(workflowDTO programToUpdate);

    }
}
