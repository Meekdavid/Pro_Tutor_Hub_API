using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramsTask.Processes
{
    public class phoneCheck
    {
        private readonly ILogger<phoneCheck> _logger;
        public phoneCheck(ILogger<phoneCheck> logger)
        {
            _logger = logger;
        }

        public string checkPhone(string phone)
        {
            string thisMethod = "validatePhoneInput";
            _logger.LogInformation($"********** About to Begin Execution of {thisMethod} *********");
            try
            {
                if (phone.StartsWith("234"))
                {
                    phone = "0" + phone.Substring(3);
                }

                ulong phoneNumber;

                _logger.LogInformation($"About Validating the Input: {phone}");
                var IsGoodNumber = ulong.TryParse(phone, out phoneNumber);
                if (!IsGoodNumber)
                {
                    _logger.LogInformation($"Phone Number: {phone} was not Accepted");
                    return "FALSE";
                }
                _logger.LogInformation($"Phone Number: {phone} was Valid and Accepted");
                return phone;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, $"An Error occured on Method: {thisMethod}, While validating Phone Number: {phone}");
                return "FALSE";
            }
        }
    }
}
