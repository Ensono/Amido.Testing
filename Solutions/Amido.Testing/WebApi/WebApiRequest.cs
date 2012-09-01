using System;
using System.Collections.Generic;
using System.Linq;
using Amido.Testing.WebApi.Request;
using Microsoft.VisualStudio.TestTools.WebTesting;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi
{
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

        public WebApiRequest WithVerb(Verb httpVerb)
        {
            verb = httpVerb;
            return this;
        }

        public WebApiRequest AddHeader(string key, string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key), "The key cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");
            
            headers.Add(key, value);
            return this;
        }

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

        public WebApiRequest AddBody(string bodyString)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(bodyString), "The body string cannot be null or empty.");

            body = bodyString;
            return this;
        }

        public WebApiRequest AddFormParameter(string key, string value, params object[] tokens)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key), "The key cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");
            
            formParameters.Add(key, string.Format(value, tokens));
            return this;
        } 

        #endregion

        #region Certificates

        public void SetCertificationValidation(bool validationResult)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => validationResult);
        } 

        #endregion

        #region Create Request

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

        protected WebTestRequest CreateRequest(String requestUrl)
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
