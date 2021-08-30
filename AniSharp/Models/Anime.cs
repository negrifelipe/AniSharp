using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AniSharp.Models
{
    public class Anime
    {
        /// <summary>
        /// Gets the id of the anime
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the anime name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the url of the anime page
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets the image of the anime
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets the anime sypnopsis
        /// </summary>
        public string Synopsis { get; set; }

        /// <summary>
        /// Gets the anime url information
        /// </summary>
        public AnimeInformation Information { get; set; }

        /// <summary>
        /// Gets the statics on the anime
        /// </summary>
        public AnimeStatics Statics { get; set; }

        #region async

        /// <summary>
        /// Gets all the characters that had taken part of the anime
        /// </summary>
        /// <returns></returns>
        public async Task<List<AnimeCharacter>> GetCharactersAsync()
        {
            var document = await AniSharp.Web.LoadFromWebAsync($"{Url}/characters");

            return ParseCaracters(document);
        }

        /// <summary>
        /// Gets all the pictures of the anime from the pictures section
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetPicturesAsync()
        {
            var document = await AniSharp.Web.LoadFromWebAsync($"{Url}/pics");

            return ParsePictures(document);
        }

        #endregion

        #region sync

        /// <summary>
        /// Gets all the characters that had taken part of the anime
        /// </summary>
        /// <returns></returns>
        public List<AnimeCharacter> GetCharacters()
        {
            var document = AniSharp.Web.Load($"{Url}/characters");

            return ParseCaracters(document);
        }

        /// <summary>
        /// Gets all the pictures of the anime from the pictures section
        /// </summary>
        /// <returns></returns>
        public List<string> GetPictures()
        {
            var document = AniSharp.Web.Load($"{Url}/pics");

            return ParsePictures(document);
        }

        #endregion

        #region internal

        internal List<AnimeCharacter> ParseCaracters(HtmlDocument document)
        {
            var nav = document.GetElementbyId("horiznav_nav");

            var staff = document.DocumentNode.SelectNodes(nav.ParentNode.XPath + "//a").FirstOrDefault(x => x.GetAttributeValue("name", string.Empty) == "staff");

            var tables = document.DocumentNode.SelectNodes(nav.ParentNode.XPath + "//table//tr//td//div//small");

            List<AnimeCharacter> characters = new List<AnimeCharacter>();

            foreach (var table in tables.Select(x => x.ParentNode.ParentNode).Where(x => staff.ParentNode.ChildNodes.IndexOf(x.ParentNode.ParentNode) < staff.ParentNode.ChildNodes.IndexOf(staff)))
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

        internal List<string> ParsePictures(HtmlDocument document)
        {
            var container = document.GetElementbyId("horiznav_nav").ParentNode;

            return document.DocumentNode.SelectNodes(container.XPath + "//table//tr//td//div//a//img").Select(x => x.GetAttributeValue("data-src", string.Empty)).ToList();
        }

        #endregion
    }
}
