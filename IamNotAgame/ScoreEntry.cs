using Newtonsoft.Json;

namespace IamNotAgame
{
    public class ScoreEntry
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
    }

    public class LeaderboardManager
    {
        public List<ScoreEntry> Entries { get; set; } = new List<ScoreEntry>();

        public void LoadLeaderboard(string filePath)
        {
            var jsonData = File.ReadAllText(filePath);
            Entries = JsonConvert.DeserializeObject<List<ScoreEntry>>(jsonData) ?? new List<ScoreEntry>();
        }

        public void UpdateLeaderboard(ScoreEntry newEntry, string filePath)
        {
            Entries.Add(newEntry);
            Entries = Entries.OrderByDescending(x => x.Score).Take(10).ToList();
            File.WriteAllText(filePath, JsonConvert.SerializeObject(Entries));
        }
    }
}

