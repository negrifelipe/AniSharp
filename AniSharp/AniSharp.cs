using AniSharp.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AniSharp.Constants;
using System.Threading.Tasks;
using System.Globalization;

namespace AniSharp
{
    public static class AniSharp
    {
        public const string BasePath = "https://myanimelist.net/";

        public static HtmlWeb Web = new HtmlWeb();

        #region async

        /// <summary>
        /// Gets the anime data from the given name
        /// </summary>
        /// <param name="name">The name of the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static async Task<Anime> GetAnimeFromNameAsync(string name)
        {
            // We need to make sure this request is not from the cache

            bool usedCache = false;

            if (Web.UsingCache)
            {
                usedCache = true;
                Web.UsingCache = false;
            }

            var document = await Web.LoadFromWebAsync($"{BasePath}anime.php?cat=anime&q={string.Join("+", name.Split(' '))}");

            Web.UsingCache = usedCache;

            var content = document.GetElementbyId("content");

            var url = content.SelectNodes("//div//table")[2].SelectSingleNode("//tr//td//a//strong").ParentNode.GetAttributeValue("href", null);

            return await GetAnimeDataAsync(url);
        }

        /// <summary>
        /// Gets the anime data from the given url
        /// </summary>
        /// <param name="url">The url from the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static async Task<Anime> GetAnimeDataAsync(string url)
        {
            var document = await Web.LoadFromWebAsync(url);

            return ParseAnime(document);
        }

        /// <summary>
        /// Gets the anime data from the given id
        /// </summary>
        /// <param name="id">The id of the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static Task<Anime> GetAnimeFromIdAsync(int id)
        {
            return GetAnimeDataAsync($"{BasePath}anime/{id}");
        }

        /// <summary>
        /// Gets a list of animes from the top
        /// </summary>
        /// <param name="startIndex">From where the search will start</param>
        /// <param name="type">This is optional, you can specify a type to search use <see cref="TopTypes"/> to get the types</param>
        /// <returns>A list with the animes</returns>
        public static async Task<List<AnimeCard>> GetTopAnimeAsync(int startIndex = 0, string type = null)
        {
            // We need to make sure this request is not from the cache

            bool usedCache = false;

            if (Web.UsingCache)
            {
                usedCache = true;
                Web.UsingCache = false;
            }

            var document = await Web.LoadFromWebAsync($"{BasePath}topanime.php?limit={startIndex}&type={type ?? ""}");

            Web.UsingCache = usedCache;

            return ParseTopAnimes(document);
        }

        #endregion

        #region sync

        /// <summary>
        /// Gets the anime data from the given name
        /// </summary>
        /// <param name="name">The name of the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static Anime GetAnimeFromName(string name)
        {
            // We need to make sure this request is not from the cache

            bool usedCache = false;

            if(Web.UsingCache)
            {
                usedCache = true;
                Web.UsingCache = false;
            }

            var document = Web.Load($"{BasePath}anime.php?cat=anime&q={string.Join("+", name.Split(' '))}");

            Web.UsingCache = usedCache;

            var content = document.GetElementbyId("content");

            var url = content.SelectNodes("//div//table")[2].SelectSingleNode("//tr//td//a//strong").ParentNode.GetAttributeValue("href", null);

            return GetAnimeData(url);
        }

        /// <summary>
        /// Gets the anime data from the given url
        /// </summary>
        /// <param name="url">The url from the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static Anime GetAnimeData(string url)
        {
            var document = Web.Load(url);

            return ParseAnime(document);
        }

        /// <summary>
        /// Gets the anime data from the given id
        /// </summary>
        /// <param name="id">The id of the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static Anime GetAnimeFromId(int id)
        {
            return GetAnimeData($"{BasePath}anime/{id}");
        }

        /// <summary>
        /// Gets a list of animes from the top
        /// </summary>
        /// <param name="startIndex">From where the search will start</param>
        /// <param name="type">This is optional, you can specify a type to search use <see cref="TopTypes"/> to get the types</param>
        /// <returns>A list with the animes</returns>
        public static List<AnimeCard> GetTopAnime(int startIndex = 0, string type = null)
        {
            // We need to make sure this request is not from the cache

            bool usedCache = false;

            if (Web.UsingCache)
            {
                usedCache = true;
                Web.UsingCache = false;
            }

            var document = Web.Load($"{BasePath}topanime.php?limit={startIndex}&type={type ?? ""}");

            Web.UsingCache = usedCache;

            return ParseTopAnimes(document);
        }

        #endregion

        #region cache

        /// <summary>
        /// Disables the cache mod
        /// </summary>
        public static void DisableCache()
        {
            Web.UsingCache = false;
        }

        /// <summary>
        /// Enables the cache mode
        /// </summary>
        /// <param name="cachePath">The path were the cache files will be located; Leave it as null to use the default one</param>
        public static void EnableCache(string cachePath = null)
        {
            if (cachePath == null)
                Web.CachePath = Path.Combine(System.Environment.CurrentDirectory, "Cache");
            else
                Web.CachePath = cachePath;

            Web.UsingCache = true;
            Web.UsingCacheIfExists = true;
        }

        #endregion

        #region internal

        internal static Anime ParseAnime(HtmlDocument document)
        {
            var name = document.GetElementbyId("contentWrapper").SelectSingleNode("//div//div//div//div//h1").InnerText;

            if (name == null) 
                return null;

            var content = document.GetElementbyId("content");

            var id = document.GetElementbyId("myinfo_anime_id").GetAttributeValue("value", 0);
            var url = document.DocumentNode.SelectSingleNode(document.GetElementbyId("horiznav_nav").XPath + "//ul//a").GetAttributeValue("href", string.Empty);
            var synopsis = content.SelectSingleNode("//table//tr//div//table//tr//td//p").InnerText;
            var sideBar = content.SelectSingleNode("//table//tr//td//div//h2").ParentNode;
            var image = sideBar.SelectNodes("//div//a//img")[1].GetAttributeValue("data-src", string.Empty);

            var charactersUrl = content.SelectNodes("//table//tr//div//table//tr//td//div//div//a").FirstOrDefault(x => x.InnerText == "More characters").GetAttributeValue("href", string.Empty);

            return new Anime()
            {
                Id = id,
                Name = name,
                Image = image,
                Url = url,
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
                    Score = sideBar.GetSidebarData("Rating")
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

        internal static List<AnimeCard> ParseTopAnimes(HtmlDocument document)
        {
            var content = document.GetElementbyId("content");

            var rankingTables = document.DocumentNode.SelectNodes(content.XPath + "//div//table//tr").Where(x => x.HasClass("ranking-list"));

            List<AnimeCard> cards = new List<AnimeCard>();

            foreach(var table in rankingTables)
            {
                var name = document.DocumentNode.SelectSingleNode(table.XPath + "//td//div//div//h3//a").InnerText;
                var score = document.DocumentNode.SelectSingleNode(table.XPath + "//td//div//span").InnerText;
                var image = document.DocumentNode.SelectSingleNode(table.XPath + "//td//a//img").GetAttributeValue("data-src", string.Empty);
                var url = document.DocumentNode.SelectSingleNode(table.XPath + "//td//a").GetAttributeValue("href", string.Empty);

                cards.Add(new AnimeCard
                {
                    Name = name,
                    Score = double.Parse(score, CultureInfo.InvariantCulture),
                    Image = image,
                    Url = url
                });
            }

            return cards;
        }

        #endregion
    }
}