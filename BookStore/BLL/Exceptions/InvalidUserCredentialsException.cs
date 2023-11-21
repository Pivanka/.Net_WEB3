namespace BLL.Exceptions;

public class InvalidUserCredentialsException : Exception
{
    public ErrorType ErrorType { get; set; }

    public InvalidUserCredentialsException(string errorMessage, ErrorType errorType) : base(errorMessage)
    {
        ErrorType = errorType;
    }
}