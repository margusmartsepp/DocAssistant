using System.Threading;
using System.Threading.Tasks;
using BlackstarBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector;

namespace DocAssistant.Services
{
	public static class UserProfileFactory
	{
		public static async Task<UserProfile> CreateUserProfile(ITurnContext turnContext, CancellationToken cancellationToken)
		{
			var channel = turnContext.Activity?.ChannelId;

			if (channel is Channels.Emulator or Channels.Test)
			{
				return new UserProfile
				{
					Name = "EmulatorTest",
					Email = "EmulatorTest@gunvorgroup.com",
					IsTest = true,
				};
			}

			if (channel != Channels.Msteams)
			{
				return new UserProfile
				{
					Name = "AzureTest",
					Email = "AzureTest@gunvorgroup.com",
					IsTest = true,
				};
			}

			var channelAccount = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

			var userProfile = new UserProfile
			{
				Name = channelAccount.Name,
				Email = channelAccount.Email,
				IsTest = false,
			};

			return userProfile;
		}
	}
}
