using Orleans.Runtime;

namespace Orleans.Redis.Issues.Client
{
	public class Worker : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider;

		public Worker(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await using var serviceScope = _serviceProvider.CreateAsyncScope();

			var grainFactory = serviceScope.ServiceProvider.GetRequiredService<IGrainFactory>();

			var managementGrain = grainFactory.GetGrain<IManagementGrain>(0);

			while (!stoppingToken.IsCancellationRequested)
			{
				await Task.Delay(30000, stoppingToken);

				try
				{
					var membershipEntries = await managementGrain.GetDetailedHosts();

					var siloRuntimeStatistics = await managementGrain.GetRuntimeStatistics(membershipEntries.Select(x => x.SiloAddress).ToArray());
				}
				catch { }
			}
		}
	}
}