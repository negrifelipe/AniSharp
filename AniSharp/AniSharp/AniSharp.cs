using AniSharp.Models;
using HtmlAgilityPack;
using System.Linq;
using System.Threading.Tasks;

namespace AniSharp
{
    public static class AniSharp
    {
        public const string BasePath = "https://myanimelist.net/";

        /// <summary>
        /// Gets the anime data from the given name
        /// </summary>
        /// <param name="name">The name of the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static async Task<Anime> GetAnimeFromNameAsync(string name)
        {
            var document = await new HtmlWeb().LoadFromWebAsync($"{BasePath}anime.php?cat=anime&q={string.Join("+", name.Split(' '))}");

            var content = document.GetElementbyId("content");
            var url = content.SelectNodes("//div//table")[2].SelectSingleNode("//tr//td//a//strong").ParentNode.GetAttributeValue("href", null);

            if (string.IsNullOrEmpty(url))
                return null;

            return await GetAnimeDataAsync(url);
        }

        /// <summary>
        /// Gets the anime data from the given url
        /// </summary>
        /// <param name="url">The url from the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static async Task<Anime> GetAnimeDataAsync(string url)
        {
            var document = await new HtmlWeb().LoadFromWebAsync(url);

            return ParseAnime(document);
        }

        public static Anime GetAnimeFromName(string name)
        {
            var document = new HtmlWeb().Load($"{BasePath}anime.php?cat=anime&q={string.Join("+", name.Split(' '))}");

            var content = document.GetElementbyId("content");
            var url = content.SelectNodes("//div//table")[2].SelectSingleNode("//tr//td//a//strong").ParentNode.GetAttributeValue("href", null);

            if (string.IsNullOrEmpty(url))
                return null;

            return GetAnimeData(url);
        }

        /// <summary>
        /// Gets the anime data from the given url
        /// </summary>
        /// <param name="url">The url from the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static Anime GetAnimeData(string url)
        {
            var document = new HtmlWeb().Load(url);

            return ParseAnime(document);
        }

        /// <summary>
        /// Gets the anime data from the given id
        /// </summary>
        /// <param name="id">The id of the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static Anime GetAnimeFromId(string id)
        {
            var document = new HtmlWeb().Load($"{BasePath}anime/{id}");

            return ParseAnime(document);
        }

        #region internal

        internal static Anime ParseAnime(HtmlDocument document)
        {
            var name = document.GetElementbyId("contentWrapper").SelectSingleNode("//div//div//div//div//h1").InnerText;

            if (name == null) 
                return null;

            var content = document.GetElementbyId("content");

            var synopsis = content.SelectSingleNode("//table//tr//div//table//tr//td//p").InnerText;
            var sideBar = content.SelectSingleNode("//table//tr//td//div//h2").ParentNode;
            var image = sideBar.SelectNodes("//div//a//img")[1].GetAttributeValue("data-src", string.Empty);

            return new Anime()
            {
                Name = name,
                Image = image,
                Synopsis = synopsis,
                Information = new AnimeInformation()
                {
                    Episodes = int.Parse(sideBar.GetSidebarData("Episodes")),
                    Status = sideBar.GetSidebarData("Status"),
                    Aired = sideBar.GetSidebarData("Aired"),
                    Season = sideBar.GetSidebarData("Premiered"),
                    Broadcast = sideBar.GetSidebarData("Broadcast"),
                    Producers = sideBar.GetSidebarData("Producers").Split(',').Select(x => x.Trim()).ToArray(),
                    Licensors = sideBar.GetSidebarData("Licensors").Split(',').Select(x => x.Trim()).ToArray(),
                    Studios = sideBar.GetSidebarData("Studios").Split(',').Select(x => x.Trim()).ToArray(),
                    Source = sideBar.GetSidebarData("Source"),
                    Genres = sideBar.GetSidebarData("Genres").Split(',').Select(x => x.Trim()).ToArray(),
                    Duration = sideBar.GetSidebarData("Duration"),
                    Rating = sideBar.GetSidebarData("Rating")
                },
                Statics = new AnimeStatics()
                {
                    Score = sideBar.GetSidebarData("Score"),
                    Rank = sideBar.GetSidebarData("Ranked"),
                    Popularity = sideBar.GetSidebarData("Popularity"),
                    Favorites = sideBar.GetSidebarData("Favorites")
                }
            };
        }

        internal static string GetSidebarData(this HtmlNode node, string data)
        {
            var find =  node.SelectNodes("//div//span").FirstOrDefault(x => x.InnerText == data + ":").ParentNode.InnerText.Split(' ').ToList();
            find.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            find.RemoveAt(0);
            return string.Join(" ", find).Trim();
        }

        #endregion
    }
}
