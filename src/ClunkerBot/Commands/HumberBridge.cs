using System;
using HtmlAgilityPack;
using ClunkerBot.Models;
using ClunkerBot.Utilities;

namespace ClunkerBot.Commands
{
    class HumberBridge : CommandsBase
    {
        public static string GetConditionsOn()
        {
            string outputEmoji = "🌉";
            string outputHeader = "Humber Bridge Status";

            try
            {
                string humberBridgeBaseUrl = "https://www.humberbridge.co.uk/";

                var url = humberBridgeBaseUrl;
                var web = new HtmlWeb();
                var document = web.Load(url);
                var documentNode = document.DocumentNode;

                var bridgeStatus = documentNode.SelectNodes("//p[contains(@class, 'bridgestatus')]")[0].InnerText;
                var restrictions = documentNode.SelectNodes("//p[contains(@class, 'restrictions')]")[0].InnerText;
                var weatherDays = documentNode.SelectNodes("//div[contains(@id, 'weather')]//div//h1");
                var weatherIcons = documentNode.SelectNodes("//div[contains(@id, 'weather')]//div//img");
                var windDirection = documentNode.SelectNodes("//p[contains(@class, 'winddirection')]")[0].InnerText;
                var windSpeed = documentNode.SelectNodes("//div[contains(@class, 'windspeed')]//h1")[0].InnerText.Replace("\n", "").Replace("\t", "");
                var windSpeedUnit = documentNode.SelectNodes("//div[contains(@class, 'windspeed')]//h1")[1].InnerText;

                var day0Weather = GetWeather(0, weatherIcons, weatherDays);
                var day1Weather = GetWeather(1, weatherIcons, weatherDays);
                var day2Weather = GetWeather(2, weatherIcons, weatherDays);
                var day3Weather = GetWeather(3, weatherIcons, weatherDays);

                var bridgeStatusIcon = "✔️";

                if(bridgeStatus != "Open")
                {
                    bridgeStatusIcon = "❌";
                }

                string result = $@"<b>{bridgeStatusIcon} {bridgeStatus}</b>
{restrictions}

<b>☁️ Conditions</b>
<i>Wind Speed:</i> {windSpeed}{windSpeedUnit}
<i>Wind Direction:</i>{windDirection}

<b>☀️ Weather</b>
{day0Weather}
{day1Weather}
{day2Weather}
{day3Weather}

<b>💵 Prices</b>
<i>Class 1:</i> Free
<i>Class 2 (3.5t max):</i> £1.50 (£1.35 TAG)
<i>Class 3 (3.5t-7.5t):</i> £4.00 (£3.60 TAG)
<i>Class 4 (7.5t over):</i> £12.00 (£10.80 TAG)";

                return BuildOutput(result, outputHeader, outputEmoji);
            }
            catch(Exception e)
            {
                return BuildErrorOutput(e);
            }
        }

        private static string GetWeather(int day, HtmlNodeCollection iconsNodes, HtmlNodeCollection daysNodes)
        {
            var dayNode = NormalizeDay(daysNodes[day].InnerText);
            var iconNode = iconsNodes[day].Attributes["src"].Value;

            var iconCode = GetWeatherIconFromImgSrc(iconNode);

            var dayText = dayNode;
            var weatherIcon = WeatherUtilities.GetWeatherIcon(iconCode);
            var weatherText = WeatherUtilities.GetWeatherText(iconCode);

            return $@"<i>{dayText}:</i> {weatherIcon} {weatherText}";
        }

        private static string GetWeatherIconFromImgSrc(string imgSrcValue)
        {
            return imgSrcValue
                .Replace("https://www.humberbridge.co.uk/wp-content/plugins/BridgeConditions/images/icons/", "")
                .Replace(".png", "");
        }

        private static string NormalizeDay(string shortDay)
        {
            switch(shortDay.ToLower())
            {
                case "mon":
                    return "Mon";
                case "tue":
                    return "Tue";
                case "wed":
                    return "Wed";
                case "thu":
                    return "Thu";
                case "fri":
                    return "Fri";
                case "sat":
                    return "Sat";
                case "sun":
                    return "Sun";
            }

            return "Now";
        }
    }
}