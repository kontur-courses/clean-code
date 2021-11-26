namespace Markdown
{
    
    public enum Status
    {
        Success,
        NotFound,
        BadResult
    }
    
    public class Result<T>
    {
        public readonly Status Status;
        public readonly T Value;

        public Result(Status status, T value)
        {
            Status = status;
            Value = value;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(Status.Success, value);
        }

        public static Result<T> Fail(Status reason)
        {
            return new Result<T>(reason, default(T));
        }
    }
}