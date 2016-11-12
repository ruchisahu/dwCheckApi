# DwCheckApi - .NET Core
## Description

This project is a .NET core implemented Web API for listing all of the (canon) [Discworld](https://en.wikipedia.org/wiki/Discworld#Novels) novels.

It uses Entity Framework Core to communicate with a Sqlite database, which contains a record for each of the Discworld novels.

## Building and Running

1. Change directory to the root of the code

    `cd dwCheckApi`

1. Issue the `dotnet` restore command (this resolves all NuGet packages)

    `dotnet restore`

1. Issue the `dotnet` build command

    `dotnet build`

    This step isn't fully neccessary, but I like to do build and run as separate steps.

1. Issue the `dotnet` run command

    `dotnet run`

    This will start the Kestrel webserver, load the `dwCheckApi` application and tell you, via the terminal, what the url to access `dwCheckApi` will be. Usually this will be `http://localhost:5000`, but it may be different based on your system configuration.

## Polling and Usage of the API

`dwCheckApi` has the following Controllers:

1. Books

    The `Books` controller has two methods:

    1. Get

        The `Get` action takes an integer Id. This field represents the ordinal for the novel. This ordinal is based on release order, so if the user want data on 'Night Watch', they would set a GET request to:

            /Books/Get/29
        
        This will return the following JSON data:

            {
                "bookId":8,
                "bookOrdinal":29,
                "bookName":"Night Watch",
                "bookIsbn10":"0552148997",
                "bookIsbn13":"9780552148993",
                "bookDescription":"This morning, Commander Vimes of the City Watch had it all. He was a Duke. He was rich. He was respected. He had a titanium cigar case. He was about to become a father. This morning he thought longingly about the good old days. Tonight, he's in them.",
                "bookCoverImage":null,
                "bookCoverImageUrl":"http://wiki.lspace.org/mediawiki/images/4/4f/Cover_Night_Watch.jpg"
            }

    1. Search

        The `Search` action takes a string parameter called `searchString`. `dwCheckApi` will search the following fields of all Book records and return once which have any matches:

        - BookName
        - BookDescription
        - BookIsbn10
        - BookIsbn13

        If the user wishes to search for the prase "Rincewind", then they should issue the following request:

            /Books/Search?searchString=Rincewind
        
        This will return the following JSON data:

            [
              {
                  "bookId":23,
                  "bookOrdinal":2,
                  "bookName":"The Light Fantastic",
                  "bookIsbn10":"0861402030",
                  "bookIsbn13":"9780747530794",
                  "bookDescription":"As it moves towards a seemingly inevitable collision with a malevolent red star, the Discworld has only one possible saviour. Unfortunately, this happens to be the singularly inept and cowardly wizard called Rincewind, who was last seen falling off the edge of the world ....",
                  "bookCoverImage":null,
                  "bookCoverImageUrl":"http://wiki.lspace.org/mediawiki/images/f/f1/Cover_The_Light_Fantastic.jpg"
              },
              {
                  "bookId":30,
                  "bookOrdinal":9,
                  "bookName":"Eric",
                  "bookIsbn10":"0575046368",
                  "bookIsbn13":"9780575046368",
                  "bookDescription":"Eric is the Discworld's only demonology hacker. Pity he's not very good at it. All he wants is three wishes granted. Nothing fancy - to be immortal, rule the world, have the most beautiful woman in the world fall madly in love with him, the usual stuff. But instead of a tractable demon, he calls up Rincewind, probably the most incompetent wizard in the universe, and the extremely intractable and hostile form of travel accessory known as the Luggage. With them on his side, Eric's in for a ride through space and time that is bound to make him wish (quite fervently) again - this time that he'd never been born.",
                  "bookCoverImage":null,
                  "bookCoverImageUrl":"http://wiki.lspace.org/mediawiki/images/2/27/Cover_Eric_%28alt%29.jpg"
              },
              {
                  "bookId":38,
                  "bookOrdinal":17,
                  "bookName":"Interesting Times",
                  "bookIsbn10":"0552142352",
                  "bookIsbn13":"9780552142359",
                  "bookDescription":"Mighty Battles! Revolution! Death! War! (and his sons Terror and Panic, and daughter Clancy). The oldest and most inscrutable empire on the Discworld is in turmoil, brought about by the revolutionary treatise What I Did On My Holidays. Workers are uniting, with nothing to lose but their water buffaloes. Warlords are struggling for power. War (and Clancy) are spreading through the ancient cities. And all that stands in the way of terrible doom for everyone is: Rincewind the Wizzard, who can't even spell the word 'wizard' ... Cohen the barbarian hero, five foot tall in his surgical sandals, who has had a lifetime's experience of not dying ...and a very special butterfly.",
                  "bookCoverImage":null,
                  "bookCoverImageUrl":"http://wiki.lspace.org/mediawiki/images/9/96/Cover_Interesting_Times.jpg"
              }
            ]

## Seeding the Database

During startup, in the Configure method, `dwCheck` will apply any outstanding mirgrations (which is not a fantastic practise, but will be ok for now) then seeds the database via the `EnsureSeedData` extention method. This is an automatic process and requires no user input.

## Data Source

The [L-Space wiki](http://wiki.lspace.org/mediawiki/Bibliography#Novels) is currently being used to seed the database. All data is copyright to Terry Pratchett and/or Transworld Publishers no infringement was intended.