using AniSharp.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AniSharp.Constants;
using System.Threading.Tasks;
using System.Globalization;
using System;

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

            return ParseDetails(document, true) as Anime;
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
        public static async Task<List<Card>> GetTopAnimeAsync(int startIndex = 0, string type = null)
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

        /// <summary>
        /// Gets a character from a specified name
        /// </summary>
        /// <param name="name">The name of the character</param>
        /// <returns>The character</returns>
        public static async Task<Character> GetCharacterFromNameAsync(string name)
        {
            // We need to make sure this request is not from the cache

            bool usedCache = false;

            if (Web.UsingCache)
            {
                usedCache = true;
                Web.UsingCache = false;
            }

            var document = await Web.LoadFromWebAsync($"{BasePath}character.php?q={name}");

            Web.UsingCache = usedCache;
            
            return await GetCharacterFromUrlAsync(ParseCharacterUrl(document));
        }

        /// <summary>
        /// Gets a character from a specified id
        /// </summary>
        /// <param name="id">The id of the character</param>
        /// <returns>The character</returns>
        public static Task<Character> GetCharacterFromIdAsync(int id)
        {
            return GetCharacterFromUrlAsync($"{BasePath}character/{id}");
        }

        /// <summary>
        /// Gets a character from a specified url
        /// </summary>
        /// <param name="url">The url of the character</param>
        /// <returns>The character</returns>
        public static async Task<Character> GetCharacterFromUrlAsync(string url)
        {
            var document = await Web.LoadFromWebAsync(url);

            return ParseCharacter(document);
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
            return ParseDetails(document, true) as Anime;
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
        public static List<Card> GetTopAnime(int startIndex = 0, string type = null)
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

        /// <summary>
        /// Gets a character from a specified name
        /// </summary>
        /// <param name="name">The name of the character</param>
        /// <returns>The character</returns>
        public static Character GetCharacterFromName(string name)
        {
            // We need to make sure this request is not from the cache

            bool usedCache = false;

            if (Web.UsingCache)
            {
                usedCache = true;
                Web.UsingCache = false;
            }

            var document = Web.Load($"{BasePath}character.php?q={name}");

            Web.UsingCache = usedCache;

            return GetCharacterFromUrl(ParseCharacterUrl(document));
        }

        /// <summary>
        /// Gets a character from a specified id
        /// </summary>
        /// <param name="id">The id of the character</param>
        /// <returns>The character</returns>
        public static Character GetCharacterFromId(int id)
        {
            return GetCharacterFromUrl($"{BasePath}character/{id}");
        }

        /// <summary>
        /// Gets a character from a specified url
        /// </summary>
        /// <param name="url">The url of the character</param>
        /// <returns>The character</returns>
        public static Character GetCharacterFromUrl(string url)
        {
            var document = Web.Load(url);

            return ParseCharacter(document);
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

        internal static string ParseCharacterUrl(HtmlDocument document)
        {
            var content = document.GetElementbyId("content");

            return document.DocumentNode.SelectNodes(content.XPath + "//table//tr//td//a")[1].GetAttributeValue("href", string.Empty);
        }

        internal static DetailsPage ParseDetails(HtmlDocument document, bool isAnime)
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
            
            var statics = new Statics()
            {
                Score = float.Parse(sideBar.GetSidebarData("Score").Split(' ')[0], CultureInfo.InvariantCulture),
                Rank = int.Parse(sideBar.GetSidebarData("Ranked").Remove('#').Split(' ')[0].Trim().Replace("#", string.Empty)),
                Popularity = sideBar.GetSidebarData("Popularity"),
                Favorites = sideBar.GetSidebarData("Favorites")
            };


            if(isAnime)
            {
                var information = new AnimeInformation()
                {
                    EnglishName = sideBar.GetSidebarData("English"),
                    JapaneseName = sideBar.GetSidebarData("Japanese"),
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
                };

                return new Anime(id, name, url, image, synopsis, statics, information);
            }

            return null;
        }

        internal static string GetSidebarData(this HtmlNode node, string data)
        {
            var select = node.SelectNodes(node.XPath + "//div//span").FirstOrDefault(x => x.InnerText == data + ":");

            if (select == null)
                return string.Empty;

            var find = select.ParentNode.InnerText.Split(' ').ToList();
            find.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            find.RemoveAt(0);
            return string.Join(" ", find).Trim();
        }

        internal static List<Card> ParseTopAnimes(HtmlDocument document)
        {
            var content = document.GetElementbyId("content");

            var rankingTables = document.DocumentNode.SelectNodes(content.XPath + "//div//table//tr").Where(x => x.HasClass("ranking-list"));

            List<Card> cards = new List<Card>();

            foreach(var table in rankingTables)
            {
                var name = document.DocumentNode.SelectSingleNode(table.XPath + "//td//div//div//h3//a").InnerText;
                var score = document.DocumentNode.SelectSingleNode(table.XPath + "//td//div//span").InnerText;
                var image = document.DocumentNode.SelectSingleNode(table.XPath + "//td//a//img").GetAttributeValue("data-src", string.Empty);
                var url = document.DocumentNode.SelectSingleNode(table.XPath + "//td//a").GetAttributeValue("href", string.Empty);

                cards.Add(new Card
                {
                    Name = name,
                    Score = double.Parse(score, CultureInfo.InvariantCulture),
                    Image = image,
                    Url = url
                });
            }

            return cards;
        }

        internal static Character ParseCharacter(HtmlDocument document)
        {
            var contentWrapper = document.GetElementbyId("contentWrapper");
            var contentTable = document.GetElementbyId("horiznav_nav").ParentNode;

            var name = document.DocumentNode.SelectSingleNode(contentWrapper.XPath + "//div//div//h1//strong").InnerText;
            var url = document.DocumentNode.SelectNodes(contentTable.XPath + "//div//div//a//span").Select(x => x.ParentNode).ToList()[2].GetAttributeValue("href", string.Empty);
            var description = string.Join(Environment.NewLine, contentTable.ChildNodes.Where(x => x.Name == "#text" && !string.IsNullOrWhiteSpace(x.InnerText)).Select(x => x.InnerText.Trim())).Trim();
            var type = document.DocumentNode.SelectSingleNode(document.GetElementbyId("profileRows").ParentNode.XPath + "//table//tr//td//div//small").InnerHtml;
            var image = document.DocumentNode.SelectSingleNode(document.GetElementbyId("content").XPath + "//table//tr//td//div//a//img").GetAttributeValue("data-src", string.Empty);

            return new Character
            {
                Name = name.Trim(),
                Description = description.Trim(),
                Image = image,
                Page = url,
                Type = type.Trim()
            };
        }

        #endregion
    }
}