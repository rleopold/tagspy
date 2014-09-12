using System.Web.Http;
using Microsoft.WindowsAzure.Storage.Table;
using TagSpy.Models;
using ElCamino.AspNet.Identity.AzureTable;
using System.Threading.Tasks;

namespace TagSpy
{
    public class TableInitializer
    {
        public static async void Initialize(HttpConfiguration config)
        {
            var tableClient = (CloudTableClient)config.DependencyResolver.GetService(typeof(CloudTableClient));

            // Initialize required azure tables here
            tableClient.GetTableReference(typeof(RealtimeSubscription).Name).CreateIfNotExists();

            //initialize azure Identity store
            var azureIdStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            await azureIdStore.CreateTablesIfNotExists();
        }
    }
}