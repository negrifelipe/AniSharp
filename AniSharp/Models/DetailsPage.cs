using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AniSharp.Models
{
    public class DetailsPage
    {
        /// <summary>
        /// Gets the id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets the image
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets the sypnopsis
        /// </summary>
        public string Synopsis { get; set; }

        /// <summary>
        /// Gets the statics
        /// </summary>
        public Statics Statics { get; set; }

        #region async

        /// <summary>
        /// Gets all the characters that had taken part; This takes a lot of time use <see cref="GetCharacterCardsAsync"/>
        /// </summary>
        /// <returns>A collection with the characters</returns>
        public async Task<List<Character>> GetCharactersAsync()
        {
            var document = await AniSharp.Web.LoadFromWebAsync($"{Url}/characters");

            var characters = new List<Character>();

            foreach (var url in ParseCaracters(document))
            {
                var web = await AniSharp.Web.LoadFromWebAsync(url);
                characters.Add(AniSharp.ParseCharacter(web));
            }

            return characters;
        }

        /// <summary>
        /// Gets all the characters that had taken part; Takes less time and its better than <see cref="GetCharactersAsync"/>
        /// </summary>
        /// <returns>A collection with the characters</returns>
        public async Task<List<CharacterCard>> GetCharacterCardsAsync()
        {
            var document = await AniSharp.Web.LoadFromWebAsync($"{Url}/characters");

            return ParseCharacterCards(document);
        }

        /// <summary>
        /// Gets all the pictures from the pictures section
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
        /// Gets all the characters that had taken part; This takes a lot of time use <see cref="GetCharacterCardsAsync"/>
        /// </summary>
        /// <returns>A collection with the characters</returns>
        public List<Character> GetCharacters()
        {
            var document = AniSharp.Web.Load($"{Url}/characters");

            var characters = new List<Character>();

            foreach (var url in ParseCaracters(document))
            {
                characters.Add(AniSharp.ParseCharacter(AniSharp.Web.Load(url)));
            }

            return characters;
        }

        /// <summary>
        /// Gets all the characters that had taken part; Takes less time and its better than <see cref="GetCharacters"/>
        /// </summary>
        /// <returns>A collection with the characters</returns>
        public List<CharacterCard> GetCharacterCards()
        {
            var document = AniSharp.Web.Load($"{Url}/characters");

            return ParseCharacterCards(document);
        }

        /// <summary>
        /// Gets all the pictures from the pictures section
        /// </summary>
        /// <returns></returns>
        public List<string> GetPictures()
        {
            var document = AniSharp.Web.Load($"{Url}/pics");

            return ParsePictures(document);
        }

        #endregion

        #region internal

        internal List<CharacterCard> ParseCharacterCards(HtmlDocument document)
        {
            var nav = document.GetElementbyId("horiznav_nav");

            var staff = document.DocumentNode.SelectNodes(nav.ParentNode.XPath + "//a").FirstOrDefault(x => x.GetAttributeValue("name", string.Empty) == "staff");

            var tables = document.DocumentNode.SelectNodes(nav.ParentNode.XPath + "//table//tr//td//div//small");

            List<CharacterCard> characters = new List<CharacterCard>();

            foreach (var table in tables.Select(x => x.ParentNode.ParentNode).Where(x => staff.ParentNode.ChildNodes.IndexOf(x.ParentNode.ParentNode) < staff.ParentNode.ChildNodes.IndexOf(staff)))
            {
                var nameNode = document.DocumentNode.SelectSingleNode(table.XPath + "//a");
                characters.Add(new CharacterCard()
                {
                    Name = nameNode.InnerText.Trim(),
                    Page = nameNode.GetAttributeValue("href", string.Empty),
                    Image = document.DocumentNode.SelectSingleNode(table.ParentNode.XPath + "//td//div//a//img").GetAttributeValue("data-src", string.Empty),
                    Type = document.DocumentNode.SelectSingleNode(table.XPath + "//div//small").InnerText
                });
            }

            return characters;
        }

        internal List<string> ParseCaracters(HtmlDocument document)
        {
            var nav = document.GetElementbyId("horiznav_nav");

            var staff = document.DocumentNode.SelectNodes(nav.ParentNode.XPath + "//a").FirstOrDefault(x => x.GetAttributeValue("name", string.Empty) == "staff");

            var tables = document.DocumentNode.SelectNodes(nav.ParentNode.XPath + "//table//tr//td//div//small");

            var characters = new List<string>();

            foreach (var table in tables.Select(x => x.ParentNode.ParentNode).Where(x => staff.ParentNode.ChildNodes.IndexOf(x.ParentNode.ParentNode) < staff.ParentNode.ChildNodes.IndexOf(staff)))
            {
                var nameNode = document.DocumentNode.SelectSingleNode(table.XPath + "//a");

                var page = nameNode.GetAttributeValue("href", string.Empty);

                characters.Add(page);
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
