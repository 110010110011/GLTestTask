using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace TestTaskGL2
{
    class Task2
    {
        HttpClient httpClient;
        readonly string url = "http://api.mathjs.org/v4/";

        [SetUp]
        public void Initialization()
        {
            httpClient = new HttpClient();
        }

        [Test]
        public void Test()
        {
            AssertGetResponse("?expr=2*(7-3)", "8", 200);
            AssertGetResponse("?expr=2/0", "Infinity", 200);
            AssertGetResponse("qwe", "Error: Required query parameter \"expr\" missing in url.", 400);
            var expr = new string[]
            {
                "a = 1.2 * (2 + 4.5)",
                "a / 2"
            };

            var expected = new string[] { "7.8", "3.9" };
            AssertPostResponse(expr, 10, expected, 200);
            expected = new string[] { "0e+11", "0e+11" };
            AssertPostResponse(expr, -10, expected, 200);
            expr = new string[]
            {
                "a = 2 * 2",
                "fake expr"
            };
            AssertPostResponse(expr, 10, expected, 400);
        }

        [TearDown]
        public void Dispose()
        {
            httpClient.Dispose();
        }

        private void AssertGetResponse(string expr, string expected, int statusCode)
        {
            var response = httpClient.GetAsync(url + expr);
            var result = response.Result.Content.ReadAsStringAsync().Result;
            switch (statusCode)
            {
                case 200:
                    Assert.AreEqual(statusCode, (int)response.Result.StatusCode);
                    Assert.AreEqual(expected, result);
                    break;
                case 400:
                    Assert.AreEqual(statusCode, (int)response.Result.StatusCode);
                    Assert.AreEqual(expected, result);
                    break;
                default:
                    break;
            }

        }

        private void AssertPostResponse(string[] expresion, int precision, string[] expected, int statusCode)
        {
            var postBody = new PostBody
            {
                Expr = expresion,
                Precision = precision
            };

            var json = JsonSerializer.Serialize(postBody);
            var strContent = new StringContent(json);

            var response = httpClient.PostAsync(url, strContent);

            var body = response.Result.Content.ReadAsStringAsync().Result;
            
            switch (statusCode)
            {
                case 200:
                    Assert.AreEqual(statusCode, (int)response.Result.StatusCode);
                    var result = JsonSerializer.Deserialize<ResponseBody>(body);

                    for (int i = 0; i < result.Result.Length; i++)
                    {
                        Assert.AreEqual(expected[i], result.Result[i]);
                    }

                    Assert.IsNull(result.Error);
                    break;
                case 400:
                    Assert.AreEqual(statusCode, (int)response.Result.StatusCode);
                    break;
                default:
                    break;
            }

        }
    }
}
