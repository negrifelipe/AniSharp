namespace AniSharp.Models
{
    public class AnimeCharacter
    {
        /// <summary>
        /// Gets the name of the caracter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets a picture of the character
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets the url from the charecter page
        /// </summary>
        public string Page { get; set; }

        /// <summary>
        /// Gets the charecter type; Example: Main or Supporting
        /// </summary>
        public string Type { get; set; }
    }
}
