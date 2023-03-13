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
    public class workFlow : IWorkflow
    {
        private readonly Container _container;
        public workFlow(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<List<workflowDTO>> retrieveWorkflows(string sqlCosmosQuery)
        {

            var query = _container.GetItemQueryIterator<workflowDTO>(new QueryDefinition(sqlCosmosQuery));

            List<workflowDTO> result = new List<workflowDTO>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        public async Task<List<workflowDTO>> retrieveWorkflowsByUserID(string sqlCosmosQuery)
        {

            var query = _container.GetItemQueryIterator<workflowDTO>(new QueryDefinition(sqlCosmosQuery));

            List<workflowDTO> result = new List<workflowDTO>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        public async Task<workflowDTO> Update(workflowDTO workflowToUpdate)
        {
            var item = await _container.UpsertItemAsync<workflowDTO>(workflowToUpdate, new PartitionKey(workflowToUpdate.userID));
            return item;
        }
    }
}
