using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Interfaces
{
    public interface IProgram
    {
        Task<MethodReturnResponse<List<ProgramDTO>>> RetrieveProgramsAsync(string sqlCosmosQuery);
        //Task<MethodReturnResponse<List<ProgramDTO>>> RetrieveProgramsByUserIDAsync(string sqlCosmosQuery);
        Task<MethodReturnResponse<ProgramDTO>> AddProgramAsync(ProgramDTO newProgram);
        Task<MethodReturnResponse<ProgramDTO>> UpdateProgramAsync(ProgramDTO programToUpdate);
    }
}
