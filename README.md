# Twitch Auth Example
A C# example of how to use Twitch's [OAuth authorization code flow](https://dev.twitch.tv/docs/authentication) to generate a user access token (as well as refresh token). We'll be using [TwitchLib.Api](https://github.com/TwitchLib/TwitchLib.Api) to make calls to Twitch, as well as [CNHttp](https://github.com/MiffOttah/CNHttp) to host a local web server for the OAuth flow. Instructions on how to get those below.

## Overview
Twitch's [OAuth implementation](https://dev.twitch.tv/docs/authentication) is fairly standard is does not deviate from [OAuth standards](https://oauth.net/2/). At a high level, you'll create an application on Twitch's developer site. This will automatically create a client id, client secret, and you'll have the chance to setup the redirect uri (where the user goes after authenticating on Twitch's auth portal). For this example, you should use "http://localhost". When the example application is run, a local web server will be started, listening on port 80, and will capture the user being bounced to http://localhost post authorization. Twitch sends them to the redirect uri address, along with a few query string parameters, including `code`. We will extract the `code` value from the query string, and use it, in combination with the client secret, client id, and redirect uri to complete the authorization flow with Twitch. Twitch will return a payload include `access_token`, `refresh_token`, `expires_in`, `scopes`, and a few other fields. Most important to us are the tokens and the expires in value. The access token expires at the end of the number of seconds provided in `expires_in`. You should use the refresh token (which does not expire unless the user revokes access to the application) to request a new access token.

### Developer Application
The first step is to create an application on Twitch's developer portal: https://dev.twitch.tv . Login with your Twitch account. After being logged in, on the right side click Register Your Application. Give it a name, and an oAuth redirect url of `http://localhost`. Select a category, and hit the Create button. Click into the application to view its details. You'll find the Client Id (note this), as well as the button called New Secret. Click the button to generate a new secret and note it. At this point you should have a client id and client secret.

### Determine The Scope
OAuth uses granular scope values to determine what kind of priviledges and data an application has access to. For Twitch, you can find the list of available scopes at: https://dev.twitch.tv/docs/authentication#scopes . Note down the scopes you're interested in.

### Setting Up The Example
In the `Config.cs` file, you'll find three fields: `TwitchClientId`, `TwitchRedirectUri`, and `TwitchClientSecret`. Fill in these fields (uri is `http://localhost`). 

In `Program.cs`, around line 10, you'll find a private list variable named `scopes` containing a number of prepopulated scopes. Feel free to remove/update/change these to your liking.

### Run The Example
Run the program. You'll see a couple things printed out, with the most recent being an authorization URL. Visit this URL. If you are not logged in, you'll be logged in. If you are logged in, you'll be asked to authorize your account against the application for the specified scopes. On authorization, you'll be bounced to `http://localhost` where you should be met with `Authorization started! Check your application!`. On the application, you'll find something along the lines of:
```
Authorization success!

User: <username> (id: <userid>)
Access token: <access_token>
Refresh token: <refresh_token>
Expires in: <expires_in_seconds>
Scopes: <csv_of_scopes>
```
This example calls the Twitch api with the recently generated access token to discover the calling user (username, userid).

## Packages Used
- [TwitchLib.Api](https://github.com/MiffOttah/CNHttp): C# Twitch api library that wraps all Twitch helix, v5, auth and undocumented endpoints, as well as some helper third party stuff.
- [CNHttp](https://github.com/MiffOttah/CNHttp): .NET Core ported NHttp web server library.

**You might need to select the "Include prerelease" checkbox in the NuGet package browser to see/get the right versions.**

## Contributors
 * Cole ([@swiftyspiffy](http://twitter.com/swiftyspiffy))

## Questions?
Ping me on Twitter: https://twitter.com/swiftyspiffy
