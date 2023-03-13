using Microsoft.Azure.Cosmos;
using ProgramsTask.Interfaces;
using ProgramsTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Helpers;

namespace ProgramsTask.Repositories
{
    public class preview : IPreview
    {
        private readonly Container _container;
        public preview(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<List<previewDTO>> applicationPreview(string sqlCosmosQuery)
        {
            var query = _container.GetItemQueryIterator<previewDTO>(new QueryDefinition(sqlCosmosQuery));

            List<previewDTO> result = new List<previewDTO>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        public async Task<List<previewDTO>> applicationPreviewByUserID(string sqlCosmosQuery)
        {
            var query = _container.GetItemQueryIterator<previewDTO>(new QueryDefinition(sqlCosmosQuery));

            List<previewDTO> result = new List<previewDTO>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }
    }
}
