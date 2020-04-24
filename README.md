# Imagination-Server
A C# server for LEGO Universe, that will almost definitely die.

## More Info
Unlike (most) other LEGO Universe servers, this uses C#, a managed, Object-Oriented programming language. The purpose of it is to show that it is possible to do this. It is run as a single application, and uses SQLite with Fluent NHibernate for storing data.

## Modules
### Base
ImaginationServer.Core - C++ code that provides a wrapper around RakNet
ImaginationServer.Common - Base C# server code shared between servers
### Servers
ImaginationServer.Auth - Authentication server, and has commands
ImaginationServer.Auth.Packets - C++ code that handles certain things that I didn't implement in C# for auth yet (this should be removed eventually)
ImaginationServer.World - World server
ImaginationServer.World.Packets - Same as .Auth.Packets, except for world.
### Utilities
sd0 utils: (Broken) sd0 compressor/decompressor
### Other
ImaginationServer.SingleServer - Application that runs the Auth and World servers.
ImaginationServer.GameLauncher - A launcher/patcher for LU, allows switching boot.cfg with a few clicks (not implemented)
ImaginationServer.ControlPanel - Same as single server, but with a GUI (not implemented)
