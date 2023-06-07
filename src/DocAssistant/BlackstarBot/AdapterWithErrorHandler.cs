// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.13.2

using DocAssistant.Services;

using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.TraceExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DocAssistant
{
	public class AdapterWithErrorHandler : CloudAdapter
	{
		public AdapterWithErrorHandler(IConfiguration configuration, ILogger<AdapterWithErrorHandler> logger, IStateService serviceState)
			: base(configuration, logger: logger)
		{
			OnTurnError = async (turnContext, exception) =>
			{
				// Log any leaked exception from the application.
				logger.LogError(exception, $"[OnTurnError] unhandled error : {exception.Message}");

				// Send a message to the user
				await turnContext.SendActivityAsync("The bot encountered an error or bug.");
				await turnContext.SendActivityAsync("To continue to run this bot, please fix the bot source code.");

				// Send a trace activity, which will be displayed in the Bot Framework Emulator
				await turnContext.TraceActivityAsync("OnTurnError Trace", exception.Message, "https://www.botframework.com/schemas/error", "TurnError");

				await serviceState.ConversationState.ClearStateAsync(turnContext);
				await serviceState.UserState.ClearStateAsync(turnContext);

			};
		}
	}
}
