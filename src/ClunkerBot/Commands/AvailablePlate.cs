using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using ClunkerBot.Data;
using ClunkerBot.Models;
using ClunkerBot.Models.ReturnModels;
using ClunkerBot.Models.ReturnModels.MessageReturnModels;
using ClunkerBot.Utilities;

namespace ClunkerBot.Commands
{
    class AvailablePlate : CommandsBase
    {
        public static string Find(string plate, string country)
        {
            try {
                //country = country.ToLower().Substring(0, 2);
                country = country.ToLower(); // HACK: Needed for line #32
                plate = plate.ToUpper();

                switch(country) {
                    case "gb":
                    case "uk":
                        return FindGbAvailablePlate(plate);
                    default:
                        //return $@"🔎 <i>Find Available Plate:</i> ❓ <code>{plate}</code>
//—
//<i>Country code '{country}' is currenty unsupported.</i>";
                        return FindGbAvailablePlate($"{plate}{country.ToUpper()}"); // HACK: People keep putting in spaces! Need to figure out a better way of doing this.
                }
            } catch(Exception e) {
                return BuildErrorOutput(e);
            }
        }

        private static string FindGbAvailablePlate(string plate)
        {
            string dvlaRegistrationsBaseUrlPrefix = "https://dvlaregistrations.dvla.gov.uk/search/results.html?search=";
            string dvlaRegistrationsBaseUrlSuffix = "&action=index&pricefrom=0&priceto=&prefixmatches=&currentmatches=&limitprefix=&limitcurrent=&limitauction=&searched=true&openoption=&language=en&prefix2=Search&super=&super_pricefrom=&super_priceto=";
        
            string output = $@"🔎 <b>Find Available Plate:</b> 🇬🇧 <code>{plate}</code>
—
";
            string additionalOutput = "";

            Enums.PlatePurchaseType purchaseType = Enums.PlatePurchaseType.Unknown;
            var plateAvailable = false;

            var availablePlateFinal = "";
            var availablePlatePrice = "";
            var availablePlateLink = "";

            var url = dvlaRegistrationsBaseUrlPrefix + plate + dvlaRegistrationsBaseUrlSuffix;
            var web = new HtmlWeb();
            var document = web.Load(url);
            var documentNode = document.DocumentNode;

            var plateNodes = documentNode.SelectNodes("//div[contains(@class, 'resultsstrip')]//a[contains(@class, 'resultsstripplate')]");
            var priceNodes = documentNode.SelectNodes("//div[contains(@class, 'resultsstrip')]//p[contains(@class, 'resultsstripprice')]");

            try
            {
                for (int x = 0; x < plateNodes.Count ; x++)
                {
                    string extractedPlate = plateNodes[x]
                        .InnerText
                        .Replace("\t","")
                        .Replace("\n","")
                        .Replace("\r","")
                        .Trim();

                    string extractedPrice = "";

                    try {
                        extractedPrice = priceNodes[x]
                            .InnerText
                            .Replace("\t","")
                            .Replace("\n","")
                            .Replace("\r","")
                            .Replace("&pound;", "£")
                            .ToLower()
                            .Replace("reserve in our upcoming auction", "<i>(Auction)</i>")
                            .Replace("future auction", "<i>Future Auction</i>")
                            .Trim();
                    } catch {
                        extractedPrice = "<i>Future Auction</i>"; // HACK: Future Auctions without a price throw errors
                    }

                    string buyLink = "";

                    if (extractedPrice.Contains("<i>(Auction)</i>")) {
                        purchaseType = Enums.PlatePurchaseType.Auction;

                        buyLink = "| <a href=\"http://www.dvlaauction.co.uk/index.php/live-auction/register-log-in/\">"
                            + "How To Buy"
                            + "</a>";
                    } else if (extractedPrice.Contains("<i>Future Auction</i>")) {
                        purchaseType = Enums.PlatePurchaseType.FutureAuction;

                        buyLink = "";
                    } else if(extractedPrice.Contains("£")) {
                        purchaseType = Enums.PlatePurchaseType.BuyNow;

                        buyLink = "| <a href=\"https://dvlaregistrations.dvla.gov.uk/buy.html"
                            + "?plate=" + extractedPlate
                            + "&price=" + extractedPrice.Replace("£", "")
                            + "\">Buy Now</a>";
                    } else {
                        buyLink = "";
                    }

                    if(plateAvailable == false) {
                        if(extractedPlate.Replace(" ", "") == plate) {
                            plateAvailable = true;

                            availablePlateFinal = extractedPlate;
                            availablePlatePrice = extractedPrice;
                            availablePlateLink = buyLink.Replace("| ", "");
                        }
                    }

                    string paddedExtractedPlate = extractedPlate.PadLeft(8, ' ');

                    additionalOutput += $@"• 🇬🇧 <code>{paddedExtractedPlate}</code> | {extractedPrice} {buyLink}
";   
                }

                if(plateAvailable) {
                    if(purchaseType == Enums.PlatePurchaseType.BuyNow) {
                        output += $@"<b>{availablePlateFinal}</b> is available. {availablePlateLink} for {availablePlatePrice}.
";
                    } else {
                        output += $@"<b>{availablePlateFinal}</b> is available. See below for details.
";
                    }
                } else {
                    output += $@"<b>{plate}</b> is unavailable. Other matches can be found below.
";
                }

                output += $@"
<b>Available Variations</b>
{additionalOutput}";
            }
            catch(Exception e) // TODO: Properly handle no results
            {
                ConsoleOutputUtilities.ErrorConsoleMessage(e.ToString());
                output += $@"<b>{plate}</b> is unavailable.";
            }

            return output;
        }
    }
}