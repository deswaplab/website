namespace DeswapApp;

using System.Net.Http.Json;
using System.Text.Json.Serialization;

public class PriceService(HttpClient httpClient, ILogger<PriceService> logger)
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly ILogger<PriceService> _logger = logger;

    // 调用okx接口获取现货行情价格
    // https://www.okx.com/docs-v5/zh/#order-book-trading-market-data-get-ticker
    public async Task<decimal> GetCurrentSpotPriceFromOkx(string symbol)
    {
        string url = $"https://aws.okx.com/api/v5/market/ticker?instId={symbol}";
        var resp = await _httpClient.GetFromJsonAsync<OkxTickResponse>(url);
        if (resp is not null && resp.Data.Count > 0)
        {
            return resp.Data[0].Last;
        }
        return 0;
    }

    // https://www.okx.com/docs-v5/zh/#public-data-rest-api-get-option-market-data
    // 推荐一个期权行权价格
    public async Task<decimal> GetRecommendOptionsPrice(OptionsKind kind, DateOnly maturityDate, string cexSymbol, string? uly)
    {
        // 传入一个交易所的symbol，获取一个推荐的价格
        var curPrice = await GetCurrentSpotPriceFromOkx(cexSymbol);
        _logger.LogInformation($"tick price: {curPrice}, symbol={cexSymbol}, uly={uly}");
        string expTime = maturityDate.ToString("yyMMdd");
        if (uly is not null)
        {
            string url = $"https://aws.okx.com/api/v5/public/opt-summary?uly={uly}&expTime={expTime}";
            var resp = await _httpClient.GetFromJsonAsync<OkxOptResponse>(url);
            if (resp is not null && resp.Data.Count > 0)
            {
                if (kind == OptionsKind.CALL)
                {
                    // 看涨，就选择一个比当前价格高，且成交最高的
                    var price = resp.Data.Where(p => p.FwdPx > curPrice).OrderByDescending(p => p.BidVol).FirstOrDefault();
                    if (price is not null)
                    {
                        return price.FwdPx;
                    }
                    else
                    {
                        return SimplePrice(curPrice, kind);
                    }
                }
                else
                {
                    var price = resp.Data.Where(p => p.FwdPx < curPrice).OrderByDescending(p => p.BidVol).FirstOrDefault();
                    if (price is not null)
                    {
                        return price.FwdPx;
                    }
                    else
                    {
                        return SimplePrice(curPrice, kind);
                    }
                }
            }
        }

        return SimplePrice(curPrice, kind);
    }

    private static decimal SimplePrice(decimal price, OptionsKind kind)
    {
        if (kind == OptionsKind.CALL)
        {
            return price * 1.05m;
        }
        return price * 0.95m;
    }
}

class OkxTickData
{
    [JsonPropertyName("last")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Last { get; set; }
}

class OkxTickResponse
{
    [JsonPropertyName("data")]
    public IList<OkxTickData> Data { get; set; } = [];
}

// {
//     "askVol": "0.73243041015625",
//     "bidVol": "0.5468839062499999",
//     "delta": "0.0070954263556313275",
//     "deltaBS": "0.007888131381390539",
//     "fwdPx": "2315.29452595970095",
//     "gamma": "0.050362315448394875",
//     "gammaBS": "0.000028487214770991475",
//     "instId": "ETH-USD-240426-5500-C",
//     "instType": "OPTION",
//     "lever": "1261.5032925295898",
//     "markVol": "0.6749136115877101",
//     "realVol": "0",
//     "theta": "-0.00004025362090984709",
//     "thetaBS": "-0.09506434805419375",
//     "ts": "1706336827348",
//     "uly": "ETH-USD",
//     "vega": "0.00010750447376170438",
//     "vegaBS": "0.2436094377229726",
//     "volLv": "0.4711082536955306"
// },
class OkxOptData
{
    [JsonPropertyName("fwdPx")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal FwdPx { get; set; }

    [JsonPropertyName("bidVol")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal BidVol { get; set; }
}

class OkxOptResponse
{
    [JsonPropertyName("data")]
    public IList<OkxOptData> Data { get; set; } = [];
}
