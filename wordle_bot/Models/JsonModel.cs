using Newtonsoft.Json;

namespace TicTacToeDiscordBot.Models
{
    public class JsonModel
    {
        [JsonProperty("token")]
        public string ReleaseToken { get; set; }

        [JsonProperty("tokenDebug")]
        public string DebugToken { get; set; }

        [JsonProperty("spreadsheetId")]
        public string SpreadsheetId { get; set; }

        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }
    }
}
