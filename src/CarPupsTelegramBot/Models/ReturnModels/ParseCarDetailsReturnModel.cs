using CarPupsTelegramBot.Models;

namespace CarPupsTelegramBot.Models.ReturnModels {
    public class ParseCarDetailsReturnModel {
        public string ParsedEngine { get; set; }
        public string ParsedEngineSize { get; set; }
        public string ParsedGeneration { get; set; }
        public string ParsedPlate { get; set; }
    }
}