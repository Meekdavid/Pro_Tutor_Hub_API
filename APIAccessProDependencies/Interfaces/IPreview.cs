using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Interfaces
{
    public interface IPreview
    {
        Task<MethodReturnResponse<List<PreviewDTO>>> ApplicationPreviewAsnyc(string sqlCosmosQuery);
        //Task<MethodReturnResponse<List<PreviewDTO>>> ApplicationPreviewByUserIDAsync(string sqlCosmosQuery);
    }
}
