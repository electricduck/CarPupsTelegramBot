using System;
using ClunkerBot.Utilities;

namespace ClunkerBot.Commands
{
    class CarbonNeutral : CommandsBase
    {
        public static string Calculate(string emissions, string mileage)
        {
            string outputEmoji = "🌳";
            string outputHeader = "Carbon Neutral";

            try {
                var normalizedEmissions = UnitConversionUtilities.NormalizeEmissions(emissions);
                var normalizedMileage = UnitConversionUtilities.NormalizeDistance(mileage);
            
                if(
                    normalizedEmissions.Calculated == false ||
                    normalizedMileage.Calculated == false
                ) {
                    return BuildSoftErrorOutput("Unknown unit. See <code>/help co2</code>.");
                }

                double treeCo2AbsorbtionPerYearInGrams = 21770;

                var treesPerYear = Math.Round((normalizedEmissions.Value * normalizedMileage.Value) / treeCo2AbsorbtionPerYearInGrams);
                var treesPerMonth = Math.Round(treesPerYear / 12);

                var donationAmount_NationalForest = GenerateDonationAmountString(treesPerYear);
                var donationAmount_PlantATree = GenerateDonationAmountString(treesPerYear, 5);
                var donationAmount_TeamTrees = GenerateDonationAmountString(treesPerYear);
                var donationAmount_WoodlandTrust = GenerateDonationAmountString(treesPerYear, 1.5, "£");

                string result = $@"<b>{treesPerYear} trees per year</b> <i>(approx.)</i> will need to be planted to offest your vehicle's emissions.

<b>Donation</b>
• <code>{donationAmount_NationalForest}</code> | <a href='https://app.etapestry.com/hosted/NationalForestFoundation/PlantTrees.html'>🇺🇸 National Forest</a>
• <code>{donationAmount_PlantATree}</code> | <a href='https://plantatreefoundation.org/plant-a-tree-campaign/'>🇺🇸 Plant A Tree</a>
• <code>{donationAmount_TeamTrees}</code> | <a href='https://teamtrees.org/'>🇺🇸 TeamTrees</a>
• <code>{donationAmount_WoodlandTrust}</code> | <a href='https://www.woodlandtrust.org.uk/protecting-trees-and-woods/campaign-with-us/big-climate-fightback/'>🇬🇧 Woodland Trust</a>
<hr>
<i>This is how many trees you'd need to plant for your driving to be carbon neutral, assuming the average tree absorbs 21.77kg of CO2 per year.</i>";

                return BuildOutput(result, outputHeader, outputEmoji);
            } catch(Exception e) {
                return BuildErrorOutput(e);
            }
        }

        private static string GenerateDonationAmountString(double trees, double modifier = 1, string currency = "$")
        {
            return (currency + String.Format("{0:0.00}", trees*modifier)).PadLeft(8, ' ');
        }
    }
}