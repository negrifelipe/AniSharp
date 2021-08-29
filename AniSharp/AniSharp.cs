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

        /// <summary>
        /// Gets the anime data from the given name
        /// </summary>
        /// <param name="name">The name of the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static async Task<Anime> GetAnimeFromNameAsync(string name)
        {
            var document = await Web.LoadFromWebAsync($"{BasePath}anime.php?cat=anime&q={string.Join("+", name.Split(' '))}");

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
        public static async Task<Anime> GetAnimeFromIdASync(string id)
        {
            var document = await Web.LoadFromWebAsync($"{BasePath}anime/{id}");

            return ParseAnime(document);
        }

        /// <summary>
        /// Gets a list of animes from the top
        /// </summary>
        /// <param name="startIndex">From where the search will start</param>
        /// <param name="type">This is optional, you can specify a type to search use <see cref="TopTypes"/> to get the types</param>
        /// <returns>A list with the animes</returns>
        public static async Task<List<AnimeCard>> GetTopAnimeAsync(int startIndex = 0, string type = null)
        {
            var document = await Web.LoadFromWebAsync($"{BasePath}topanime.php?limit={startIndex}&type={type ?? ""}");

            return ParseTopAnime(document);
        }

        /// <summary>
        /// Gets the anime data from the given name
        /// </summary>
        /// <param name="name">The name of the anime</param>
        /// <returns>The searched anime; Returns null if not found</returns>
        public static Anime GetAnimeFromName(string name)
        {
            var document = Web.Load($"{BasePath}anime.php?cat=anime&q={string.Join("+", name.Split(' '))}");

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
        public static Anime GetAnimeFromId(string id)
        {
            var document = Web.Load($"{BasePath}anime/{id}");

            return ParseAnime(document);
        }

        /// <summary>
        /// Gets a list of animes from the top
        /// </summary>
        /// <param name="startIndex">From where the search will start</param>
        /// <param name="type">This is optional, you can specify a type to search use <see cref="TopTypes"/> to get the types</param>
        /// <returns>A list with the animes</returns>
        public static List<AnimeCard> GetTopAnime(int startIndex = 0, string type = null)
        {
            var document = Web.Load($"{BasePath}topanime.php?limit={startIndex}&type={type ?? ""}");
            System.Console.WriteLine($"{BasePath}topanime.php?limit={startIndex}&type={type ?? ""}");
            return ParseTopAnime(document);
        }

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

            var charactersUrl = content.SelectNodes("//table//tr//div//table//tr//td//div//div//a").FirstOrDefault(x => x.InnerText == "More characters").GetAttributeValue("href", string.Empty);

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
                    Score = sideBar.GetSidebarData("Rating")
                },
                Statics = new AnimeStatics()
                {
                    Score = sideBar.GetSidebarData("Score"),
                    Rank = sideBar.GetSidebarData("Ranked"),
                    Popularity = sideBar.GetSidebarData("Popularity"),
                    Favorites = sideBar.GetSidebarData("Favorites")
                },
                Characters = GetCaracters($"{BasePath}{charactersUrl}")
            };
        }

        internal static string GetSidebarData(this HtmlNode node, string data)
        {
            var find =  node.SelectNodes("//div//span").FirstOrDefault(x => x.InnerText == data + ":").ParentNode.InnerText.Split(' ').ToList();
            find.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            find.RemoveAt(0);
            return string.Join(" ", find).Trim();
        }

        internal static List<AnimeCharacter> GetCaracters(string url)
        {
            var document = Web.Load(url);

            var nav = document.GetElementbyId("horiznav_nav");

            var staff = document.DocumentNode.SelectNodes(nav.ParentNode.XPath + "//a").FirstOrDefault(x => x.GetAttributeValue("name", string.Empty) == "staff");

            var tables = document.DocumentNode.SelectNodes(nav.ParentNode.XPath + "//table//tr//td//div//small");

            List<AnimeCharacter> characters = new List<AnimeCharacter>();

            foreach(var table in tables.Select(x => x.ParentNode.ParentNode).Where(x => staff.ParentNode.ChildNodes.IndexOf(x.ParentNode.ParentNode) < staff.ParentNode.ChildNodes.IndexOf(staff)))
            {
                var nameNode = document.DocumentNode.SelectSingleNode(table.XPath + "//a");
                characters.Add(new AnimeCharacter()
                {
                    Name = nameNode.InnerText.Trim(),
                    Page = nameNode.GetAttributeValue("href", string.Empty),
                    Image = document.DocumentNode.SelectSingleNode(table.ParentNode.XPath + "//td//div//a//img").GetAttributeValue("data-src", string.Empty),
                    Type = document.DocumentNode.SelectSingleNode(table.XPath + "//div//small").InnerText
                });
            }

            return characters;
        }

        internal static List<AnimeCard> ParseTopAnime(HtmlDocument document)
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
