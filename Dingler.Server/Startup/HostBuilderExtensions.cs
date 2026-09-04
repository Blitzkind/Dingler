using System.Reflection;
using Dingler.Server.Abstractions;
using Dingler.Server.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dingler.Server.Startup;

public static class HostBuilderExtensions
{
	public static IHostBuilder BuildGameServer(this IHostBuilder builder,
		Action<HostBuilderContext, ServerConfiguration>? configAction = null)
	{
		builder
			.ConfigureServices((hb, sc) =>
			{
				sc.AddSingleton<ServerLifetimeManager>()
					.AddScoped<IDinglerServer, DinglerServer>()
					.AddScoped<ICancellationManager, CancellationManager>()
					.AddHandlers();

				var configuration = new ServerConfiguration();

				configAction?.Invoke(hb, configuration);

				var descriptors = sc
					.Where(d => d.ServiceType.IsGenericType &&
					            (d.ServiceType.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<>) ||
					             d.ServiceType.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>) ||
					             d.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
					             d.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
					.Select(d => d.ServiceType)
					.Distinct()
					.ToList();

				sc.AddScoped<IncomingPipeline>(sp =>
				{
					var handlerMiddleware = new HandlerMiddleware();

					foreach (var interfaceType in descriptors)
					{
						var handler = sp.GetRequiredService(interfaceType);
						var genericArgs = interfaceType.GetGenericArguments();
						var interfaceGenericDefinition = interfaceType.GetGenericTypeDefinition();

						var registerMethod = typeof(HandlerMiddleware)
							.GetMethods()
							.First(m =>
							{
								if (m.Name != nameof(HandlerMiddleware.RegisterHandler))
									return false;

								if (m.GetGenericArguments().Length != genericArgs.Length)
									return false;

								var parameters = m.GetParameters();
								if (parameters.Length != 1)
									return false;

								return parameters[0].ParameterType.GetGenericTypeDefinition() ==
								       interfaceGenericDefinition;
							});

						
						registerMethod
							.MakeGenericMethod(genericArgs)
							.Invoke(handlerMiddleware, [handler]);
					}

					configuration.IncomingPipelineBuilder.Use(handlerMiddleware);
					return new IncomingPipeline(configuration.IncomingPipelineBuilder.Build());
				});

				sc.AddScoped<OutgoingPipeline>(sp =>
					new OutgoingPipeline(configuration.OutgoingPipelineBuilder.Build()));
			});

		return builder;
	}

	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddScopedStartupService<TService>(Func<IServiceProvider, TService>? factory = null)
			where TService : class, IStartupService
		{
			if (factory is null)
				serviceCollection.AddScoped<TService>();
			else
				serviceCollection.AddScoped(factory);
		
			serviceCollection.AddScoped<IStartupService>(sp => sp.GetRequiredService<TService>());
			return serviceCollection;
		}

		public IServiceCollection AddScopedAsyncStartupService<TService>(Func<IServiceProvider, TService>? factory = null)
			where TService : class, IAsyncStartupService
		{
			if (factory is null)
				serviceCollection.AddScoped<TService>();
			else
				serviceCollection.AddScoped(factory);
		
			serviceCollection.AddScoped<IAsyncStartupService>(sp => sp.GetRequiredService<TService>());
			return serviceCollection;
		}
		
		public IServiceCollection AddSingletonStartupService<TService>(Func<IServiceProvider, TService>? factory = null)
			where TService : class, IStartupService
		{
			if (factory is null)
				serviceCollection.AddSingleton<TService>();
			else
				serviceCollection.AddSingleton(factory);
		
			serviceCollection.AddSingleton<IStartupService, TService>(sp => sp.GetRequiredService<TService>());
			return serviceCollection;
		}

		public IServiceCollection AddSingletonAsyncStartupService<TService>(Func<IServiceProvider, TService>? factory = null)
			where TService : class, IAsyncStartupService
		{
			if (factory is null)
				serviceCollection.AddSingleton<TService>();
			else
				serviceCollection.AddSingleton(factory);
		
			serviceCollection.AddSingleton<IAsyncStartupService>(sp => sp.GetRequiredService<TService>());
			return serviceCollection;
		}

		private IServiceCollection AddHandlers()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();

			var allTypes = assemblies.SelectMany(a =>
			{
				try
				{
					return a.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					return ex.Types.Where(t => t != null)!;
				}
			});
		
			var handlers = allTypes
				.Where(t => t is { IsClass: true, IsAbstract: false })
				.Where(t => t.GetInterfaces().Any(i =>
					i.IsGenericType &&
					(i.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<>) ||
					 i.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>) ||
					 i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
					 i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))))
				.ToList();

			foreach (var handler in handlers)
			{
				if (handler is null)
					continue;
				
				var interfaces = handler.GetInterfaces()
					.Where(i => i.IsGenericType &&
					            (i.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<>) ||
					             i.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>) ||
					             i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
					             i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

				foreach (var i in interfaces) serviceCollection.AddScoped(i, handler);
			}

			return serviceCollection;
		}
	}
}