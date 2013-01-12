namespace Amido.Testing.Http
{
    public enum RetryType
    {
        UntilStatusCodeEquals,
        UntilBodyIncludes,
        UntilBodyDoesNotInclude
    }
}
