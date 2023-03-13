using Microsoft.Azure.Cosmos;
using ProgramsTask.Interfaces;
using ProgramsTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Helpers;

namespace ProgramsTask.Repositories
{
    public class programme : IProgram
    {
        private readonly Container _container;
        public programme(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<programDTO> AddProgramAsync(programDTO newProgram)
        {
            var item = await _container.CreateItemAsync<programDTO>(newProgram, new PartitionKey(newProgram.UserID));
            return item;
        }

        public async Task<List<programDTO>> retrievePrograms(string sqlCosmosQuery)
        {
            var query = _container.GetItemQueryIterator<programDTO>(new QueryDefinition(sqlCosmosQuery));

            List<programDTO> result = new List<programDTO>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        public async Task<List<programDTO>> retrieveProgramsByUserID(string sqlCosmosQuery)
        {
            var query = _container.GetItemQueryIterator<programDTO>(new QueryDefinition(sqlCosmosQuery));

            List<programDTO> result = new List<programDTO>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        public async Task<programDTO> Update(programDTO programToUpdate)
        {
            var item = await _container.UpsertItemAsync<programDTO>(programToUpdate, new PartitionKey(programToUpdate.UserID));
            return item;
        }
    }
}
