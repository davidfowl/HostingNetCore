## Hosting .NET Core from C++

This is a sample showing how to use hostfxr.dll to host a managed application. It assumes you have .NET Core preview 2 installed. If you
don't have it installed, get it from [here](https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.0.0-preview2-download.md).

## ASP.NET Core native server

The sample also shows how the library hosting the .NET Core application can be used to wire into the ASP.NET pipeline. This could be useful
for integrating as a native web server plugin (nginx etc).
