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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static APIAccessProDependencies.Helpers.Common.Utils;

namespace APIAccessProDependencies.Repositories
{
    public class Program : IProgram
    {
        private readonly Container _container;
        private string className = string.Empty;
        public Program(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
            className = GetType().Name;
        }
        public async Task<MethodReturnResponse<ProgramDTO>> AddProgramAsync(ProgramDTO newProgram)
        {
            string methodName = "AddProgramAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<ProgramDTO> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Request Received to Add Program to Database is: {JsonConvert.SerializeObject(newProgram)}").AppendLine();

            try
            {
                object existingProgram = null;
                try { existingProgram = await _container.ReadItemAsync<ProgramDTO>(newProgram.id, new PartitionKey(newProgram.programId)); } catch { }
                if (existingProgram == null)
                {
                    var item = await _container.CreateItemAsync/*<ProgramDTO>*/(newProgram, new PartitionKey(newProgram.programId.Trim()));

                    if (item.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Program Sucessfully Created").AppendLine();

                        theReturner = new MethodReturnResponse<ProgramDTO>
                        {
                            success = true,
                            objectValue = item,
                            Logs = logs,
                            _message = Utils.StatusMessage_Success
                        };
                    }
                    else
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} An Error Occured when Creating the Program, Response: {JsonConvert.SerializeObject(item)}").AppendLine();

                        theReturner = new MethodReturnResponse<ProgramDTO>
                        {
                            success = false,
                            objectValue = item,
                            Logs = logs,
                            _message = Utils.StatusMessage_UnknownError
                        };
                    }
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Record Already Exists on the Database for the Requested Program to Add.").AppendLine();
                    theReturner = new MethodReturnResponse<ProgramDTO>
                    {
                        success = false,
                        objectValue = null,
                        Logs = logs,
                        _message = Utils.StatusMessage_RecordAlreadyExists

                    };
                }

                
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Program Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Program").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<ProgramDTO>
                {
                    success = false,
                    objectValue = null,
                    Logs = logs,
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

        public async Task<MethodReturnResponse<List<ProgramDTO>>> RetrieveProgramsAsync(string sqlCosmosQuery)
        {
            string methodName = "RetrieveProgramsAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<List<ProgramDTO>> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Retreiving Program from the Database with Query: {JsonConvert.SerializeObject(sqlCosmosQuery)}").AppendLine();

            try
            {
                var query = _container.GetItemQueryIterator<ProgramDTO>(new QueryDefinition(sqlCosmosQuery));

                List<ProgramDTO> result = new List<ProgramDTO>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    result.AddRange(response);
                }

                if (result.Count > 0)
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Program Retreived Sucessfully.").AppendLine();

                    theReturner = new MethodReturnResponse<List<ProgramDTO>>
                    {
                        success = true,
                        objectValue = result,
                        Logs = logs,
                        _message = Utils.StatusMessage_Success
                    };
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} No Program Found on the Database.").AppendLine();

                    theReturner = new MethodReturnResponse<List<ProgramDTO>>
                    {
                        success = true,
                        Logs = logs,
                        objectValue = result,
                        _message = Utils.StatusMessage_ProgramUnavilable
                    };
                }
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Retreiving Program Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Retreiving the Program").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<List<ProgramDTO>>
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

        //public async Task<MethodReturnResponse<List<ProgramDTO>>> RetrieveProgramsByUserIDAsync(string sqlCosmosQuery)
        //{
        //    string methodName = "RetrieveProgramsByUserIDAsync", classAndMethodName = $"{className}.{methodName}";
        //    var logs = new List<Log>();
        //    var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
        //    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About to Retreive Program by ID from the DB with Query: {sqlCosmosQuery}").AppendLine();

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        //ON EXCEPTION STORE THE PREVIOUS LOG
        //        LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Retreiving Program Exception");

        //        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Retreiving the Program").AppendLine();
        //        logBuilder.ToString().AddToLogs(ref logs);
        //    }
        //    finally
        //    {
        //        logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
        //        logBuilder.ToString().AddToLogs(ref logs);
        //        logBuilder.Clear();
        //    }
        //}

        public async Task<MethodReturnResponse<ProgramDTO>> UpdateProgramAsync(ProgramDTO programToUpdate)
        {
            string methodName = "UpdateProgramAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<ProgramDTO> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Program Received to Update the Database is: {JsonConvert.SerializeObject(programToUpdate)}").AppendLine();

            try
            {
                var existingProgram =  await _container.ReadItemAsync<ProgramDTO>(programToUpdate.id, new PartitionKey(programToUpdate.programId));
                if(existingProgram.Resource != null)
                {
                    // Use reflection to check and ensure only requested parameters are updated.
                    PropertyInfo[] properties = typeof(ProgramDTO).GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        string propertyName = property.Name;

                        if (propertyName.ToUpper() == "ID" || propertyName.ToUpper() == "PROGRAMID") continue;

                        object updatedValue = property.GetValue(programToUpdate);

                        if ((updatedValue != null && !string.IsNullOrEmpty(updatedValue.ToString()) && updatedValue.ToString().ToLower() != "string"))
                        {
                            property.SetValue(existingProgram.Resource, updatedValue);
                        }
                    }

                    var item = await _container.UpsertItemAsync<ProgramDTO>(existingProgram.Resource, new PartitionKey(existingProgram.Resource.programId));

                    if (item.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Program Sucessfully Updated").AppendLine();

                        theReturner = new MethodReturnResponse<ProgramDTO>
                        {
                            success = true,
                            objectValue = item,
                            Logs = logs,
                            _message = Utils.StatusMessage_Success

                        };
                    }
                    else
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Requested Program was Unable to Update, Response: {JsonConvert.SerializeObject(item)}").AppendLine();

                        theReturner = new MethodReturnResponse<ProgramDTO>
                        {
                            success = false,
                            objectValue = item,
                            Logs = logs,
                            _message = Utils.StatusMessage_UnknownError

                        };
                    }
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} No Record Found on the Database for the Requested Program to Update.").AppendLine();
                    theReturner = new MethodReturnResponse<ProgramDTO>
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
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Updating Program Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Updating the Program").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<ProgramDTO>
                {
                    success = false,
                    objectValue = null,
                    Logs = logs,
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
