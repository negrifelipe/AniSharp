namespace AniSharp.Models
{
    public class Anime : DetailsPage
    {
        /// <summary>
        /// Creates a new instance of <see cref="Anime"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="image"></param>
        /// <param name="synopsis"></param>
        /// <param name="statics"></param>
        /// <param name="information"></param>
        public Anime(int id, string name, string url, string image, string synopsis, Statics statics, AnimeInformation information) : base(id, name, url, image, synopsis, statics)
        {
            Id = id;
            Name = name;
            Url = url;
            Image = image;
            Synopsis = synopsis;
            Statics = statics;
            Information = information;
        }

        /// <summary>
        /// Gets the anime url information
        /// </summary>
        public AnimeInformation Information { get; set; }
    }
}
