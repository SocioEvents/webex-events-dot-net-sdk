using System.Net.Http.Headers;

namespace WebexEvents;

public class RateLimiter
{
    private const string DailyCallLimit = "x-daily-call-limit";
    private const string SecondlyCallLimit = "x-secondly-call-limit";
    private const string DailyRetryAfter = "x-daily-retry-after";
    private const string SecondlyRetryAfter = "x-secondly-retry-after";
    
    private readonly Dictionary<string, string> _headers;

    private readonly int _usedSecondBasedCost;
    private readonly int _secondBasedCostThreshold;
    private readonly int _usedDailyBasedCost;
    private readonly int _dailyBasedCostThreshold;
    private readonly int _dailyRetryAfterInSecond;
    private readonly int _secondlyRetryAfterInMs;

    public RateLimiter(Response response)
    {
        _headers = response.ResponseHeaders;

        var dailyCallLimit = GetHeaderValue(DailyCallLimit);
        if (dailyCallLimit.Length > 0)
        {
            string[] parts = dailyCallLimit.Split("/");
            _usedDailyBasedCost = Int32.Parse(parts[0]);
            _dailyBasedCostThreshold = Int32.Parse(parts[1]);
        }
        
        var secondlyCallLimit = GetHeaderValue(SecondlyCallLimit);
        if (secondlyCallLimit.Length > 0)
        {
            string[] parts = secondlyCallLimit.Split("/");
            _usedSecondBasedCost = Int32.Parse(parts[0]);
            _secondBasedCostThreshold = Int32.Parse(parts[1]);
        }

        var dailyRetryAfter = GetHeaderValue(DailyRetryAfter);
        if (dailyRetryAfter.Length > 0)
        {
            _dailyRetryAfterInSecond = int.Parse(dailyRetryAfter);
        }
        
        var secondlyRetryAfter = GetHeaderValue(SecondlyRetryAfter);
        if (secondlyRetryAfter.Length > 0)
        {
            _secondlyRetryAfterInMs = int.Parse(secondlyRetryAfter);
        }
    }

    private string GetHeaderValue(string header)
    {
        if (_headers.ContainsKey(header))
        {
            return _headers[header];
        }
        else
        {
            return "";
        }
    }
    
    public int UsedSecondBasedCost()
    {
        return _usedSecondBasedCost;
    }

    public int SecondBasedCostThreshold()
    {
        return _secondBasedCostThreshold;
    }

    public int UsedDailyBasedCost()
    {
        return _usedDailyBasedCost;
    }

    public int DailyBasedCostThreshold()
    {
        return _dailyBasedCostThreshold;
    }

    public int DailyRetryAfterInSecond()
    {
        return _dailyRetryAfterInSecond;
    }

    public int SecondlyRetryAfterInMs()
    {
        return _secondlyRetryAfterInMs;
    }
}