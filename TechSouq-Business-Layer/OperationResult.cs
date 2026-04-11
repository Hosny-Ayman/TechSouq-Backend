using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechSouq.Application
{

    public enum OperationStatus
    {
        Success,
        NotFound,
        BadRequest,
        ServerError,
        Unauthorized
    }

    public class OperationResult<T>
    {
        public OperationStatus Status { get; private set; }

        public string Message { get; private set; } 

        public T Data { get; private set; }

        public bool IsSuccess { get; private set; }

        public List<string> Errors { get; private set; }

        private OperationResult (bool isSuccess, T data, string message, OperationStatus status,List<string>errors )
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            Status = status;
            Errors = errors ?? new List<string>();
        }

        public static OperationResult<T> Success(T data, string message = "Operation successful")
        {
            return new OperationResult<T>(true, data, message, OperationStatus.Success, null);
        }

        public static OperationResult<T> Failure(string message = "An unexpected error occurred. Please try again later.")
        {
            return new OperationResult<T>(false, default, message, OperationStatus.ServerError, null);
        }

        public static OperationResult<T> NotFound(string message = "The requested resource was not found.")
        {
            return new OperationResult<T>(false, default, message, OperationStatus.NotFound, null);
        }

        public static OperationResult<T> BadRequest(string message = "Invalid request data or validation failed.", List<string>errors = null)
        {
            return new OperationResult<T>(false, default, message, OperationStatus.BadRequest, errors);
        }

    }
}
