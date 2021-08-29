using System.Collections.Generic;

namespace AniSharp.Models
{
    public class Anime
    {
        /// <summary>
        /// Gets the anime name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the image of the anime
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets the anime sypnopsis
        /// </summary>
        public string Synopsis { get; set; }

        /// <summary>
        /// Gets the anime information
        /// </summary>
        public AnimeInformation Information { get; set; }

        /// <summary>
        /// Gets the statics on the anime
        /// </summary>
        public AnimeStatics Statics { get; set; }

        /// <summary>
        /// Gets all the caracters that had taken part of the anime
        /// </summary>
        public List<AnimeCharacter> Characters { get; set; }
    }
}
