﻿using Reddit.Api.Exceptions;
using Reddit.Api.Extensions;
using Reddit.Api.Json.Interfaces;
using System.Diagnostics;
using System.Net.Http.Json;

namespace Reddit.Api.Json
{
    public class JsonClient : IJsonClient
    {
        private readonly HttpClient _httpClient;

        public JsonClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            this.SetDefaultHeader("Accept", "application/json");
        }

        public async Task<T> Get<T>(string url) where T : class
        {
            string response = await Retry(async () =>
            {
                HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);
                string response = await responseMessage.Content.ReadAsStringAsync();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new EntityNotFoundException(url, response);
                    }
                    else if(responseMessage.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        throw new TooManyRequestsException(url, response);
                    }
                    else
                    {
                        throw new RemoteException(url, response, responseMessage.StatusCode);
                    }
                }

                return response;
            });

            return JsonDeserializer.Deserialize<T>(response);
        }

        public async Task<T> Post<T>(string url, object body) where T : class
        {
            JsonContent content = JsonContent.Create(body);

            HttpResponseMessage responseMessage = await _httpClient.PostAsync(url, content);

            string response = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RemoteException(url, response, responseMessage.StatusCode);
            }

            return JsonDeserializer.Deserialize<T>(response);
        }

        public async Task Post(string url)
        {
            HttpResponseMessage responseMessage = await _httpClient.PostAsync(url, null);

            string response = await responseMessage.Content.ReadAsStringAsync();

            Debug.WriteLine(response);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RemoteException(url, response, responseMessage.StatusCode);
            }
        }

        public async Task Post(string url, object body)
        {
            JsonContent content = JsonContent.Create(body);

            HttpResponseMessage responseMessage = await _httpClient.PostAsync(url, content);

            string response = await responseMessage.Content.ReadAsStringAsync();

            Debug.WriteLine(response);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RemoteException(url, response, responseMessage.StatusCode);
            }
        }

        public async Task Post(string url, Dictionary<string, string> formValues)
        {
            // Create the content to be posted as form data
            FormUrlEncodedContent content = new(formValues);

            // Post the form data to the specified URL
            HttpResponseMessage responseMessage = await _httpClient.PostAsync(url, content);

            // Read the response content as a string
            string response = await responseMessage.Content.ReadAsStringAsync();

            Debug.WriteLine(response);

            // Ensure the response indicates success
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RemoteException(url, response, responseMessage.StatusCode);
            }
        }

        public async Task<T> Post<T>(string url, Dictionary<string, string> formValues) where T : class
        {
            // Create the content to be posted as form data
            FormUrlEncodedContent content = new(formValues);

            // Post the form data to the specified URL
            HttpResponseMessage responseMessage = await _httpClient.PostAsync(url, content);

            // Read the response content as a string
            string response = await responseMessage.Content.ReadAsStringAsync();

            // Ensure the response indicates success
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RemoteException(url, response, responseMessage.StatusCode);
            }

            return JsonDeserializer.Deserialize<T>(response);
        }

        public void SetDefaultHeader(string key, string value)
        {
            _httpClient.SetDefaultHeader(key, value);
        }

        private static T Retry<T>(Func<T> func, int maxRetries = 3)
        {
            int retries = 0;

            while (true)
            {
                try
                {
                    return func();
                }
                catch(TooManyRequestsException)
                {
                    Thread.Sleep(1000);
                    Debug.WriteLine("Too many requests.  Retrying...");
                }
                catch (RemoteException e) when (e.HttpStatusCode is System.Net.HttpStatusCode.ServiceUnavailable or System.Net.HttpStatusCode.NotFound)
                {
                    if (retries++ > maxRetries)
                    {
                        throw;
                    }

                    Thread.Sleep(retries++ * 1000);

                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}