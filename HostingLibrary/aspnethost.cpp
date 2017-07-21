
#include "stdafx.h"
#include "aspnethost.h"
#include <Windows.h>
#include <thread>

request_handler request_callback;
shutdown_handler shutdown_callback;

void register_request_callback(request_handler callback)
{
    // Set the request handler callback so we can call it when a request comes in
    request_callback = callback;
}

void register_shutdown_callback(shutdown_handler callback)
{
    shutdown_callback = callback;
}

void unregister_request_callback()
{
    request_callback = nullptr;
}

void unregister_shutdown_callback()
{
    shutdown_callback = nullptr;
}

void shutdown_application()
{
    if (shutdown_callback != nullptr) 
    {
        shutdown_callback();
    }
}

int execute(int argc, const wchar_t* argv[])
{
    // Load the hostfxr library from the .NET Core installation
    auto module = LoadLibraryW(L"C:\\Program Files\\dotnet\\host\\fxr\\2.0.0-preview2-25407-01\\hostfxr.dll");

    if (module == nullptr) 
    {
        // .NET Core 2.0 not installed
        return -1;
    }

    // Get the entry point for main
    auto proc = (hostfxr_main_fn)GetProcAddress(module, "hostfxr_main");

    return proc(argc, argv);
}
