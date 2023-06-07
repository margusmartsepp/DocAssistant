using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using DocAssistant.Helpers;
using DocAssistant.Services;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace DocAssistant.Dialogs
{
	internal class MainDialog : ComponentDialog
	{
		public static string MainFlow = $"{nameof(MainDialog)}.mainFlow";
		public static string MainDialogCreationDealDialog = $"{nameof(MainDialog)}.creationDealDialog";

		private readonly IStateService _stateService;

		public MainDialog(
			IStateService legacyStateService) : base(nameof(MainDialog))
		{
			_stateService = legacyStateService ?? throw new ArgumentNullException(nameof(legacyStateService));

			InitializeWaterfallDialog();
		}

		private void InitializeWaterfallDialog()
		{
			// Create Waterfall Steps
			var waterfallSteps = new WaterfallStep[]
			{
				InitialStepAsync,
				FinalStepAsync
			};

			// Add Named Dialogs
			AddDialog(new WaterfallDialog(MainFlow, waterfallSteps));

			// Set the starting Dialog
			InitialDialogId = MainFlow;
		}
		private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var userInput = stepContext.Context.Activity.Text.ToLowerInvariant();

			if (stepContext.Context.Activity.Text == "version")
			{
				var version = Assembly.GetEntryAssembly()
					.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
					.InformationalVersion;

				await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Blackstar MS Teams bot version: {version}"), cancellationToken);

			}

			return await stepContext.NextAsync(null, cancellationToken);
		}

		private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			return await stepContext.EndDialogAsync(null, cancellationToken);
		}
	}
}
