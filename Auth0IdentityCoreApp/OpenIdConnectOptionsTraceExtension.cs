using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions; 

public static class OpenIdConnectOptionsTraceExtension
{
/*    public static RemoteAuthenticationOptions AddTracing(this RemoteAuthenticationOptions options)
    {
        return options;
    }
*/
    public static OpenIdConnectOptions AddTracing(this OpenIdConnectOptions options)
    {
        return options;
    }

    public static Auth0WebAppOptions AddTracing(this Auth0WebAppOptions options)
    {
        options.OpenIdConnectEvents = new OpenIdConnectEvents()
        {
            OnRedirectToIdentityProvider = ctx =>
            {
                //Console.WriteLine("OnRedirectToIdentityProvider");
                //Console.WriteLine(ctx.Request.GetDisplayUrl());
                //EnumerateProperties(ctx.Options);

                var x = ctx.Request.GetDisplayUrl();

                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("You started the authentication process with the following options:");
                Console.WriteLine($"Authority: {ctx.Options.Authority}");
                Console.WriteLine($"Client ID: {ctx.Options.ClientId}");
                Console.WriteLine($"Client Secret: **********");
                Console.WriteLine($"Response Type: {ctx.Options.ResponseType}");
                Console.WriteLine($"Response Mode: {ctx.Options.ResponseMode}");
                Console.WriteLine($"Scope: {string.Join(" ", ctx.Options.Scope)}");
                Console.WriteLine($"Callback URL: {ctx.Options.CallbackPath}");


                return Task.CompletedTask;
            },
            OnMessageReceived = ctx =>
            {
                //Console.WriteLine("OnMessageReceived");
                //Console.WriteLine(ctx.Request.GetDisplayUrl());
                //EnumerateProperties(ctx.ProtocolMessage);

                return Task.CompletedTask;
            },
            OnAuthorizationCodeReceived = ctx =>
            {
                //Console.WriteLine("OnAuthorizationCodeReceived");
                //Console.WriteLine(ctx.Request.GetDisplayUrl());

                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("You received the following authorization code from the authorization server:");
                Console.WriteLine(ctx.ProtocolMessage.Code);
                
                return Task.CompletedTask;
            },
            OnTokenResponseReceived = ctx =>
            {
                //Console.WriteLine("OnTokenResponseReceived");
                //Console.WriteLine(ctx.Request.GetDisplayUrl());

                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("You received the following tokens from the authorization server:");

                if (!string.IsNullOrEmpty(ctx.TokenEndpointResponse.IdToken))
                {
                    Console.WriteLine("ID token:");
                    Console.WriteLine(ctx.TokenEndpointResponse.IdToken);
                    Console.WriteLine();
                }

                if (!string.IsNullOrEmpty(ctx.TokenEndpointResponse.AccessToken))
                {
                    Console.WriteLine("Access token:");
                    Console.WriteLine(ctx.TokenEndpointResponse.AccessToken);
                    Console.WriteLine();
                }

                if (!string.IsNullOrEmpty(ctx.TokenEndpointResponse.RefreshToken))
                {
                    Console.WriteLine("Refresh token:");
                    Console.WriteLine(ctx.TokenEndpointResponse.RefreshToken);
                    Console.WriteLine();
                }

                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                //Console.WriteLine("OnTokenValidated");
                //Console.WriteLine(ctx.Request.GetDisplayUrl());
                /*
                if (ctx.Result.Succeeded)
                {
                    Console.WriteLine("The token has been validated.");
                } else
                {
                    Console.WriteLine("The token is not valid.");
                }
                */
                return Task.CompletedTask;
            },
            OnTicketReceived = ctx =>
            {
                //Console.WriteLine("OnTicketReceived");
                //Console.WriteLine(ctx.Request.GetDisplayUrl());

                //Console.WriteLine(ctx.Result.Ticket?.ToString());

                return Task.CompletedTask;
            },
            OnRedirectToIdentityProviderForSignOut = ctx =>
            {
                //Console.WriteLine("OnRedirectToIdentityProviderForSignOut");
                //Console.WriteLine(ctx.Request.GetDisplayUrl());

                return Task.CompletedTask;
            },
            OnRemoteSignOut = ctx =>
            {
                Console.WriteLine("OnRemoteSignOut");

                return Task.CompletedTask;
            },
            OnSignedOutCallbackRedirect = ctx =>
            {
                Console.WriteLine("OnSignedOutCallbackRedirect");

                return Task.CompletedTask;
            },
            OnUserInformationReceived = ctx =>
            {
                Console.WriteLine("OnUserInformationReceived");

                return Task.CompletedTask;
            },
            OnAccessDenied = ctx =>
            {
                Console.WriteLine("OnAccessDenied");

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("OnAuthenticationFailed");

                return Task.CompletedTask;
            },
            OnRemoteFailure = ctx =>
            {
                Console.WriteLine("OnRemoteFailure");

                return Task.CompletedTask;
            }
        };

        return options;
    }

    private static void EnumerateProperties(Object obj)
    {
        var stringPropertyNamesAndValues = obj.GetType()
          .GetProperties()
          .Where(pi => pi.PropertyType == typeof(string) && pi.GetGetMethod() != null)
          .Select(pi => new
          {
              Name = pi.Name,
              Value = pi.GetGetMethod().Invoke(obj, null)
          });

        foreach (var pair in stringPropertyNamesAndValues)
        {
            if (pair.Value != null)
            {
                Console.WriteLine("Name: {0}", pair.Name);
                Console.WriteLine("Value: {0}", pair.Value);
            }
        }
    }
}