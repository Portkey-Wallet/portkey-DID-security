using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers.MongoDB.Configuration;
using Orleans.Statistics;

namespace CASecurity.Silo.Extensions;

public static class OrleansHostExtensions
{
     public static IHostBuilder UseOrleansSnapshot(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseOrleans((context, siloBuilder) =>
        {
            var configSection = context.Configuration.GetSection("Orleans");
            Console.WriteLine("configSection\t" + JsonConvert.SerializeObject(configSection));
            Console.WriteLine("configSection AdvertisedIP\t" + configSection.GetValue<string>("AdvertisedIP"));
            var isRunningInKubernetes = configSection.GetValue<bool>("isRunningInKubernetes");
            var advertisedIP = isRunningInKubernetes ?  Environment.GetEnvironmentVariable("POD_IP") :configSection.GetValue<string>("AdvertisedIP");
            var clusterId = isRunningInKubernetes ? Environment.GetEnvironmentVariable("ORLEANS_CLUSTER_ID") : configSection.GetValue<string>("ClusterId");
            var serviceId = isRunningInKubernetes ? Environment.GetEnvironmentVariable("ORLEANS_SERVICE_ID") : configSection.GetValue<string>("ServiceId");
            
            //Configure OrleansSnapshot
            siloBuilder
                //.ConfigureEndpoints(advertisedIP:IPAddress.Parse(configSection.GetValue<string>("AdvertisedIP")),siloPort: configSection.GetValue<int>("SiloPort"), gatewayPort: configSection.GetValue<int>("GatewayPort"), listenOnAnyHostAddress: true)
                .ConfigureEndpoints(advertisedIP: IPAddress.Parse(advertisedIP),
                    siloPort: configSection.GetValue<int>("SiloPort"),
                    gatewayPort: configSection.GetValue<int>("GatewayPort"), listenOnAnyHostAddress: true)
                .UseMongoDBClient(configSection.GetValue<string>("MongoDBClient"))
                .UseMongoDBClustering(options =>
                {
                    options.DatabaseName = configSection.GetValue<string>("DataBase");;
                    options.Strategy = MongoDBMembershipStrategy.SingleDocument;
                })
                .AddMongoDBGrainStorage("Default",(MongoDBGrainStorageOptions op) =>
                {
                    op.CollectionPrefix = "GrainStorage";
                    op.DatabaseName = configSection.GetValue<string>("DataBase");
                
                    op.ConfigureJsonSerializerSettings = jsonSettings =>
                    {
                        // jsonSettings.ContractResolver = new PrivateSetterContractResolver();
                        jsonSettings.NullValueHandling = NullValueHandling.Include;
                        jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;
                        jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                    };
                    
                })
                .UseMongoDBReminders(options =>
                {
                    options.DatabaseName = configSection.GetValue<string>("DataBase");
                    options.CreateShardKeyForCosmos = false;
                })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = clusterId;
                    options.ServiceId = serviceId;
                })
               // .AddMemoryGrainStorage("PubSubStore")
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
                .UseDashboard(options => {
                    options.Username = configSection.GetValue<string>("DashboardUserName");
                    options.Password = configSection.GetValue<string>("DashboardPassword");
                    options.Host = "*";
                    options.Port = configSection.GetValue<int>("DashboardPort");
                    options.HostSelf = true;
                    options.CounterUpdateIntervalMs = configSection.GetValue<int>("DashboardCounterUpdateIntervalMs");
                })
                .UseLinuxEnvironmentStatistics()
                .ConfigureLogging(logging => { logging.SetMinimumLevel(LogLevel.Debug).AddConsole(); });
        });
    }
}