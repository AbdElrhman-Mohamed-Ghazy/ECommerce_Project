using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Results
{
    public class Result
    {
        protected Result(bool isSuccess, IReadOnlyCollection<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get; }
        public IReadOnlyCollection<string> Errors { get; }

        public static Result Success() => new(true, Array.Empty<string>());

        public static Result Failure(params string[] errors) =>
            new(false, errors?.ToArray() ?? Array.Empty<string>());
    }

    public class Result<T> : Result
    {
        private Result(bool isSuccess, T? value, IReadOnlyCollection<string> errors)
            : base(isSuccess, errors)
        {
            Value = value;
        }

        public T? Value { get; }

        public static Result<T> Success(T value) => new(true, value, Array.Empty<string>());

        public static new Result<T> Failure(params string[] errors) =>
            new(false, default, errors?.ToArray() ?? Array.Empty<string>());
    }
}
