using System;

namespace Markdown.TagRendering
{
    public class Result<T>
    {
        public readonly Status Status;
        public readonly T Value;

        public bool IsSuccess => Status == Status.Success;
        public bool IsFaulted => !IsSuccess;

        public static Result<T> Success(T value) => new Result<T>(Status.Success, value);
        public static Result<T> Fail(Status status) => new Result<T>(status != Status.Success ? status : throw new ArgumentException(), default(T));

        public Result<TR> Select<TR>(Func<T, TR> selector) => IsSuccess ? Result<TR>.Success(selector(Value)) : Result<TR>.Fail(Status);

        private Result(Status status, T value)
        {
            Status = status;
            Value = value;
        }
    }
    
    public enum Status
    {
        Success,
        Fail,
        NotFound
    }
}