using System;
using HtmlAgilityPack;

namespace PartyGoer.Infotainment;

public static class CalendarEventService
{
    public static async Task<string> GetTodaysEventsAsync()
    {
        string eventsList = string.Empty;
        using (var client = new HttpClient())
        {
            var content = await client.GetStringAsync("http://calendar.by");
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(content);

            var programmerLinks = htmlDocument.DocumentNode.Descendants("td")
                .Where(node => node.InnerText.Contains("События и праздники")).ToList();

            eventsList = programmerLinks.First().Ancestors().First().InnerText;
        }
        eventsList = eventsList.Substring(eventsList.IndexOf('С'));
        eventsList = $"{DateTime.UtcNow.ToLongDateString()}\n{eventsList}";
        return eventsList;
    }
}
