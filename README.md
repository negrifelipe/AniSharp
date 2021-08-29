<h1 align="center">AniSharp</h1>
![NugetDownloads](https://img.shields.io/nuget/dt/Feli.AniSharp?label=Nuget%20Downloads)
<h3 align="center">This library makes easier to get data from my myanimelist.net using web scraping. This doesnt use the mal api so there can be some bugs or problems using it. In my case everything works great. Take in mind that this library is still on development</h3>

# Installation 
```
dotnet add package Feli.AniSharp
```

# Basic Usage
With AniSharp there are no needs to login using credentials you can directly get the data from the web

```csharp
// If you want you can use cache so requesting the same site several times will be faster
AniSharp.EnableCache();

// If you want you can disable cache in this way
AniSharp.DisableCache();

// Gets the anime called Shinmai Maou no Testament (https://myanimelist.net/anime/23233/Shinmai_Maou_no_Testament)
var anime = AniSharp.GetAnimeFromName("Testament");

// Gets the top of ova
var ovas = AniSharp.GetTopAnime(type: TopTypes.Ova);

// Gets the default top of anime from the 50
var animes = AniSharp.GetTopAnime(startIndex: 50);

// You can also get things asynchronously
var anime = await AniSharp.GetAnimeFromNameAsync("Testament");

// Lets print the anime name and synopsis
Console.WriteLine(anime.Name);
Console.WriteLine(anime.Synopsis);
```

# Libraries Used
This project uses html-agility-pack licensed under the mit license https://github.com/zzzprojects/html-agility-pack/blob/master/LICENSE
