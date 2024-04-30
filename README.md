# Weather App
This repository contains a sample application showing the Core Identity feature supported by Auth0.

It's a Blazor Web application with Interactive Auto render mode secured with [Auth0](https://auth0.com/) and calling an internal and an external API providing random weather forecast data.

The application implements the [Backend for Frontend pattern](https://auth0.com/blog/the-backend-for-frontend-pattern-bff/) to protect tokens.

It also (partially) shows in the console what is happening under the hood during the authentication step.

## To run the applications:

Clone the repo: `git clone https://github.com/andychiare/auth0-core-identity-app`

To run the applications:

1. Go to the `ExternalAPI` folder
2. Follow the instructions in the `README.md` file to register, configure, and run the API.
3. Move to the `Auth0IdentityCoreApp` folder 
4. Add your Auth0 credentials to the `appsettings.json` configuration file.
5. Type `dotnet run` in a terminal window.
6. Point your browser to `https://localhost:7255`.

## Requirements:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed on your machine
- An [Auth0](https://auth0.com/) account.
