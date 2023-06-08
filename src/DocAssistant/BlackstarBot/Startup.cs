using System;
using System.Net;
using System.Net.Http;
using DocAssistant.Bots;
using DocAssistant.Configuration;
using DocAssistant.Dialogs;
using DocAssistant.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;

namespace DocAssistant
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddControllers().AddNewtonsoftJson();

			// Create the Bot Framework Adapter with error handling enabled.
			services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

			// Configure State
			ConfigureState(services);

			ConfigureDialogs(services);

			// Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
			services.AddTransient<IBot, DialogBot<MainDialog>>();
		}
		public void ConfigureDialogs(IServiceCollection services)
		{
			services.AddSingleton<MainDialog>();
		}
		public void ConfigureState(IServiceCollection services)
		{
			// Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.) 
			services.AddSingleton<IStorage, MemoryStorage>();
			//var storageAccount = "";
			//var storageContainer = "";

			//services.AddSingleton<IStorage>(new BlobsStorage(storageAccount, storageContainer));

			// Create the User state. 
			services.AddSingleton<UserState>();

			// Create the Conversation state. 
			services.AddSingleton<ConversationState>();

			// Create an instance of the state service 
			services.AddSingleton<IStateService, StateService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment() || env.IsEnvironment("Test"))
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseDefaultFiles()
				.UseWebSockets()
				.UseRouting()
				.UseAuthorization()
				.UseEndpoints(endpoints =>
				{
					endpoints.MapControllerRoute(
						name: "default",
						pattern: "{controller=Home}/{action=Index}/{id?}");
				});

			app.UseHttpsRedirection();
		}
	}
}
