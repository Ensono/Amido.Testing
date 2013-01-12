using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Amido.Testing.Dbc;
using System.Threading;

namespace Amido.Testing.Http
{
    public class RestClient : IRestClient, IVerb, IRetryAttempts
    {
        #region Declarations

        private readonly List<HttpRequestMessage> httpRequestMessageList;
        private string url;
        private readonly Dictionary<string, string> formParameters;
        private string contentTypeString;
        private RetryType RetryType;
        private object RetryParameter;
        private int MaxRetries;
        private int Interval;

        #endregion

        #region Construction

        public static IRetryAttempts RequestUrl(string url, params object[] tokens)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(url), "The url cannot be null or empty.");

            return new RestClient(string.Format(url, tokens));
        }

        protected RestClient(string url)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(url), "The url cannot be null or empty.");
            SetCertificationValidation(true);
            this.url = url;
            formParameters = new Dictionary<string, string>();
            httpRequestMessageList = new List<HttpRequestMessage>();
        }

        #endregion

        public IVerb NoRetries()
        {
            httpRequestMessageList.Add(new HttpRequestMessage());
            MaxRetries = 1;
            return this;
        }

        public IVerb WithRetries(RetryType retryType, object retryParameter, int maxRetries, int interval)
        {
            RetryType = retryType;
            RetryParameter = retryParameter;
            MaxRetries = maxRetries;
            Interval = interval;

            for (var i = 0; i < maxRetries; i++)
            {
                httpRequestMessageList.Add(new HttpRequestMessage());
            }

            return this;
        }

        #region Url

        /// <summary>
        /// Adds a query string parameter to the request url.
        /// </summary>
        /// <param name="key">The key of the querystring parameter.</param>
        /// <param name="value">The value of the querystring parameter.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        public virtual IRestClient AddQueryStringParameter(string key, string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key), "The key cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");

            var querySeparator = "?";
            if (url.IndexOf("?", StringComparison.InvariantCulture) > -1)
            {
                querySeparator = "&";
            }

            url = string.Format("{0}{1}{2}={3}", url, querySeparator, key, value);

            return this;
        }

        #endregion

        #region Headers

        public IRestClient WithVerb(HttpMethod httpMethod)
        {
            foreach (var httpRequestMessage in httpRequestMessageList)
            {
                httpRequestMessage.Method = httpMethod;
            }
            return this;
        }

        /// <summary>
        /// Add a header to the request.
        /// </summary>
        /// <param name="key">The key of the header.</param>
        /// <param name="value">The value of the header.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        public virtual IRestClient AddHeader(string key, string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key), "The key cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");

            foreach (var httpRequestMessage in httpRequestMessageList)
            {
                httpRequestMessage.Headers.Add(key, value);
            }
            
            return this;
        }

        /// <summary>
        /// Helper method for adding an Accept header to the request.
        /// </summary>
        /// <param name="acceptHeader">The <see cref="AcceptHeader"/>.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        public virtual IRestClient AddAcceptHeader(AcceptHeader acceptHeader)
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

            foreach (var httpRequestMessage in httpRequestMessageList)
            {
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(headerValue));
            }

            return this;
        }

        public IRestClient AddAuthorizationHeader(string scheme, string parameter)
        {
            foreach (var httpRequestMessage in httpRequestMessageList)
            {
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(scheme, parameter);
            }
            
            return this;
        }

        /// <summary>
        /// Helper method for adding a content type to the request.
        /// </summary>
        /// <param name="contentType">The <see cref="ContentType"/>.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        public virtual IRestClient AddContentType(ContentType contentType)
        {
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
            return this;
        }

        #endregion

        #region Request Body

        /// <summary>
        /// The request body as a string.
        /// </summary>
        /// <param name="bodyString">The string to be used for the request body.</param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        public virtual IRestClient AddBody(string bodyString)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(bodyString), "The body string cannot be null or empty.");

            foreach (var httpRequestMessage in httpRequestMessageList)
            {
                httpRequestMessage.Content = new StringContent(bodyString);
            }
           
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tokens"></param>
        /// <returns>The current instance of <see cref="RestClient"/>.</returns>
        public virtual IRestClient AddFormParameter(string key, string value, params object[] tokens)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(key), "The key cannot be null or empty.");
            Contract.Requires(!string.IsNullOrWhiteSpace(value), "The value cannot be null or empty.");

            formParameters.Add(key, string.Format(value, tokens));
            return this;
        }

        #endregion

        public virtual HttpResponseMessage Execute()
        {
            HttpResponseMessage httpResponseMessage = null;
            
            for(var currentRetryIndex = 0; currentRetryIndex < MaxRetries; currentRetryIndex++)
            {
                var httpRequestMessage = httpRequestMessageList[currentRetryIndex];

                using (var httpClient = new HttpClient())
                {
                    httpRequestMessage.RequestUri = new Uri(url);

                    if (!string.IsNullOrWhiteSpace(contentTypeString))
                    {
                        if (contentTypeString == "application/x-www-form-urlencoded")
                        {
                            httpRequestMessage.Content = new FormUrlEncodedContent(formParameters);
                        }

                        if (httpRequestMessage.Content != null)
                        {
                            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentTypeString);
                        }
                    }

                    httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result;

                    if(IsFinalRetryRequest(currentRetryIndex))
                    {
                        return httpResponseMessage;
                    }

                    if (IsRetryConditionSatisfied(httpResponseMessage, RetryType, RetryParameter))
                    {
                        return httpResponseMessage;
                    }
                }
            }

            return httpResponseMessage;
        }

        private bool IsFinalRetryRequest(int currentRetryIndex)
        {
            return currentRetryIndex == MaxRetries - 1;
        }

        private static bool IsRetryConditionSatisfied(HttpResponseMessage httpResponseMessage, RetryType retryType, object retryParameter)
        {
            switch (retryType)
            {
                case RetryType.UntilStatusCodeEquals:
                    {
                        if (httpResponseMessage.StatusCode == (HttpStatusCode)retryParameter)
                        {
                            return true;
                        }
                        break;
                    }
                case RetryType.UntilBodyIncludes:
                    {
                        if (httpResponseMessage.Content.ReadAsStringAsync().Result.Contains(retryParameter.ToString()))
                        {
                            return true;
                        }
                        break;
                    }
                case RetryType.UntilBodyDoesNotInclude:
                    {
                        if (!httpResponseMessage.Content.ReadAsStringAsync().Result.Contains(retryParameter.ToString()))
                        {
                            return true;
                        }
                        break;
                    }
            }

            return false;
        }


        private IRestClient SetCertificationValidation(bool validationResult)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => validationResult);
            return this;
        }
    }
}
