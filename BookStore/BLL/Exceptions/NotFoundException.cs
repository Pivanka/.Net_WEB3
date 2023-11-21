namespace BLL.Exceptions;

public class NotFoundException : Exception
{
    public ErrorType ErrorType { get; set; }

    public NotFoundException(string errorMessage, ErrorType errorType) : base(errorMessage)
    {
        ErrorType = errorType;
    }
}