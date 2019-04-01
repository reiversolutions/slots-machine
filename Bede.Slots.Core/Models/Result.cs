using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Models
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string FailureMessage { get; set; }

        public T Data { get; set; }

        public static Result<T> OnSuccess(T result)
        {
            return new Result<T>()
            {
                Success = true,
                FailureMessage = string.Empty,
                Data = result
            };
        }

        public static Result<T> OnFailure(string failureMessage)
        {
            return new Result<T>()
            {
                Success = false,
                FailureMessage = failureMessage,
                Data = default(T)
            };
        }
    }
}
