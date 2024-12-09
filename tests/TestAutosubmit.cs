using System.Net;
using Moq;
using Moq.Protected;

using aoc16;

namespace tests;

public class TestAutosubmit
{
  private readonly string ResultsFile = Path.GetTempFileName();

  ~TestAutosubmit()
  {
    if (File.Exists(ResultsFile)) File.Delete(ResultsFile);
  }

  private static HttpClient MockHttpClientWithResponse(string content)
  {
    var mockHttp = new Mock<HttpMessageHandler>();
    var response = new HttpResponseMessage(HttpStatusCode.OK);
    response.Content = new StringContent(content);
    mockHttp.Protected().Setup<HttpResponseMessage>(
        "Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
      .Returns(response);
    return new HttpClient(mockHttp.Object);
  }

  private static HttpClient MockHttpClientWithError()
  {
    var mockHttp = new Mock<HttpMessageHandler>();
    var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
    mockHttp.Protected().Setup<HttpResponseMessage>(
        "Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
      .Returns(response);
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
  public void TestWrongLevel()
  {
    var mockHttp = MockHttpClientWithResponse("<html><p>You don't seem to be solving the right level.</p></html>");
    var result = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
    Assert.Equal(Autosubmit.Result.WRONG_LEVEL, result);
  }

  [Fact]
  public void TestWrongLevel_DoesNotCache()
  {
    var mockHttp = MockHttpClientWithResponse("<html><p>You don't seem to be solving the right level.</p></html>");
    var result = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
    Assert.Equal(Autosubmit.Result.WRONG_LEVEL, result);
    mockHttp = MockHttpClientWithResponse("<html><p>That's the right answer</p></html>");
    result = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
    Assert.Equal(Autosubmit.Result.ACCEPTED, result);
  }

  [Fact]
  public void TestRejectedTooLow()
  {
    var mockHttp =
      MockHttpClientWithResponse("<html><p>That's not the right answer - your answer is too low.");
    var result = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
    Assert.Equal(Autosubmit.Result.REJECTED_TOO_LOW, result);
  }

  [Fact]
  public void TestRejectedTooHigh()
  {
    var mockHttp =
      MockHttpClientWithResponse("<html><p>That's not the right answer - your answer is too high.");
    var result = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
    Assert.Equal(Autosubmit.Result.REJECTED_TOO_HIGH, result);
  }

  [Fact]
  public void TestDoesNotResubmitRejectedResult()
  {
    var mockHttp = MockHttpClientWithResponse("<html><p>That's not the right answer</p></html>");
    var resultFirst = Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { });
    Assert.Equal(Autosubmit.Result.REJECTED, resultFirst);
    var resultSecond =
      Autosubmit.Submit(7, 1, "42", MockHttpClientWithError(), ResultsFile, _ => { });
    Assert.Equal(Autosubmit.Result.REJECTED, resultSecond);
  }

  [Fact]
  public void TestMultipleRejectedResults()
  {
    var mockHttp = MockHttpClientWithResponse("<html><p>That's not the right answer</p></html>");
    Assert.Equal(Autosubmit.Result.REJECTED,
      Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { }));
    Assert.Equal(Autosubmit.Result.REJECTED,
      Autosubmit.Submit(7, 1, "43", mockHttp, ResultsFile, _ => { }));
    Assert.Equal(Autosubmit.Result.REJECTED,
      Autosubmit.Submit(7, 1, "44", mockHttp, ResultsFile, _ => { }));
    Assert.Equal(Autosubmit.Result.REJECTED,
      Autosubmit.Submit(7, 1, "42", MockHttpClientWithError(), ResultsFile, _ => { }));
    Assert.Equal(Autosubmit.Result.REJECTED,
      Autosubmit.Submit(7, 1, "43", MockHttpClientWithError(), ResultsFile, _ => { }));
    Assert.Equal(Autosubmit.Result.REJECTED,
      Autosubmit.Submit(7, 1, "44", MockHttpClientWithError(), ResultsFile, _ => { }));
  }

  [Fact]
  public void TestChecksUpperBound()
  {
    var mockHttp =
      MockHttpClientWithResponse("<html><p>That's not the right answer - your answer is too high.");
    Assert.Equal(Autosubmit.Result.REJECTED_TOO_HIGH,
      Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { }));
    Assert.Equal(Autosubmit.Result.REJECTED_TOO_HIGH,
      Autosubmit.Submit(7, 1, "40", mockHttp, ResultsFile, _ => { }));
    Assert.Equal(Autosubmit.Result.REJECTED_TOO_HIGH,
      Autosubmit.Submit(7, 1, "50", MockHttpClientWithError(), ResultsFile, _ => { }));
  }

  [Fact]
  public void TestChecksLowerBound()
  {
    var mockHttp =
      MockHttpClientWithResponse("<html><p>That's not the right answer - your answer is too low.");
    Assert.Equal(Autosubmit.Result.REJECTED_TOO_LOW,
      Autosubmit.Submit(7, 1, "42", mockHttp, ResultsFile, _ => { }));
    Assert.Equal(Autosubmit.Result.REJECTED_TOO_LOW,
      Autosubmit.Submit(7, 1, "44", mockHttp, ResultsFile, _ => { }));
    Assert.Equal(Autosubmit.Result.REJECTED_TOO_LOW,
      Autosubmit.Submit(7, 1, "30", MockHttpClientWithError(), ResultsFile, _ => { }));
  }

  [Theory]
  [InlineData("You have 48s left to wait", 48)]
  [InlineData("You have 5m 31s left to wait", 331)]
  public void TestBackoff(string prompt, int expectedDelay)
  {
    var mockHttp = new Mock<HttpMessageHandler>();
    var responseWithTimeout = new HttpResponseMessage(HttpStatusCode.OK);
    responseWithTimeout.Content = new StringContent("<html><p>You gave an answer too recently. " + prompt);
    var responseWithReply = new HttpResponseMessage(HttpStatusCode.OK);
    responseWithReply.Content = new StringContent("<html><p>That's the right answer</p></html>");
    int responseCount = 0;
    mockHttp.Protected().Setup<HttpResponseMessage>(
        "Send", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
      .Callback(() => responseCount += 1)
      .Returns(() => responseCount >= 2 ? responseWithReply : responseWithTimeout);

    int? actualDelay = null;
    Assert.Equal(Autosubmit.Result.ACCEPTED,
      Autosubmit.Submit(7, 1, "42", new HttpClient(mockHttp.Object), ResultsFile, delay => actualDelay = delay));

    Assert.Equal(expectedDelay, actualDelay);
  }
}