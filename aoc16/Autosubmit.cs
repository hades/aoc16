using System.Drawing;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;
using Tomlyn;
using Tomlyn.Model;
using Console = Colorful.Console;

namespace aoc16;

public class Autosubmit
{
  public enum Result
  {
    ACCEPTED,
    REJECTED,
    REJECTED_TOO_LOW,
    REJECTED_TOO_HIGH,
    WRONG_LEVEL
  }

  public static Result Submit(int day, int part, string answer, HttpClient httpClient,
    string resultsFile, Action<int> delayFunc)
  {
    var existingResults =
      Toml.ToModel(File.Exists(resultsFile) ? File.ReadAllText(resultsFile) : "");
    var key = $"day{day}part{part}";
    if (!existingResults.ContainsKey(key)) existingResults.Add(key, new TomlTable());
    var problemResults = (TomlTable)existingResults[key];
    if (problemResults.ContainsKey("accepted"))
    {
      var acceptedAnswer = (string)problemResults["accepted"];
      Console.WriteLine($"an answer has already been accepted: {acceptedAnswer}",
        ColorTranslator.FromHtml("#bdbdbd"));
      return acceptedAnswer == answer ? Result.ACCEPTED : Result.REJECTED;
    }

    var (resultFromBounds, lowerBound, upperBound) = AnswerBounds(answer, problemResults);
    if (resultFromBounds is not null) return (Result)resultFromBounds;
    if (problemResults.ContainsKey("rejected"))
    {
      var rejectedResults = (TomlArray)problemResults["rejected"];
      if (rejectedResults.Where(x => (string?)x == answer).Any()) return Result.REJECTED;
    }

    var result = SubmitToServer(day, part, answer, httpClient, delayFunc);
    if (result == Result.WRONG_LEVEL) {
      return result;
    }
    if (result == Result.REJECTED)
    {
      problemResults.TryAdd("rejected", new TomlArray());
      ((TomlArray)problemResults["rejected"]).Add(answer);
    }
    else if (result == Result.ACCEPTED)
    {
      problemResults.Add("accepted", answer);
    }
    else if (result == Result.REJECTED_TOO_LOW)
    {
      problemResults.Remove("lower_bound");
      problemResults.Add("lower_bound", Math.Max(long.Parse(answer), lowerBound ?? long.MinValue));
    }
    else if (result == Result.REJECTED_TOO_HIGH)
    {
      problemResults.Remove("upper_bound");
      problemResults.Add("upper_bound", Math.Min(long.Parse(answer), upperBound ?? long.MaxValue));
    }

    File.WriteAllText(resultsFile, Toml.FromModel(existingResults));
    return result;
  }

  private static (Result?, long?, long?) AnswerBounds(string answer, TomlTable problemResults)
  {
    var isAnswerLong = long.TryParse(answer, out var answerAsLong);
    if (!isAnswerLong) return (null, null, null);
    long? upperBoundResult = null;
    long? lowerBoundResult = null;
    if (problemResults.ContainsKey("upper_bound"))
    {
      upperBoundResult = (long)problemResults["upper_bound"];
      if (answerAsLong >= upperBoundResult)
      {
        Console.WriteLine($"exceeded previous upper bound of {upperBoundResult}",
          ColorTranslator.FromHtml("#bdbdbd"));
        return (Result.REJECTED_TOO_HIGH, null, null);
      }
    }

    if (problemResults.ContainsKey("lower_bound"))
    {
      lowerBoundResult = (long)problemResults["lower_bound"];
      if (answerAsLong <= lowerBoundResult)
      {
        Console.WriteLine($"not reached previous lower bound of {lowerBoundResult}",
          ColorTranslator.FromHtml("#bdbdbd"));
        return (Result.REJECTED_TOO_LOW, null, null);
      }
    }

    return (null, lowerBoundResult, upperBoundResult);
  }

  private static Result SubmitToServer(int day, int part, string answer, HttpClient httpClient,
    Action<int> delayFunc)
  {
    var url = $"https://adventofcode.com/2016/day/{day}/answer";
    var request = new HttpRequestMessage(HttpMethod.Post, url);
    request.Content = new StringContent($"level={part}&answer={HttpUtility.UrlEncode(answer)}");
    request.Content.Headers.ContentType =
      new MediaTypeHeaderValue(MediaTypeNames.Application.FormUrlEncoded);
    var httpResponse = httpClient.Send(request);
    httpResponse.EnsureSuccessStatusCode();
    var response = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    if (response.Contains("You gave an answer too recently"))
    {
      var m = new Regex("You have (?:(\\d+)m )?(\\d+)s left to wait").Match(response);
      if (!m.Success) throw new InvalidOperationException($"failed to parse response: {response}");
      var (minutes, seconds) = (m.Groups[1].Value, m.Groups[2].Value);
      var timeout = (minutes.Length > 0 ? int.Parse(minutes) : 0) * 60 + (seconds.Length > 0 ? int.Parse(seconds) : 0);
      Console.WriteLine($"waiting {timeout} seconds before submitting",
        ColorTranslator.FromHtml("#bdbdbd"));
      delayFunc(timeout);
      return SubmitToServer(day, part, answer, httpClient, delayFunc);
    }

    if (response.Contains("You don't seem to be solving the right level.")) return Result.WRONG_LEVEL;
    if (response.Contains("That's the right answer")) return Result.ACCEPTED;
    if (response.Contains("your answer is too high")) return Result.REJECTED_TOO_HIGH;
    if (response.Contains("your answer is too low")) return Result.REJECTED_TOO_LOW;
    return Result.REJECTED;
  }
}