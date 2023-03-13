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
    public interface IProgram
    {
        Task<List<programDTO>> retrievePrograms(string sqlCosmosQuery);
        Task<List<programDTO>> retrieveProgramsByUserID(string sqlCosmosQuery);
        Task<programDTO> AddProgramAsync(programDTO newProgram);
        Task<programDTO> Update(programDTO programToUpdate);
    }
}
