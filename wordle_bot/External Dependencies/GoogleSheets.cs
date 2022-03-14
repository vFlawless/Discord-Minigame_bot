using System.Collections.Generic;
using System.IO;
using System.Threading;
using DSharpPlus.Entities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using wordle_bot;

namespace TicTacToeDiscordBot.External_Dependencies
{
    public class GoogleSheets
    {
        public string[] Scopes = { SheetsService.Scope.Spreadsheets };
        public string ApplicationName = "Discord Bot Server List";
        public string spreadsheetId = Bot.ReadFromJson("1cAAmY9rGaCPHJ21OZYyK0t7Wt3WHs96Sr-JHm6aFS4o"); // Insert your spreadsheetId here.
        public SheetsService SheetsService;

        public void InitSheetsApi()
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            SheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public void UpdateData(List<DiscordGuild> servers)
        {
            ClearSpreadsheet();
            // Define request parameters.
            string range = "Server List!A1";

            ValueRange valueRange = new ValueRange();
            valueRange.MajorDimension = "COLUMNS";

            List<object> oblist = new List<object>();
            

            foreach (var server in servers)
            {
                oblist.Add(server.Name);
            }

            valueRange.Values = new List<IList<object>> { oblist }; valueRange.Values = new List<IList<object>> { oblist };
            SpreadsheetsResource.ValuesResource.UpdateRequest update = SheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            update.Execute();
        }

        private void ClearSpreadsheet()
        {
            string range = "Server List!A1:A255";
            ClearValuesRequest requestBody = new ClearValuesRequest();
            SpreadsheetsResource.ValuesResource.ClearRequest request = SheetsService.Spreadsheets.Values.Clear(requestBody, spreadsheetId, range);
            ClearValuesResponse response = request.Execute();
        }
    }
}
