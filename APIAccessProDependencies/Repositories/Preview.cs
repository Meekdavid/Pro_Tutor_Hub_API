using APIAccessProDependencies.Helpers.Common;
using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Global;
using APIAccessProDependencies.Helpers.DTOs.Models;
using APIAccessProDependencies.Helpers.Extensions;
using APIAccessProDependencies.Helpers.Logger;
using APIAccessProDependencies.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APIAccessProDependencies.Helpers.Common.Utils;

namespace APIAccessProDependencies.Repositories
{
    public class Preview : IPreview
    {
        private readonly Container _container;
        private string className = string.Empty;
        public Preview(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
            className = GetType().Name;
        }
        public async Task<MethodReturnResponse<List<PreviewDTO>>> ApplicationPreviewAsnyc(string sqlCosmosQuery)
        {
            string methodName = "ApplicationPreviewAsnyc", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<List<PreviewDTO>> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Retreiving Preview from the Database with Query: {sqlCosmosQuery}").AppendLine();

            try
            {
                var query = _container.GetItemQueryIterator<PreviewDTO>(new QueryDefinition(sqlCosmosQuery));

                List<PreviewDTO> result = new List<PreviewDTO>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    result.AddRange(response);
                }

                if (result.Count > 0)
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Preview Successfully Fetched from the Database.").AppendLine();

                    theReturner = new MethodReturnResponse<List<PreviewDTO>>
                    {
                        success = true,
                        Logs = logs,
                        objectValue = result,
                        _message = Utils.StatusMessage_Success
                    };
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} No Preview Found on the Database.").AppendLine();

                    theReturner = new MethodReturnResponse<List<PreviewDTO>>
                    {
                        success = true,
                        Logs = logs,
                        objectValue = result,
                        _message = Utils.StatusMessage_PreviewUnavilable
                    };
                }
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Retreiving Preview Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Retreiving the Preview").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<List<PreviewDTO>>
                {
                    success = false,
                    Logs = logs,
                    objectValue = null
                };
            }
            finally
            {
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            return theReturner;
        }

        //public Task<MethodReturnResponse<List<PreviewDTO>>> ApplicationPreviewByUserIDAsync(string sqlCosmosQuery)
        //{
        //    string methodName = "ApplicationPreviewByUserIDAsync", classAndMethodName = $"{className}.{methodName}";
        //    var logs = new List<Log>();
        //    var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
        //    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Retreiving Preview by User ID with Query: {sqlCosmosQuery}").AppendLine();

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        //ON EXCEPTION STORE THE PREVIOUS LOG
        //        LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Retreiving Preview Exception");

        //        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Retreiving the Preview").AppendLine();
        //        logBuilder.ToString().AddToLogs(ref logs);
        //    }
        //    finally
        //    {
        //        logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
        //        logBuilder.ToString().AddToLogs(ref logs);
        //        logBuilder.Clear();
        //    }
        //}
    }
}
