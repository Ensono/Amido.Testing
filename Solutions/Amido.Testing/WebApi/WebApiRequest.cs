using System;
using System.Collections.Generic;
using System.Linq;
using Amido.Testing.WebApi.Request;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi
{
    /// <summary>
    /// Helper class for creating a <see cref="WebTestRequest"/>.
    /// </summary>
    public class WebApiRequest
    {
        #region Declarations

        private string url;
        private Verb verb;
        private readonly Dictionary<string, string> headers;
        private readonly Dictionary<string, string> formParameters;
        private string body = string.Empty;

        #endregion

        #region Construction

        /// <summary>
        /// Static method for constructing a new <see cref="WebApiRequest"/>.
        /// </summary>
        /// <param name="url">The url. Zero indexed placeholders can be used to tokenise url, exactly like using string.Format.</param>
        /// <param name="tokens">The values used to replace any zero indexed tokens.</param>
        /// <returns>Returns a new <see cref="WebApiRequest"/>.</returns>
        public static WebApiRequest Url(string url, params object[] tokens)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(url), "The url cannot be null or empty.");

            return new WebApiRequest(string.Format(url, tokens));
        }

        protected WebApiRequest(string url)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(url), "The url cannot be null or empty.");

            this.url = url;
            headers = new Dictionary<string, string>();
            formParameters = new Dictionary<string, string>();
        }

        #endregion

        #region Url

        /// <summary>
        /// Adds a query string parameter to the request url.
        /// </summary>
        /// <param name="key">The key of the querystring parameter.</param>
        /// <param name="value">The value of the querystring parameter.</param>
        /// <returns>The current instance of <see cref="WebApiRequest"/>.</returns>
        public WebApiRequest AddQueryStringParameter(string key, string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key), "The key cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");
            
            var querySeparator = "?";
            if(url.IndexOf("?", StringComparison.InvariantCulture) > -1)
            {
                querySeparator = "&";
            }

            url = string.Format("{0}{1}{2}={3}", url, querySeparator, key, value);

            return this;
        }
        
        #endregion
        
        #region Headers

        /// <summary>
        /// Sets the http verb to be used in the request.
        /// </summary>
        /// <param name="httpVerb">The <see cref="Verb"/>.</param>
        /// <returns>The current instance of <see cref="WebApiRequest"/>.</returns>
        public WebApiRequest WithVerb(Verb httpVerb)
        {
            verb = httpVerb;
            return this;
        }

        /// <summary>
        /// Add a header to the request.
        /// </summary>
        /// <param name="key">The key of the header.</param>
        /// <param name="value">The value of the header.</param>
        /// <returns>The current instance of <see cref="WebApiRequest"/>.</returns>
        public WebApiRequest AddHeader(string key, string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key), "The key cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");
            
            headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// Helper method for adding an Accept header to the request.
        /// </summary>
        /// <param name="acceptHeader">The <see cref="AcceptHeader"/>.</param>
        /// <returns>The current instance of <see cref="WebApiRequest"/>.</returns>
        public WebApiRequest AddAcceptHeader(AcceptHeader acceptHeader)
        {
            string headerValue = string.Empty;
            switch (acceptHeader)
            {
                case AcceptHeader.Json:
                    headerValue += "application/json";
                    break;
                case AcceptHeader.Xml:
                    headerValue = "text/xml";
                    break;
            }
            headers.Add("Accept", headerValue);
            return this;
        }

        /// <summary>
        /// Helper method for adding a content type to the request.
        /// </summary>
        /// <param name="contentType">The <see cref="ContentType"/>.</param>
        /// <returns>The current instance of <see cref="WebApiRequest"/>.</returns>
        public WebApiRequest AddContentType(ContentType contentType)
        {
            string contentTypeString;
            switch (contentType)
            {
                case ContentType.Json:
                    contentTypeString = "application/json";
                    break;
                case ContentType.Xml:
                    contentTypeString = "text/xml";
                    break;
                default:
                    contentTypeString = "application/x-www-form-urlencoded";
                    break;
            }

            headers.Add("Content-Type", contentTypeString);
            return this;
        }

        #endregion
        
        #region Request Body

        /// <summary>
        /// The request body as a string.
        /// </summary>
        /// <param name="bodyString">The string to be used for the request body.</param>
        /// <returns>The current instance of <see cref="WebApiRequest"/>.</returns>
        public WebApiRequest AddBody(string bodyString)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(bodyString), "The body string cannot be null or empty.");

            body = bodyString;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tokens"></param>
        /// <returns>The current instance of <see cref="WebApiRequest"/>.</returns>
        public WebApiRequest AddFormParameter(string key, string value, params object[] tokens)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key), "The key cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");
            
            formParameters.Add(key, string.Format(value, tokens));
            return this;
        } 

        #endregion

        #region Certificates

        /// <summary>
        /// Helper method for overriding the certificate validation callback result.
        /// </summary>
        /// <param name="validationResult">the value to return on the certificate validation callback.</param>
        /// <returns>The current instance of <see cref="WebApiRequest"/>.</returns>
        public WebApiRequest SetCertificationValidation(bool validationResult)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => validationResult);
            return this;
        } 

        #endregion

        #region Create Request

        /// <summary>
        /// Creates a <see cref="WebTestRequest"/> using the properties and values of the configured <see cref="WebApiRequest"/>.
        /// </summary>
        /// <returns>A newly created and populated <see cref="WebTestRequest"/>.</returns>
        public WebTestRequest Create()
        {
            var request = CreateRequest(url);

            if (verb == Verb.POST || verb == Verb.PUT)
            {
                AddRequestBody(request);
            }

            return request;
        } 

        #endregion

        #region Private

        private void AddRequestBody(WebTestRequest request)
        {
            if (headers["Content-Type"] == "application/x-www-form-urlencoded")
            {
                var formBody = new FormPostHttpBody();

                foreach (var formParameter in formParameters)
                {
                    formBody.FormPostParameters.Add(formParameter.Key, formParameter.Value);
                }
                request.Body = formBody;
            }
            else
            {
                request.Body = new StringHttpBody { BodyString = body, ContentType = headers["Content-Type"] };
            }

        }

        private WebTestRequest CreateRequest(String requestUrl)
        {
            var request = new WebTestRequest(requestUrl) { Method = verb.ToString() };
            SetRequestHeaders(request);
            return request;
        }

        private void SetRequestHeaders(WebTestRequest request)
        {
            foreach (var header in headers.Where(x => x.Key != "Content-Type"))
            {
                request.Headers.Add(header.Key, header.Value);
            }
        } 

        #endregion
    }
}
