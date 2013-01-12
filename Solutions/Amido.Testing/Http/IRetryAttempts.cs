namespace Amido.Testing.Http
{
    public interface IRetryAttempts
    {
        IVerb NoRetries();
        IVerb WithRetries(RetryType retryType, object retryParameter, int maxRetries, int interval);
    }
}