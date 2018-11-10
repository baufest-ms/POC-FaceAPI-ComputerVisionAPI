using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BF.Demo.ComputerVisionAPI
{
    internal class ComputerVisionAPIClient
    {
        #region -- URI & Subscription Key --

        private const string API_BASE_URI = "[ENDPOINT]";
        private const string API_SUBSCRIPTION_KEY = "[SUBSCRIPTION_KEY]";

        #endregion -- Subscription Key & URI --

        private HttpClient client;

        internal ComputerVisionAPIClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", API_SUBSCRIPTION_KEY);
        }

        internal async Task<string> AnalyzeImageAsync(byte[] image, string features = null, string details = null)
        {
            var requestParameters = "language=en"
                + (!string.IsNullOrEmpty(features) ? $"&visualFeatures={features}" : string.Empty)
                + (!string.IsNullOrEmpty(details) ? $"&details={details}" : string.Empty);

            var uri = $"{API_BASE_URI}/analyze?{requestParameters}";

            using (var content = new ByteArrayContent(image))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await client.PostAsync(uri, content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        internal async Task<string> DescribeImageAsync(byte[] image)
        {
            var requestParameters = "maxCandidates=5";

            var uri = $"{API_BASE_URI}/describe?{requestParameters}";

            using (var content = new ByteArrayContent(image))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await client.PostAsync(uri, content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        internal async Task<string> OCRAsync(byte[] image)
        {
            var requestParameters = "language=en"
                + "&detectOrientation=true";

            var uri = $"{API_BASE_URI}/ocr?{requestParameters}";

            using (var content = new ByteArrayContent(image))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await client.PostAsync(uri, content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        internal async Task<string> RecognizeTextAsync(byte[] image, string mode)
        {
            var requestParameters = $"mode={mode}";

            var uri = $"{API_BASE_URI}/recognizeText?{requestParameters}";

            using (var content = new ByteArrayContent(image))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await client.PostAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                var operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();

                string recognizeText;
                int i = 0;
                do
                {
                    Thread.Sleep(1000);

                    response = await client.GetAsync(operationLocation);
                    recognizeText = await response.Content.ReadAsStringAsync();
                    ++i;
                }
                while (i < 10 && recognizeText.IndexOf("\"status\":\"Succeeded\"") == -1);

                if (i == 10 && recognizeText.IndexOf("\"status\":\"Succeeded\"") == -1)
                {
                    return "Timeout error trying to get the recognized text";
                }

                return recognizeText;
            }
        }

    }
}