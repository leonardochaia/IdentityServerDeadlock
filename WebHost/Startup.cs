using Microsoft.Owin;
using Owin;
using Configuration;
using IdentityServer3.Core.Configuration;
using Serilog;
using IdentityServer3.Host.Config;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Models;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(WebHost.Startup))]

namespace WebHost
{
    public class Startup
    {
        private class InsecureRedirectUriValidator : IRedirectUriValidator
        {
            public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
            {
                return Task.FromResult(true);
            }

            public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
            {
                return Task.FromResult(true);
            }
        }
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Trace(outputTemplate: "{Timestamp} [{Level}] ({Name}){NewLine} {Message}{NewLine}{Exception}")
                .CreateLogger();

            var factory = new IdentityServerServiceFactory()
                        .UseInMemoryUsers(Users.Get())
                        .UseInMemoryClients(Clients.Get())
                        .UseInMemoryScopes(Scopes.Get());

            factory.RedirectUriValidator = new Registration<IRedirectUriValidator>(new InsecureRedirectUriValidator());

            var options = new IdentityServerOptions
            {
                SigningCertificate = Certificate.Load(),
                Factory = factory,
                RequireSsl = false,
            };

            app.Map("/core", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(options);
            });
        }
    }
}