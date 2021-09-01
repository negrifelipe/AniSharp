using System;
using System.Diagnostics;

namespace AniSharp.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter the anime you want to search");
                var name = Console.ReadLine();
                Console.WriteLine("Cache mode? true or false");

                bool cache = false;

                if(!bool.TryParse(Console.ReadLine(), out cache))
                {
                    Console.WriteLine("Could not parse the value");
                    Console.WriteLine("Setting to false...");
                }

                if (cache)
                    AniSharp.EnableCache();
                else
                    AniSharp.DisableCache();

                Stopwatch watch = new Stopwatch();
                watch.Start();
                var anime = AniSharp.GetAnimeFromName(name);
                watch.Stop();
                Console.WriteLine("");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("");
                Console.WriteLine($"Getting the data took: {watch.Elapsed.TotalSeconds}");
                Console.WriteLine("");
                Console.WriteLine($"Id: {anime.Id}");
                Console.WriteLine($"Url: {anime.Url}");
                Console.WriteLine($"Name: {anime.Name}");
                Console.WriteLine($"English Name: {anime.Information.EnglishName}");
                Console.WriteLine($"Japanse Name: {anime.Information.JapaneseName}");
                Console.WriteLine($"Rank: {anime.Statics.Rank}");
                Console.WriteLine($"Score: {anime.Statics.Score}");
                Console.WriteLine($"Synopsis: {anime.Synopsis}");
                Console.WriteLine($"Producers {string.Join(", ", anime.Information.Producers)}");
                Console.WriteLine($"Genres: {string.Join(", ", anime.Information.Genres)}");
                Console.WriteLine("");

                foreach(var picture in anime.GetPictures())
                {
                    Console.WriteLine("===");
                    Console.WriteLine(picture);
                    Console.WriteLine("===");
                }
                
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("");
            }
        }
    }
}
