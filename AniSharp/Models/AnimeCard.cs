namespace AniSharp.Models
{
    public class AnimeCard
    {
        /// <summary>
        /// Gets the anime name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the score of the anime
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Gets the image url of the anime
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets the url of the anime page that contains more data
        /// </summary>
        public string Url { get; set; }
    }
}
