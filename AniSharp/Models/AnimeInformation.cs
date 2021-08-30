namespace AniSharp.Models
{
    public class AnimeInformation
    {
        /// <summary>
        /// Gets the anime name in japanese; Can be empty
        /// </summary>
        public string JapaneseName { get; set; }

        /// <summary>
        /// Gets the anime name in english; Can be empty
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// Gets the amount of episodes
        /// </summary>
        public int Episodes { get; set; }

        /// <summary>
        /// Gets the status of the anime; Ex: Finished Airing or Currently Airing 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets the release date of the anime
        /// </summary>
        public string Aired { get; set; }

        /// <summary>
        /// Gets the season when the anime was releaed
        /// </summary>
        public string Season { get; set; }

        /// <summary>
        /// The date where this anime is broadcasted
        /// </summary>
        public string Broadcast { get; set; }

        /// <summary>
        /// The producers that had taken part of this anime
        /// </summary>
        public string[] Producers { get; set; }

        /// <summary>
        /// The licensor of the anime
        /// </summary>
        public string[] Licensors { get; set; }

        /// <summary>
        /// The studios that had taken part of this anime
        /// </summary>
        public string[] Studios { get; set; }

        /// <summary>
        /// Where this anime came from
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The genres of the anime
        /// </summary>
        public string[] Genres { get; set; }

        /// <summary>
        /// How much does every episode is long
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// The score of anime
        /// </summary>
        public string Score { get; set; }
    }
}
