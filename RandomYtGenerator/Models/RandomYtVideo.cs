using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RandomYTGenerátor.Models
{
    public class SearchVideos
    {
        public string videoId;
        public string authorId;
        public long viewCount;
        public long published;
    }

    public class VideoStats
    {
        public int subCount;
    }

    public class RandomYtVideo
    {
        public Dictionary<string, long> _videos = new Dictionary<string, long>();
        public Dictionary<string, string> _author = new Dictionary<string, string>();
        public List<long> kentusMetrSorter = new List<long>();

        private static string GetLetters()
        {
            string chars = "abcdefghijklmnopqrstuvwxyz";
            Random rand = new Random();
            int num1 = rand.Next(0, chars.Length);
            int num2 = rand.Next(0, chars.Length);
            return chars[num1] + "" + chars[num2];
        }

        private async Task<List<SearchVideos>> GetJson()
        {
            HttpClient client = new HttpClient();
            string response = await client.GetStringAsync("https://invidio.xamh.de/api/v1/search/?fields=videoId,viewCount,published,authorId&q=" + GetLetters() + "&page=10&sort_by=view_count&date=today&duration=short&type=video&pretty=1");
            while (response.Equals("[]"))
            {
                response = await client.GetStringAsync("https://invidio.xamh.de/api/v1/search/?fields=videoId,viewCount,published,authorId&q=" + GetLetters() + "&page=10&sort_by=view_count&date=today&duration=short&type=video&pretty=1");
                continue;
            }
            return JsonConvert.DeserializeObject<List<SearchVideos>>(response);
        }

        private async Task<VideoStats> GetSubsJson(string authorId)
        {
            HttpClient client = new HttpClient();
            string response = await client.GetStringAsync("https://invidio.xamh.de/api/v1/channels/" + authorId + "?fields=subCount&pretty=1");
            return JsonConvert.DeserializeObject<VideoStats>(response);
        }

        public async Task<string[]> GetVideoAsync()
        {
            try
            {
                var json = GetJson().Result;
                foreach (var item in json)
                {
                    _videos.Add(item.videoId, item.published / (item.viewCount + 1));
                    _author.Add(item.videoId, item.authorId);
                    kentusMetrSorter.Add(item.published / (item.viewCount + 1));
                }

                kentusMetrSorter.Sort();
                var key = _videos.FirstOrDefault(x => x.Value == kentusMetrSorter.Last()).Key;

                foreach (var item in json)
                {
                    if (item.videoId == key)
                    {
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        dateTime = dateTime.AddSeconds(item.published);

                        if (_author.GetValueOrDefault(key) == null)
                            throw new Exception();

                        var listStats = await GetSubsJson(_author.GetValueOrDefault(key));
                        return new string[] { "https://www.youtube-nocookie.com/embed/" + item.videoId + "?fs=0&modestbranding=1&rel=0", listStats.subCount.ToString(), item.viewCount.ToString(), dateTime.ToString("d") };
                    }
                }

                return new string[] { "https://www.youtube-nocookie.com/embed/dQw4w9WgXcQ", "N/A", "N/A", "N/A" };
            }
            catch (HttpRequestException)
            {
                return new string[] { "https://www.youtube-nocookie.com/embed/dQw4w9WgXcQ", "N/A", "N/A", "N/A" };
            }
        }
    }
}