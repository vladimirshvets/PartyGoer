# PartyGoer
A playground for chat bots.


### Currently supported bots:
- Telegram


## Tech Stack
- C# 11, .NET 7.0
- ASP.NET Core MVC (web interface)
- MS SQL Server (data storage)
- Redis (cache)
- NLog (logger)


## Setup
1. docker-compose up -d
2. ...


## Docker Images
- MS SQL Server: mcr.microsoft.com/mssql/server:2022-latest
  * For Apple M1/M2 Silicon: Docker Desktop Dashboard -> Settings -> “Features in development” -> select the “Use Rosetta for x86/amd64 emulation on Apple Silicon” checkbox.
- Redis: redis/redis-stack


## Related Links
- [.NET Client for Telegram Bot API]
- [Quickstart: Run SQL Server Linux container images with Docker]


[.NET Client for Telegram Bot API]: https://github.com/TelegramBots/Telegram.Bot
[Quickstart: Run SQL Server Linux container images with Docker]: https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker
[MS SQL Server Docker Images]: https://mcr.microsoft.com/en-us/product/mssql/server/about