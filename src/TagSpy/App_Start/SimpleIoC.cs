using InstaSharp;
using Serilog;
using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using TagSpy.Models;
using WindowsAzure.Table;
using Microsoft.WindowsAzure.Storage.Table;
using TagSpy.Data;

namespace TagSpy
{
    public class SimpleIoC
    {
        public static void Initialize()
        {
            // Create the container as usual.
            var container = new Container();

            //setup logger
            container.RegisterSingle<ILogger>(() =>
                new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .WriteTo.Trace()
                .CreateLogger()
            );
            
            //instagram config for InstaSharp
            container.RegisterSingle<InstagramConfig>(() =>
            {
                var clientId = ConfigurationManager.AppSettings["clientId"];
                var clientSecret = ConfigurationManager.AppSettings["clientSecret"];
                var callbackUri = ConfigurationManager.AppSettings["callbackUri"];
                var redirectUri = ConfigurationManager.AppSettings["redirectUri"];

                var icfg = new InstagramConfig(clientId, clientSecret);
                icfg.CallbackUri = callbackUri;
                icfg.RedirectUri = redirectUri;
                return icfg;
            }
            );

            // Azure storage account
            container.RegisterSingle(() =>
            {
                var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
                return CloudStorageAccount.Parse(connectionString);
            });

            // Azure table client
            container.RegisterSingle(() => container.GetInstance<CloudStorageAccount>().CreateCloudTableClient());

            //register our tablesets, in this case we only have one
            container.RegisterWebApiRequest<ITableSet<RealtimeSubscription>>(() =>
                new TableSet<RealtimeSubscription>(container.GetInstance<CloudTableClient>())
                );

            //register repositories
            container.Register<ISubscriptionRepository, SubscriptionRepository>(Lifestyle.Singleton);

            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}