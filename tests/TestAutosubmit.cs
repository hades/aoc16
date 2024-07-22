using aoc16;
using Moq;
using Moq.Protected;
using System.Net;

namespace tests
{
    public class TestAutosubmit
    {
        private readonly string ResultsFile = Path.GetTempFileName();

        ~TestAutosubmit() {
            if (File.Exists(ResultsFile))
            {
                File.Delete(ResultsFile);
            }
        }

        private static HttpClient MockHttpClientWithResponse(string content)
        {
            var mockHttp = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(content);
            mockHttp.Protected().Setup<HttpResponseMessage>(
                "Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(response);
            return new HttpClient(mockHttp.Object);
        }

        private static HttpClient MockHttpClientWithError()
        {
            var mockHttp = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            mockHttp.Protected().Setup<HttpResponseMessage>(
                "Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(response);
            return new HttpClient(mockHttp.Object);
        }

        [Fact]
        public void TestAcceptedResult()
        {
            var mockHttp = MockHttpClientWithResponse("<html><p>That's the right answer</p></html>");
            var result = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
            Assert.Equal(Autosubmit.Result.ACCEPTED, result);
        }

        [Fact]
        public void TestRejectedResult()
        {
            var mockHttp = MockHttpClientWithResponse("<html><p>That's not the right answer</p></html>");
            var result = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
            Assert.Equal(Autosubmit.Result.REJECTED, result);
        }

        [Fact]
        public void TestRejectedTooLow()
        {
            var mockHttp = MockHttpClientWithResponse("<html><p>That's not the right answer - your answer is too low.");
            var result = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
            Assert.Equal(Autosubmit.Result.REJECTED_TOO_LOW, result);
        }

        [Fact]
        public void TestRejectedTooHigh()
        {
            var mockHttp = MockHttpClientWithResponse("<html><p>That's not the right answer - your answer is too high.");
            var result = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
            Assert.Equal(Autosubmit.Result.REJECTED_TOO_HIGH, result);
        }

        [Fact]
        public void TestDoesNotResubmitRejectedResult()
        {
            var mockHttp = MockHttpClientWithResponse("<html><p>That's not the right answer</p></html>");
            var resultFirst = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
            Assert.Equal(Autosubmit.Result.REJECTED, resultFirst);
            var resultSecond = Autosubmit.Submit(7, 1, "42", MockHttpClientWithError(), ResultsFile, _ => { });
            Assert.Equal(Autosubmit.Result.REJECTED, resultSecond);
        }

        [Fact]
        public void TestMultipleRejectedResults()
        {
            var mockHttp = MockHttpClientWithResponse("<html><p>That's not the right answer</p></html>");
            Assert.Equal(Autosubmit.Result.REJECTED, Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { }));
            Assert.Equal(Autosubmit.Result.REJECTED, Autosubmit.Submit(7, 1, "43", mockHttp, ResultsFile, _ => { }));
            Assert.Equal(Autosubmit.Result.REJECTED, Autosubmit.Submit(7, 1, "44", mockHttp, ResultsFile, _ => { }));
            Assert.Equal(Autosubmit.Result.REJECTED, Autosubmit.Submit(7, 1, "42", MockHttpClientWithError(), ResultsFile, _ => { }));
            Assert.Equal(Autosubmit.Result.REJECTED, Autosubmit.Submit(7, 1, "43", MockHttpClientWithError(), ResultsFile, _ => { }));
            Assert.Equal(Autosubmit.Result.REJECTED, Autosubmit.Submit(7, 1, "44", MockHttpClientWithError(), ResultsFile, _ => { }));
        }

        [Fact]
        public void TestChecksUpperBound()
        {
            var mockHttp = MockHttpClientWithResponse("<html><p>That's not the right answer - your answer is too high.");
            Assert.Equal(Autosubmit.Result.REJECTED_TOO_HIGH, Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { }));
            Assert.Equal(Autosubmit.Result.REJECTED_TOO_HIGH, Autosubmit.Submit(7, 1, "40", mockHttp, ResultsFile, _ => { }));
            Assert.Equal(Autosubmit.Result.REJECTED_TOO_HIGH, Autosubmit.Submit(7, 1, "50", MockHttpClientWithError(), ResultsFile, _ => { }));
        }

        [Fact]
        public void TestChecksLowerBound()
        {
            var mockHttp = MockHttpClientWithResponse("<html><p>That's not the right answer - your answer is too low.");
            Assert.Equal(Autosubmit.Result.REJECTED_TOO_LOW, Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { }));
            Assert.Equal(Autosubmit.Result.REJECTED_TOO_LOW, Autosubmit.Submit(7, 1, "44", mockHttp, ResultsFile, _ => { }));
            Assert.Equal(Autosubmit.Result.REJECTED_TOO_LOW, Autosubmit.Submit(7, 1, "30", MockHttpClientWithError(), ResultsFile, _ => { }));
        }
    }
}