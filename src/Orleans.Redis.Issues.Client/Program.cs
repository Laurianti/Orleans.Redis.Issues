using StackExchange.Redis;

namespace Orleans.Redis.Issues.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = Host
				.CreateDefaultBuilder(args)
				.ConfigureServices(services =>
				{
					services.AddHostedService<Worker>();

					services.AddOrleansClient(sb =>
					{
						var configurationOptions = new ConfigurationOptions
						{
							User = "default",
							Password = "AitzAI9VdQL8J1nF9ua1tdccpB1FCeTo",
							EndPoints = { { "localhost", 6379 } }
						};

						sb.UseRedisClustering(options =>
						{
							options.ConfigurationOptions = configurationOptions;
						});
					});
				})
				.Build();

			await host.RunAsync();
		}
	}
}