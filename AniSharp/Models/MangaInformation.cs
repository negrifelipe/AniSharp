namespace AniSharp.Models
{
    public class MangaInformation
    {
        /// <summary>
        /// Gets the manga name in japanese; Can be empty
        /// </summary>
        public string JapaneseName { get; set; }

        /// <summary>
        /// Gets the manga name in english; Can be empty
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// Gets the amount of volumes
        /// </summary>
        public int Volumes { get; set; }

        /// <summary>
        /// Gets the amount of chapters
        /// </summary>
        public int Chapters { get; set; }

        /// <summary>
        /// Gets the status of the manga
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets the publish date of the manga
        /// </summary>
        public string Published { get; set; }

        /// <summary>
        /// Gets the genres of the manga
        /// </summary>
        public string[] Genres { get; set; }

        /// <summary>
        /// Gets the authors of the manga
        /// </summary>
        public string[] Authors { get; set; }

        /// <summary>
        /// Gets the serialization magazine
        /// </summary>
        public string Serialization { get; set; }
    }
}
