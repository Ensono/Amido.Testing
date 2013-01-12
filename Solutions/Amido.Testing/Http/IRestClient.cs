using System.Net.Http;

namespace Amido.Testing.Http
{
    public interface IRestClient
    {
        /// <summary>
        /// Adds a query string parameter to the request url.
        /// </summary>
        /// <param name="key">The key of the querystring parameter.</param>
        /// <param name="value">The value of the querystring parameter.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        IRestClient AddQueryStringParameter(string key, string value);

        /// <summary>
        /// Add a header to the request.
        /// </summary>
        /// <param name="key">The key of the header.</param>
        /// <param name="value">The value of the header.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        IRestClient AddHeader(string key, string value);

        /// <summary>
        /// Helper method for adding an Accept header to the request.
        /// </summary>
        /// <param name="acceptHeader">The <see cref="AcceptHeader"/>.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        IRestClient AddAcceptHeader(AcceptHeader acceptHeader);

        /// <summary>
        /// Helper method for adding an authorization header to the request.
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IRestClient AddAuthorizationHeader(string scheme, string parameter);

        /// <summary>
        /// Helper method for adding a content type to the request.
        /// </summary>
        /// <param name="contentType">The <see cref="ContentType"/>.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        IRestClient AddContentType(ContentType contentType);

        /// <summary>
        /// The request body as a string.
        /// </summary>
        /// <param name="bodyString">The string to be used for the request body.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        IRestClient AddBody(string bodyString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tokens"></param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        IRestClient AddFormParameter(string key, string value, params object[] tokens);

        /// <summary>
        /// Executes the http request.
        /// </summary>
        /// <returns>A <see cref="HttpResponseMessage"/>.</returns>
        HttpResponseMessage Execute();
    }
}