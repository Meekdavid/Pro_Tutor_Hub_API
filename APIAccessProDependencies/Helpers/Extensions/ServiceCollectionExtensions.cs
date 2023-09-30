using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using APIAccessProDependencies.Helpers.ConfigurationSettings.ConfigManager;
using Microsoft.Azure.Cosmos;
using APIAccessProDependencies.Interfaces;
using APIAccessProDependencies.Repositories;

namespace MerchantTransactionCore.Helpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDBServices(this IServiceCollection services)
        {            
            string url = ConfigSettings.ApplicationSetting.URI;
            string primaryKey = ConfigSettings.ApplicationSetting.primaryKey;
            string dbName = ConfigSettings.ApplicationSetting.cosmosDatabase;

            services.AddSingleton<IApplicationForm>(options =>
            {
                string containerName = ConfigSettings.ApplicationSetting.applicationFormContainer;
                var cosmosClient = new CosmosClient(url, primaryKey);

                return new ApplicationForm(cosmosClient, dbName, containerName);
            });
            services.AddSingleton<IProgram>(options =>
            {
                string containerName = ConfigSettings.ApplicationSetting.programContainer;
                var cosmosClient = new CosmosClient(
                    url,
                    primaryKey
                );

                return new Program(cosmosClient, dbName, containerName);
            });
            services.AddSingleton<IWorkflow>(options =>
            {
                string containerName = ConfigSettings.ApplicationSetting.workflowContainer;
                var cosmosClient = new CosmosClient(
                    url,
                    primaryKey
                );

                return new Workflow(cosmosClient, dbName, containerName);
            });
            services.AddSingleton<IPreview>(options =>
            {
                //string containerName = ConfigSettings.ApplicationSetting.previewContainer;
                //var cosmosClient = new CosmosClient(
                //    url,
                //    primaryKey
                //);

                string containerName = ConfigSettings.ApplicationSetting.programContainer;
                var cosmosClient = new CosmosClient(
                    url,
                    primaryKey
                );

                return new Preview(cosmosClient, dbName, containerName);
            });

            return services;
        }
        public static IServiceCollection AddOtherServices(this IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            return services;
        }
    }
}
