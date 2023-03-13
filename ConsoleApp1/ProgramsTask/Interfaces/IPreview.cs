using ProgramsTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Helpers;

namespace ProgramsTask.Interfaces
{
    public interface IPreview
    {
        Task<List<previewDTO>> applicationPreview(string sqlCosmosQuery);
        Task<List<previewDTO>> applicationPreviewByUserID(string sqlCosmosQuery);
    }
}
