using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BF.Demo.FaceAPI
{
    internal class FaceAPIClient
    {
        #region -- URI & Subscription Key --

        private const string API_BASE_URI = "[ENDPOINT]";
        private const string API_SUBSCRIPTION_KEY = "[SUBSCRIPTION_KEY]";

        #endregion -- Subscription Key & URI --

        private HttpClient client;

        internal FaceAPIClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", API_SUBSCRIPTION_KEY);
        }

        internal async Task<string> DetectAsync(byte[] image, string attributes = null)
        {
            var requestParameters =
                "returnFaceId=true"
                + "&returnFaceLandmarks=false"
                + (!string.IsNullOrEmpty(attributes) ? $"&returnFaceAttributes={attributes}" : string.Empty);

            var uri = $"{API_BASE_URI}/detect?{requestParameters}";

            using (var content = new ByteArrayContent(image))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await client.PostAsync(uri, content);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}