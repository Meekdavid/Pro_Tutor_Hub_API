using APIAccessProDependencies.Helpers.Common;
using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Global;
using APIAccessProDependencies.Helpers.DTOs.Models;
using APIAccessProDependencies.Helpers.Extensions;
using APIAccessProDependencies.Helpers.Logger;
using APIAccessProDependencies.Interfaces;
using APIAccessProDependencies.Services;
using AutoMapper.Internal;
using Azure;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Newtonsoft.Json;
using ProgramsTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static APIAccessProDependencies.Helpers.Common.Utils;

namespace APIAccessProDependencies.Repositories
{
    public class ApplicationForm : IApplicationForm
    {
        private readonly Container _container;
        private string className = string.Empty;
        public ApplicationForm(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
            className = GetType().Name;
        }
        public async Task<MethodReturnResponse<ApplicationFormDTO>> AddFormAsync(ApplicationFormDTO newForm)
        {
            string methodName = "AddFormAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<ApplicationFormDTO> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Form Received to Add to Database is: {JsonConvert.SerializeObject(newForm)}").AppendLine();

            try
            {
                var existingForm = await _container.ReadItemAsync<ApplicationFormDTO>(newForm.id, new PartitionKey(newForm.programId));
                if(existingForm.Resource != null)
                {
                    ApplicationFormDTO item = await _container.CreateItemAsync<ApplicationFormDTO>(newForm, new PartitionKey(newForm.programId));

                    if (item != null)
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Form Sucessfully Added to the Database.").AppendLine();
                        theReturner = new MethodReturnResponse<ApplicationFormDTO>
                        {
                            success = true,
                            Logs = logs,
                            objectValue = item,
                            _message = Utils.StatusMessage_Success
                        };
                    }
                    else
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Form was not Sucessfully Added to the Database, Response: {JsonConvert.SerializeObject(item)}").AppendLine();
                        theReturner = new MethodReturnResponse<ApplicationFormDTO>
                        {
                            success = false,
                            Logs = logs,
                            objectValue = item,
                            _message = Utils.StatusMessage_UnknownError
                        };
                    }
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Record Already Exists on the Database for the Requested Form to Add.").AppendLine();
                    theReturner = new MethodReturnResponse<ApplicationFormDTO>
                    {
                        success = false,
                        objectValue = null,
                        Logs = logs,
                        _message = Utils.StatusMessage_RecordAlreadyExists

                    };
                }
                
            }
            catch( Exception ex )
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<ApplicationFormDTO>
                {
                    success = false,
                    Logs = logs,
                    objectValue = null,
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

        public async Task<MethodReturnResponse<ApplicationFormDTO>> AmendFormAsync(AddQuestion arrayToAmend)
        {
            string methodName = "AmendFormAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<ApplicationFormDTO> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            string command = arrayToAmend.ids.command.ToLower() == "add" ? "Add Question" : "Delete Question";
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Received Request to {command}: {JsonConvert.SerializeObject(arrayToAmend)}").AppendLine();

            try
            {
                var existingForm = await _container.ReadItemAsync<ApplicationFormDTO>(arrayToAmend.ids.documentID, new PartitionKey(arrayToAmend.ids.programID));
                if(existingForm.Resource != null)
                {                    
                    //existingForm.Resource.personal_Information.add_a_Question = existingForm.Resource.personal_Information.add_a_Question.Where(item => item != arrayToDeleteHolder).ToArray();

                    if(arrayToAmend.ids.command.ToLower() == "add")//If it requires adding a question
                    {
                        /* TO DO: Amend the implementation to add multiple array to the existing array */
                        AddQuestionModel arrayToDeleteHolder = new AddQuestionModel
                        {
                            type = arrayToAmend.type,
                            question = arrayToAmend.question,
                            choice = arrayToAmend.choice
                        };

                        // Create a new array with a larger size
                        AddQuestionModel[] newArray = new AddQuestionModel[existingForm.Resource.personal_Information.add_a_Question.Length + 1];

                        // Copy the elements from the existing array to the new one
                        Array.Copy(existingForm.Resource.personal_Information.add_a_Question, newArray, existingForm.Resource.personal_Information.add_a_Question.Length);

                        // Add the new object to the last position
                        newArray[newArray.Length - 1] = arrayToDeleteHolder;

                        // Update the reference to the new array
                        existingForm.Resource.personal_Information.add_a_Question = newArray;
                    }
                    else
                    {
                        AddQuestionModel arrayToDeleteHolder = new AddQuestionModel
                        {
                            type = arrayToAmend.type,
                            question = arrayToAmend.question,
                            choice = arrayToAmend.choice
                        };

                        // Remove the specific instance based on the criteria
                        existingForm.Resource.personal_Information.add_a_Question = existingForm.Resource.personal_Information.add_a_Question.Where(model =>
                            model.type != arrayToDeleteHolder.type &&
                            model.question != arrayToDeleteHolder.question &&
                            model.choice != arrayToDeleteHolder.choice).ToArray();
                    }                    

                    //var response = await _container.ReplaceItemAsync(existingForm.Resource, existingForm.Resource.id, new PartitionKey(existingForm.Resource.programId));
                    var response = await _container.UpsertItemAsync<ApplicationFormDTO>(existingForm.Resource, new PartitionKey(existingForm.Resource.programId));
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Question Sucessfully Amended.").AppendLine();

                        theReturner = new MethodReturnResponse<ApplicationFormDTO>
                        {
                            Logs = logs,
                            objectValue = existingForm.Resource,
                            success = true,
                            _message = Utils.StatusMessage_Success
                        };
                    }
                    else
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Question was not Sucessfully Amended, Response: {JsonConvert.SerializeObject(response)}").AppendLine();

                        theReturner = new MethodReturnResponse<ApplicationFormDTO>
                        {
                            Logs = logs,
                            objectValue = existingForm.Resource,
                            success = false,
                            _message = Utils.StatusMessage_UnknownError
                        };
                    }
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} No Record Found on the Database for the Requested Question to Amend.").AppendLine();
                    theReturner = new MethodReturnResponse<ApplicationFormDTO>
                    {
                        Logs = logs,
                        objectValue = null,
                        success = false,
                        _message = Utils.StatusMessage_RecordNotFound
                    };
                }
                
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Delete Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Deleting the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<ApplicationFormDTO>
                {
                    Logs = logs,
                    objectValue = null,
                    success = false
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

        public async Task<MethodReturnResponse<List<ApplicationFormDTO>>> RetrieveFormsAsync(string sqlCosmosQuery)
        {
            string methodName = "RetrieveFormsAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<List<ApplicationFormDTO>> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About to Retreive Forms with Query: {sqlCosmosQuery}").AppendLine();

            try
            {
                var query = _container.GetItemQueryIterator<ApplicationFormDTO>(new QueryDefinition(sqlCosmosQuery));

                List<ApplicationFormDTO> result = new List<ApplicationFormDTO>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    result.AddRange(response);
                }

                if (result.Count > 0)
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Form Record Retreived is: {JsonConvert.SerializeObject(result)}").AppendLine();

                    theReturner = new MethodReturnResponse<List<ApplicationFormDTO>>
                    {
                        success = true,
                        Logs = logs,
                        objectValue = result,
                        _message = Utils.StatusMessage_Success
                    };
                }
                else
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} No Application Form Found on the Database.").AppendLine();

                    theReturner = new MethodReturnResponse<List<ApplicationFormDTO>>
                    {
                        success = true,
                        Logs = logs,
                        objectValue = result,
                        _message = Utils.StatusMessage_ApplicationFormUnavilable
                    };
                }
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Retreiving Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Retreiving the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<List<ApplicationFormDTO>>
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

        public async Task<MethodReturnResponse<ApplicationFormDTO>> UpdateFormAsync(ApplicationFormDTO formToUpdate)
        {
            string methodName = "UpdateFormAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            MethodReturnResponse<ApplicationFormDTO> theReturner;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Form Received to Update to Database is: {JsonConvert.SerializeObject(formToUpdate)}").AppendLine();

            try
            {
                var existingForm = await _container.ReadItemAsync<ApplicationFormDTO>(formToUpdate.id, new PartitionKey(formToUpdate.programId));
                if (existingForm.Resource != null)
                {
                    // Use reflection to check and ensure only requested parameters are updated.                   
                    ObjectModificationManager.UpdateProperties(existingForm.Resource, formToUpdate);

                    var item = await _container.UpsertItemAsync<ApplicationFormDTO>(existingForm.Resource, new PartitionKey(existingForm.Resource.programId));

                    if (item.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Database Sucessfully Updated.").AppendLine();

                        theReturner = new MethodReturnResponse<ApplicationFormDTO>
                        {
                            success = true,
                            objectValue = item,
                            Logs = logs,
                            _message = Utils.StatusMessage_Success
                        };
                    }
                    else
                    {
                        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Database was not Sucessfully Updated, Response: {JsonConvert.SerializeObject(item)}").AppendLine();

                        theReturner = new MethodReturnResponse<ApplicationFormDTO>
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
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} No Record Found on the Database for the Requested Form to Update.").AppendLine();
                    theReturner = new MethodReturnResponse<ApplicationFormDTO>
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
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Updating Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Updating the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                theReturner = new MethodReturnResponse<ApplicationFormDTO>
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
