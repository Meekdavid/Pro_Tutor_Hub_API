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
    public class applicationForm : IApplicationForm
    {
        private readonly Container _container;
        public applicationForm(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task Delete(string id, string userID)
        {
            await _container.DeleteItemAsync<applicationFormModel>(id, new PartitionKey(userID));
        }

        //public async Task<List<applicationFormModel>> AddFormAsync(applicationFormModel newForm)
        //{
        //    var item = await _container.CreateItemAsync<List<applicationFormModel>>(newForm, new PartitionKey(newForm.userID));
        //    return item;
        //}

        public async Task<List<applicationFormDTO>> retrieveForms(string sqlCosmosQuery)
        {
            var query = _container.GetItemQueryIterator<applicationFormDTO>(new QueryDefinition(sqlCosmosQuery));

            List<applicationFormDTO> result = new List<applicationFormDTO>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        public async Task<List<applicationFormDTO>> retrieveFormsByUserID(string sqlCosmosQuery)
        {
            var query = _container.GetItemQueryIterator<applicationFormDTO>(new QueryDefinition(sqlCosmosQuery));

            List<applicationFormDTO> result = new List<applicationFormDTO>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        public async Task<applicationFormDTO> Update(applicationFormDTO formToUpdate)
        {
            var item = await _container.UpsertItemAsync<applicationFormDTO>(formToUpdate, new PartitionKey(formToUpdate.userID));
            return item;
        }
    }
}
