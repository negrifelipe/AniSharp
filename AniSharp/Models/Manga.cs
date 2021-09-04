namespace AniSharp.Models
{
    public class Manga : DetailsPage
    {
        /// <summary>
        /// Creates a new instance of <see cref="Manga"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="image"></param>
        /// <param name="synopsis"></param>
        /// <param name="statics"></param>
        public Manga(int id, string name, string url, string image, string synopsis, Statics statics, MangaInformation information) : base(id, name, url, image, synopsis, statics)
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
        /// Gets the manga extra information
        /// </summary>
        public MangaInformation Information { get; set; }
    }
}
