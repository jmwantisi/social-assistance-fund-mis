namespace socialAssistanceFundMIS.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }
        public List<string> ValidationErrors { get; }

        private Result(bool isSuccess, T? value, string? error, List<string>? validationErrors = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            ValidationErrors = validationErrors ?? new List<string>();
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string error) => new(false, default, error);
        public static Result<T> ValidationFailure(List<string> validationErrors) => new(false, default, "Validation failed", validationErrors);

        public static implicit operator Result<T>(T value) => Success(value);
    }

    public class Result : Result<object>
    {
        private Result(bool isSuccess, object? value, string? error, List<string>? validationErrors = null) 
            : base(isSuccess, value, error, validationErrors) { }

        public static Result Success() => new(true, null, null);
        public static Result Failure(string error) => new(false, null, error);
        public static Result ValidationFailure(List<string> validationErrors) => new(false, null, "Validation failed", validationErrors);
    }
}

