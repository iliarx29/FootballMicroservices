namespace Auth.Application.Results;
internal class ResultException : Exception
{
    public ResultException(string? message) : base(message)
    {
    }
}
