# Pet GTR Turbo Kart Drift 3000 _!!!_ Deluxe Game of The Year Edition�

A game for CSS 475 (databases class).

[![Build status](https://ci.appveyor.com/api/projects/status/85yu79oweov3xyrc/branch/master?svg=true)](https://ci.appveyor.com/project/Chris-Johnston/pgtrtkd3000dgoty/branch/master)

![Logo image.](PetRaceTurbo30xx.png)

## Building

Building and running the application is done entirely in Visual Studio. All typescript files
get compiled into regular js automatically.

1. Make sure you have `npm` installed. This is bundled with Node JS, so you'll have to install that
too.

2. Install gulp: `npm i -g gulp gulp-cli` (This may require sudo/admin)


## Local Configuration

In your `PetGame` project configuration, add the following environment variable:

```
PETGAME_DB_CONNECTION_STRING Server=localhost;Database=PetGame;Trusted_Connection=True;
```

This will point the ASP.NET connections to your local database.