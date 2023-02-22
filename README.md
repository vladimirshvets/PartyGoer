# PartyGoer
A playground for chat bots.


### Currently supported bots:
- Telegram


## Setup
- C# 11, .NET 7.0
- ASP.NET Core MVC (web interface)
- MSSQL Server (data storage)
- Redis (caching chat info)

## Docker setup
- MS SQL Server: _mcr.microsoft.com/mssql/server:2022-latest_
  * For Apple M1/M2 Silicon: Docker Desktop Dashboard -> Settings -> “Features in development” -> select the “Use Rosetta for x86/amd64 emulation on Apple Silicon” checkbox.
- Redis: redis/redis-stack


## Related Links:
- [.NET Client for Telegram Bot API]
- [Quickstart: Run SQL Server Linux container images with Docker]


[.NET Client for Telegram Bot API]: https://github.com/TelegramBots/Telegram.Bot
[Quickstart: Run SQL Server Linux container images with Docker]: https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker
