using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BsuirHealthProjectServer.Shared
{
    public class ValidationResult
    {
        public bool IsSuccess { get; private set; }

        public List<string> ErrorMessages { get; private set; }

        public ValidationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
            ErrorMessages = new List<string>();
        }

        public ValidationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            ErrorMessages = new List<string>();
            ErrorMessages.Add(message);
        }

        public void AddErrorMessage(string message)
        {
            IsSuccess = false;
            ErrorMessages.Add(message);
        }

        public string GetAllErrors()
        {
            string result = ErrorMessages.Count > 0 ? ErrorMessages.First() : string.Empty;
            for (int i = 1; i < ErrorMessages.Count; i++)
                result += Environment.NewLine + ErrorMessages[i];
            return result;
        }
    }

}