using Domain.Errors;

namespace Domain.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if(IsSuccess && error != Error.None ) throw new InvalidOperationException();

        if(!IsSuccess && error == Error.None) throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new (true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Success<T>(T value) => new(value, true, Error.None);

    public static Result<T> Failure<T>(Error error) => new(default, false, error);

    public static Result<T> Create<T>(T? value) => value is not null ? Success(value) : Failure<T>(Error.NullValue);
}
