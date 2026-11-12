using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers.Commerce;
using SPTarkov.Server.Core.Helpers.Dialogue.Commando.SptCommands;
using SPTarkov.Server.Core.Helpers.Profile;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Dialog;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Services.Commerce;
using WTTServerCommonLib.Helpers;

namespace WTTContentBackport.Commands;

[Injectable]
public class PvestigeMeCommand(
    MailSendService mailSendService,
    RewardHelper rewardHelper,
    ProfileHelper profileHelper) : ISptCommand
{
    public ValueTask<string> PerformAction(UserDialogInfo commandHandler, MongoId sessionId, SendMessageRequest request)
    {
        var profile = profileHelper.GetFullProfile(sessionId); 
        rewardHelper.AddAchievementToProfile(profile, "6948990c05f4f91bdb9a56f3");
        rewardHelper.AddAchievementToProfile(profile, "6af0fd88f31e06338eee7800");

        profile.AddCustomisation("6aa40cf673e497d3080a99bc", "shootingRangeMark", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6aa4105726814f849d09243f", "shootingRangeMark", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6aa41155850607cb1402b9ef", "shootingRangeMark", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6ab66eb051f69f0feb032ef7", "shootingRangeMark", CustomisationSource.DEFAULT);

        return new ValueTask<string>(request.DialogId);
    }

    public string Command => "pvestigeme";

    public string CommandHelp => "Usage: Receive PVE Prestige content";
}