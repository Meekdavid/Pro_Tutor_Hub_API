using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramsTask.Processes
{
    public class xmlInjectionCheck
    {
        private readonly ILogger<xmlInjectionCheck> _logger;
        public xmlInjectionCheck(ILogger<xmlInjectionCheck> logger)
        {
            this._logger = logger;
        }

        public string xmlCheck(string input)
        {
            string thisMethod = "XMLCheck";
            _logger.LogInformation($"\r\n\r\n********** About to Begin Execution of {thisMethod} *********");
            try
            {
                _logger.LogInformation($"About Validating the Input: {input} against XMLInjection");
                if (input.Contains("<") || input.Contains(">"))
                {
                    _logger.LogInformation($"Input: {input} was vulnerable to XMLInjection");
                    return "FALSE";                    
                }
                _logger.LogInformation($"Input: {input} is not vulnerable to XMLInjection");
                return input;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, $"An Error occured on Method: {thisMethod}, While validating Input: {input}");
                return "FALSE";
            }
        }
    }
}
