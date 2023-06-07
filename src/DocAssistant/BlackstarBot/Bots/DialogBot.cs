using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DocAssistant.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using DocAssistant.Helpers;
using Microsoft.Bot.Builder.Teams;

namespace DocAssistant.Bots
{
	internal class DialogBot<T> : ActivityHandler where T : Dialog
	{
		#region Variables
		protected readonly Dialog _dialog;
		protected readonly ILogger _logger;

		#endregion

		public DialogBot(IStateService stateService, T dialog, ILogger<DialogBot<T>> logger)
		{
			StateService = stateService ?? throw new System.ArgumentNullException(nameof(stateService));
			_dialog = dialog ?? throw new System.ArgumentNullException(nameof(dialog));
			_logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
		}

		protected IStateService StateService { get; }

		public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
		{
			await base.OnTurnAsync(turnContext, cancellationToken);

			// Save any state changes that might have occured during the turn.
			await StateService.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
			await StateService.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
		}

		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			//Remove the @entry for the bot
			turnContext.Activity.RemoveRecipientMention();

			_logger.LogInformation("Running dialog with Message Activity.");

			// Run the Dialog with the new message Activity.
			await _dialog.Run(turnContext, StateService.DialogStateAccessor, cancellationToken);
		}

		protected override async Task OnMembersAddedAsync(
			IList<ChannelAccount> membersAdded,
			ITurnContext<IConversationUpdateActivity> turnContext,
			CancellationToken cancellationToken)
		{
			foreach (var member in membersAdded)
			{
				if (member.Id == turnContext.Activity.Recipient.Id) continue;

				var userProfile = await UserProfileFactory.CreateUserProfile(turnContext, cancellationToken);

				await turnContext.SendActivityAsync(MessageFactory.Text($"Hi {userProfile.Name}. How can I help you today?"), cancellationToken);
			}
		}
	}
}

