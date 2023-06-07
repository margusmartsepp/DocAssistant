using BlackstarBot.Models;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;

namespace DocAssistant.Services
{
	public interface IStateService
	{
		ConversationState ConversationState { get; }
		UserState UserState { get; }

		IStatePropertyAccessor<DialogState> DialogStateAccessor { get; }
		IStatePropertyAccessor<UserProfile> UserProfileAccessor { get; }
	}

	internal class StateService : IStateService
	{
		#region Variables

		public ConversationState ConversationState { get; }
		public UserState UserState { get; }

		public static string DialogStateId { get; } = $"{nameof(StateService)}.DialogState";
		public static string UserProfileId { get; } = $"{nameof(StateService)}.UserProfile";

		public IStatePropertyAccessor<UserProfile> UserProfileAccessor { get; set; }
		public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }

		#endregion

		public StateService(UserState userState, ConversationState conversationState)
		{
			ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
			UserState = userState ?? throw new ArgumentNullException(nameof(userState));

			InitializeAccessors();
		}

		public void InitializeAccessors()
		{
			// Initialize Conversation State Accessors
			DialogStateAccessor = ConversationState.CreateProperty<DialogState>(DialogStateId);
			UserProfileAccessor = UserState.CreateProperty<UserProfile>(UserProfileId);
		}
	}
}
