using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AniSharp.Models
{
    public class Character
    {
        /// <summary>
        /// Gets the name of the character
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the description of the character
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the main picture of the character
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets the url from the charecter page
        /// </summary>
        public string Page { get; set; }

        /// <summary>
        /// Gets the character type; Example: Main or Supporting
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// A method to get a list with pictures of the character
        /// </summary>
        /// <returns>A collection that contains all the pictures</returns>
        public List<string> GetPictures() 
        {
            var pictures = new List<string>();

            var document = AniSharp.Web.Load($"{Page}");

            var nav = document.GetElementbyId("horiznav_nav");

            var picturesUrl = document.DocumentNode.SelectNodes(nav.XPath + "//ul//li//a").FirstOrDefault(x => x.InnerText == "Pictures").GetAttributeValue("href", string.Empty);

            if (pictures == null)
                return pictures;

            var picturesDocument = AniSharp.Web.Load(picturesUrl);

            var content = picturesDocument.GetElementbyId("content");

            var tables = picturesDocument.DocumentNode.SelectNodes(content.XPath + "//table//tr//td//div//table//tr");

            foreach(var table in tables)
            {
                var tablePictures = table.SelectNodes(table.XPath + "//td//div//a//img").Select(x => x.GetAttributeValue("data-src", string.Empty));

                foreach (var picture in tablePictures)
                    pictures.Add(picture);
            }

            return pictures;
        }
    }
}
