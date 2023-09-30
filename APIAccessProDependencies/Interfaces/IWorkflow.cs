using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Interfaces
{
    public interface IWorkflow
    {
        Task<MethodReturnResponse<List<WorkflowDTO>>> RetrieveWorkflowsAsync(string sqlCosmosQuery);
        //Task<MethodReturnResponse<List<WorkflowDTO>>> RetrieveWorkflowsByUserIDAsync(string sqlCosmosQuery);
        Task<MethodReturnResponse<WorkflowDTO>> UpdateWorkflowsAsync(WorkflowDTO programToUpdate, string command);
    }
}
