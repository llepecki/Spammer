using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spammer.Data;
using Spammer.Infrastructure;
using Spammer.Security;
using Spammer.Sending;

namespace Spammer
{
    public class Startup
    {
        private const string AuthenticationScheme = "Basic";
        private const string SpamDirectory = @"C:\Spam";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AuthenticationScheme)
                .AddScheme<TrivialAuthenticationOptions, TrivialAuthenticationHandler>(AuthenticationScheme, options =>
                {
                    options.User = "admin";
                    options.Password = "drezyn4";
                });

            services.AddMvc();

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add(MailRouteConstraint.Name, typeof(MailRouteConstraint));
                options.ConstraintMap.Add(TopicRouteConstraint.Name, typeof(TopicRouteConstraint));
            });

            services.AddTransient<ISystemClock, SystemClock>();

            services.AddTransient<ISubscriptionRepository, InMemorySubscriptionRepository>();

            services.AddTransient<ISendContent, SendToDirectory>(provider => new SendToDirectory(provider.GetService<ISystemClock>(), SpamDirectory));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
