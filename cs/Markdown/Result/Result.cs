using System;

namespace Markdown.Result
{
    public class Result<T>
    {
        private readonly Status status;
        public readonly T Value;

        private bool IsSuccess => status == Status.Success;
        public bool IsFaulted => !IsSuccess;

        public static Result<T> Success(T value) => new Result<T>(Status.Success, value);
        public static Result<T> Fail(Status status) => new Result<T>(status != Status.Success ? status : throw new ArgumentException(), default(T));

        public Result<TR> Select<TR>(Func<T, TR> selector) => IsSuccess ? Result<TR>.Success(selector(Value)) : Result<TR>.Fail(status);

        private Result(Status status, T value)
        {
            this.status = status;
            Value = value;
        }
    }
}