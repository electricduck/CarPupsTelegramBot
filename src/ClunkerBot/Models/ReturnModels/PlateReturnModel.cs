// TODO: Rename this 'GbPlateReturnModel' in the 'PlateReturnModels' namespace

using ClunkerBot.Models;

namespace ClunkerBot.Models.ReturnModels {
    public class PlateReturnModel {
        public string Location { get; set; }

        public int Year { get; set; }
        public string Month { get; set; }
        public string Issue { get; set; }
        public Enums.GbPlateSpecial Type { get; set; }
        public Enums.GbPlateFormat Format { get; set; }
        public string DiplomaticOrganisation { get; set; }
        public string DiplomaticType { get; set; }
        public string DiplomaticRank { get; set; }
        public bool Valid { get; set; }
    }
}