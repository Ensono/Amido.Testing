namespace Amido.Testing.Http
{
    public interface IRetryAttempts
    {
        IVerb WithoutRetries();
        IVerb WithRetries(RetryType retryType, object retryParameter, int maxRetries, int interval);
    }
}