using APIAccessProDependencies.Helpers.Common;
using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Global;
using APIAccessProDependencies.Helpers.DTOs.Models;
using APIAccessProDependencies.Helpers.Extensions;
using APIAccessProDependencies.Helpers.Logger;
using APIAccessProDependencies.Interfaces;
using AutoMapper.Internal;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static APIAccessProDependencies.Helpers.Common.Utils;

namespace APIAccessProDependencies.Repositories
{
    public class Workflow : IWorkflow
    {
        private readonly Container _container;
        private string className = string.Empty;
        public Workflow(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
            className = GetType().Name;
        }
        public async Task<MethodReturnResponse<List<WorkflowDTO>>> RetrieveWorkflowsAsync(string sqlCosmosQuery)
        {
            string methodName = "RetrieveWorkflowsAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<List<WorkflowDTO>> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Retreiving Workflow from the Database with Query: {sqlCosmosQuery}").AppendLine();

            try
            {
                var query = _container.GetItemQueryIterator<WorkflowDTO>(new QueryDefinition(sqlCosmosQuery));

                List<WorkflowDTO> result = new List<WorkflowDTO>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    result.AddRange(response);
                }

                if (result.Count > 0)
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Workflow Sucessfully Retreived").AppendLine();

                    theReturner = new MethodReturnResponse<List<WorkflowDTO>>
                    {
                        success = true,
                        objectValue = result,
                        Logs = logs,
                        _message = Utils.StatusMessage_Success
                    };
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} No Workflow Found on the Database.").AppendLine();

                    theReturner = new MethodReturnResponse<List<WorkflowDTO>>
                    {
                        success = true,
                        Logs = logs,
                        objectValue = result,
                        _message = Utils.StatusMessage_WorkflowUnavilable
                    };
                }
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Retreiving Workflow Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Retreiving the Workflow").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<List<WorkflowDTO>>
                {
                    success = false,
                    objectValue = null,
                    Logs = logs
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

        //public async Task<MethodReturnResponse<List<WorkflowDTO>>> RetrieveWorkflowsByUserIDAsync(string sqlCosmosQuery)
        //{
        //    string methodName = "RetrieveWorkflowsByUserIDAsync", classAndMethodName = $"{className}.{methodName}";
        //    var logs = new List<Log>();
        //    var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
        //    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About to Retreive Workflow from the Database with Query: {sqlCosmosQuery}").AppendLine();

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        //ON EXCEPTION STORE THE PREVIOUS LOG
        //        LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Retreiving Workflow Exception");

        //        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Retreiving the Workflow").AppendLine();
        //        logBuilder.ToString().AddToLogs(ref logs);
        //    }
        //    finally
        //    {
        //        logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
        //        logBuilder.ToString().AddToLogs(ref logs);
        //        logBuilder.Clear();
        //    }
        //}

        public async Task<MethodReturnResponse<WorkflowDTO>> UpdateWorkflowsAsync(WorkflowDTO workflowToUpdate, string command)
        {
            string methodName = "UpdateWorkflowsAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<WorkflowDTO> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Request Received to Update Workflow to Database is: {JsonConvert.SerializeObject(workflowToUpdate)}").AppendLine();

            try
            {
                var existingFlow = await _container.ReadItemAsync<WorkflowDTO>(workflowToUpdate.id, new PartitionKey(workflowToUpdate.programId));
                if (existingFlow.Resource != null)
                {
                    if (command.Contains("stage"))//When request is about adding a new stage to the workflow
                    {                        
                        existingFlow.Resource.stages = existingFlow.Resource.stages.Concat(workflowToUpdate.stages ?? Array.Empty<Stages>()).ToArray();
                    }
                    else
                    {
                        // Use reflection to check and ensure only requested parameters are updated.
                        PropertyInfo[] properties = typeof(WorkflowDTO).GetProperties();

                        foreach (PropertyInfo property in properties)
                        {
                            string propertyName = property.Name;

                            if (propertyName.ToUpper() == "ID" || propertyName.ToUpper() == "PROGRAMID") continue;

                            object updatedValue = property.GetValue(workflowToUpdate);

                            if ((updatedValue != null && !string.IsNullOrEmpty(updatedValue.ToString()) && updatedValue.ToString().ToLower() != "string"))
                            {
                                property.SetValue(existingFlow.Resource, updatedValue);
                            }
                            /* TO DO: Also Check for other Properties that are Objects themselves, using Recurssion */
                        }
                    }

                    var item = await _container.UpsertItemAsync<WorkflowDTO>(existingFlow.Resource, new PartitionKey(existingFlow.Resource.programId));

                    if (item.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Workflow Sucessfully Updated").AppendLine();

                        theReturner = new MethodReturnResponse<WorkflowDTO>
                        {
                            Logs = logs,
                            objectValue = item,
                            success = true,
                            _message = Utils.StatusMessage_Success
                        };
                    }
                    else
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} There was a Problem Updating the Requested Workflow, Response: {JsonConvert.SerializeObject(item)}").AppendLine();

                        theReturner = new MethodReturnResponse<WorkflowDTO>
                        {
                            Logs = logs,
                            objectValue = item,
                            success = true,
                            _message = Utils.StatusMessage_UnknownError
                        };
                    }
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} No Record Found on the Database for the Requested Flow to Update.").AppendLine();
                    theReturner = new MethodReturnResponse<WorkflowDTO>
                    {
                        success = false,
                        objectValue = null,
                        Logs = logs,
                        _message = Utils.StatusMessage_RecordNotFound

                    };
                }
                
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Updating Workflow Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Updating the Workflow").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<WorkflowDTO>
                {
                    Logs = logs,
                    objectValue = null,
                    success = false,
                    _message = Utils.StatusMessage_Failure
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
    }
}
