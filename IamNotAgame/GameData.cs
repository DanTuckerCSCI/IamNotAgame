using System.Collections.Generic;
namespace IamNotAgame
{
    public class GameData
    {
        public string Description { get; set; }
        public string Instructions { get; set; }
        public string JavaScriptCode { get; set; }
        // For simplicity, we'll use the path to the thumbnail image.
        // Depending on requirements, this could be changed to byte[] if the image data itself needs to be included.
        public string ThumbnailImagePath { get; set; }
        public List<ScoreEntry> Leaderboard { get; set; }

        public GameData()
        {
            Leaderboard = new List<ScoreEntry>();
        }
    }
}
