// ApplicationHost.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "..\HostingLibrary\aspnethost.h"

int main()
{
    // Execute the sample ASP.NET application
    wchar_t* path = L"..\\SampleApplication\\bin\\Debug\\netcoreapp2.0\\SampleApplication.dll";
    const wchar_t* args[2];

    // The first argument is mostly ignored
    args[0] = L"C:\\Users\\dfowler\\.dotnet\\x64\\dotnet.exe";
    args[1] = path;
    return execute(2, args);
}

