using DistribuidoraDoChines.Mvc.Areas.Identity;
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace DistribuidoraDoChines.Mvc.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => { });
        }
    }
}