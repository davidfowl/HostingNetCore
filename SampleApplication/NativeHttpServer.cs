using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApplication
{
    public class NativeHttpServer : IServer
    {
        public delegate void request_handler_cb(int error, IntPtr http_context, IntPtr state);
        public delegate void request_handler(IntPtr http_context, request_handler_cb callback, IntPtr state);

        [DllImport("HostingLibrary.dll")]
        public static extern void register_shutdown_callback(Action callback);

        [DllImport("HostingLibrary.dll")]
        public static extern void register_request_callback(request_handler callback);

        [DllImport("HostingLibrary.dll")]
        public static extern void unregister_request_callback();

        public IFeatureCollection Features { get; } = new FeatureCollection();

        public Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
        {
            // Start the server by registering the callback (async void is evil but it gets the job done)
            register_request_callback(async (http_context, cb, state) =>
            {
                var features = MakeFeatureCollection(http_context);

                // Create the hosting context
                var context = application.CreateContext(features);

                try
                {
                    await application.ProcessRequestAsync(context);
                    application.DisposeContext(context, exception: null);
                    cb(0, http_context, state);
                }
                catch (Exception ex)
                {
                    application.DisposeContext(context, ex);
                    cb(0, http_context, state);
                }
            });

            return Task.CompletedTask;
        }

        private IFeatureCollection MakeFeatureCollection(IntPtr http_context)
        {
            // TODO: Make a feature collection represenation for the native http context representation
            var features = new FeatureCollection();
            var request = new HttpRequestFeature();
            var response = new HttpResponseFeature();
            features.Set<IHttpRequestFeature>(request);
            features.Set<IHttpResponseFeature>(response);
            return features;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // TODO: Drain pending requests

            // Stop all further calls back into managed code by unhooking the callback
            unregister_request_callback();

            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseNativeHttpServer(this IWebHostBuilder builder)
        {
            return builder.ConfigureServices(services =>
            {
                services.AddSingleton<IServer, NativeHttpServer>();
            });
        }
    }
}
