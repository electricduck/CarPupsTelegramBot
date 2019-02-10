using ClunkerBot.Plates.Models;

namespace ClunkerBot.Plates.Models.ReturnModels {
    public class DePlateReturnModel : PlateReturnModel {
        public Enums.DePlateFormatEnum Format { get; set; }
        public string Location { get; set; }
        public string Special { get; set; }
    }
}