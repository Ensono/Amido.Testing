namespace Amido.Testing.WebApi.Request
{
    /// <summary>
    /// Retry test type enumeration.
    /// </summary>
    public enum RetryTestType
    {
        /// <summary>
        /// Retry until the body equals the specified value.
        /// </summary>
        BodyEquals,

        /// <summary>
        /// Retry until the body includes the specified value.
        /// </summary>
        BodyIncludes,

        /// <summary>
        /// Retry until the body does not include the specified value.
        /// </summary>
        BodyDoesNotInclude,

        /// <summary>
        /// Retry until the status code equals the specified value.
        /// </summary>
        StatusCodeEquals
    }
}
