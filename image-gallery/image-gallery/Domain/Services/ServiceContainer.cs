using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace image_gallery.Domain.Services
{
    public sealed class ServiceContainer
    {
        private static IServiceProvider _services;

        private static object _serviceLocker = new();

        public static IServiceProvider Services
        {
            get
            {
                if (_services == null)
                {
                    lock (_serviceLocker)
                    {
                        if (_services == null)
                        {
                            _services = CreateHostBuilder(null).Build().Services;
                        }
                    }
                }
                return _services;
            }
        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                services.AddDbContext<PicturesContext>(options => options.UseSqlite("Filename=PicturesDB.db")
                ));
            return host;
        }
    }
}
