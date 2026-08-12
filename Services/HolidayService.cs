using System.Globalization;
using System.Xml.Linq;

namespace calendar4;

public sealed class HolidayService
{
    private const string ApiKey = "J2oeQhi+RfMtZirCpU8UM/Nnuqsa6u9nZiDHHtdDRVTVt1vQ2BCgbv8BHATrM7uLV0/Q7lndndr/0/Omo8JGpw==";

    public async Task<Dictionary<DateTime, string>> GetHolidaysAsync(int year, int month)
    {
        var encodedKey = Uri.EscapeDataString(ApiKey);
        var url =
            "https://apis.data.go.kr/B090041/openapi/service/SpcdeInfoService/getRestDeInfo" +
            "?serviceKey=" + encodedKey +
            "&solYear=" + year +
            "&solMonth=" + month.ToString("D2") +
            "&pageNo=1&numOfRows=100&_type=xml";

        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        var xml = await client.GetStringAsync(url);
        var document = XDocument.Parse(xml);
        var result = new Dictionary<DateTime, string>();

        foreach (var item in document.Descendants("item"))
        {
            var dateText = item.Element("locdate")?.Value;
            var name = item.Element("dateName")?.Value ?? string.Empty;
            if (DateTime.TryParseExact(
                dateText,
                "yyyyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
            {
                result[date.Date] = name;
            }
        }

        return result;
    }
}
