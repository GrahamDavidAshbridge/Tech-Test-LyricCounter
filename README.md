# To Run CLI
**In Command Prompt**
- Open CMD
- CD to directory the exe is installed
- enter command 
- > LyricCounter.Cli -a "Artists Name" 

**In Terminal**
- CD/LS to directory 
- enter command
- > dotnet run -a "Artists Name"

**In IDE (Options)**
1. As above using terminal
2. Set program argument of -a "Artist Name" in IDE build options.

![image](https://user-images.githubusercontent.com/69345270/149851007-c37df90b-545f-4104-a390-684b2837da78.png)


# Improvements

2.5 to 3 hours was spent on this project. Going forwards if this was to be a fully featured application or more time would be spent, the following suggested improvements would be recommended.
- Estabilish and use a distrubuted Cache for API results.
- Structure using Clean Architecture (Jason Taylor).
- Validation of API responses using FluentValidation.
- Research more robust APIs.  Currently using the provided API and a free lyric api from google results.
- Implemented integration tests using wiremock.net
- Improved logging away from a local file.


# Dependencies
- .Net 6
- Ardalis.GuardClauses (Used for basic guard clauses due to lack of response validation, see above)
- CommandLineParser (parse value from arguments)
- Microsoft.Extensions.Configuration.Json (Accessing settings from appsettings.json)
- Microsoft.Extensions.DependencyInjection (Accessing DI)
- Microsoft.Extensions.Http.Poly -> Http resilience (retries)
- Microsoft.Extensions.Http -> Api calls.
- SeriLog -> Logging
- Serilog.Extensions.Logging.File -> Writing log to file.
- Moq -> Unit test mock objects.
- xunit -> Unit testing.
- Shouldly -> Unit testing.
- Serilog.Sinks.TestCorrelator -> Unit testing for serilog.
