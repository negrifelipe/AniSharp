using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AniSharp.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the anime you want to search");
            var name = Console.ReadLine();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var anime = AniSharp.GetAnimeFromName(name);
            watch.Stop();
            Console.WriteLine($"Getting the data took: {watch.Elapsed.TotalSeconds}");
            Console.WriteLine($"Name: {anime.Name}");
            Console.WriteLine($"Synopsis: {anime.Synopsis}");
            Console.WriteLine($"Producers {string.Join(", ", anime.Information.Producers)}");
            Console.WriteLine($"Genres: {string.Join(", ", anime.Information.Genres)}");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
