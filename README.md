# Dingler

## Information
A server emulator for Hex: Shards of Fate written in C# that attempts to mimic how it ran prior to its shutdown.

## Requirements
- Visual Studio 2026 or Rider
- Most recent Hex client from either Steam or standalone

## Installation
- Getting the most recent version of the release is the first part. After that, you must go to the client and copy the following files into the install folder:
  - Assembly-CSharp-firstpass.dll
  - ICSharpCode.SharpZipLib.dll
  - NCalc.dll
  - SampleClassLibrary.dll
  - System.EnterpriseServices.dll
  - System.Web.Services.dll
  - UnityEngine.dll
- Update the appsettings.json file to point at your Hex: Shards of Fate installation directory.
  - If installed fresh from Steam this is <SteamFolder>/steamapps/common/HEX SHARDS OF FATE
- Update your client to point at the game server and auth. To just run locally by default these should be changed in config.ini in the above Hex directory
  - GameServerIP=127.0.0.1
  - CZEAuthUrl=http://localhost:5000/auth/hexlogin
- If at any point someone hosts a real server for gameplay, you'll point at their urls instead.
- Boot Dingler.Auth first followed by Dingler.Terminal
- Hit Start Server and wait for it to boot
- Enjoy playing Hex again!

## Known issues
While this attempts to be as accurate as possible there are some issues that present themselves as backend bugs when they are just presentational.
- Sir Pies, Brown Fox Scout, and Subterranean Spy do not work correctly. The current implementation of the rules engine does not reflect when the game has extra known zones and therefore does not send the card info correctly to the players
- Scrounge visual effect does not work. This is just a visual bug, the fiery effect just doesn't work yet. 
- The Tournament system is held together with hopes and dreams. It may have leaks. It may break. It's so rough man.
- If you see any other issues please report it. Let it be known that any bugs that existed in the final version of Hex will likely be left alone. If at some point we want to make this the definitive version we can do that

## Supported features
- Everyone gets four of every card. No packs. Just straight everything.
- 1v1 matches for standard and immortal

## Roadmap
- Direct challenges
- 8+ man tournaments
- Ability to schedule tournaments for you and your friends
- Limited
- Special formats like Rock and Corinth
- Making the tournament system not so embarassing

## FAQ
- How do I register? There's no page.
  - I didn't want to bother making a bespoke registration page. Just type your username in with any password. If that user does not exist, congrats, you own it with that password.
- Will PVE be supported?
  - At this point, I don't have the bandwidth, that being said, if you want to work on it, fork it and let's gooooooo.
- Why are the releases so large?
  - In order to keep using these easy for all users, the compiled binaries are self contained so you don't need to download or install a huge number of dependencies from Microsoft
