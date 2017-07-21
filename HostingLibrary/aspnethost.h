#pragma once


typedef int(*hostfxr_main_fn) (const int argc, const wchar_t* argv[]);
typedef void(*request_handler_cb) (int error, void* http_context, void* state);
typedef void(*request_handler) (void* http_context, request_handler_cb callback, void* state);
typedef void(*shutdown_handler) ();

extern "C" __declspec(dllexport) void register_request_callback(request_handler callback);
extern "C" __declspec(dllexport) void register_shutdown_callback(shutdown_handler callback);
extern "C" __declspec(dllexport) void unregister_request_callback();
extern "C" __declspec(dllexport) void unregister_shutdown_callback();
extern "C" __declspec(dllexport) void shutdown_application();
extern "C" __declspec(dllexport) int execute(int argc, const wchar_t* argv[]);
