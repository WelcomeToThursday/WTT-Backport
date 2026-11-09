using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Callbacks;
using SPTarkov.Server.Core.Controllers;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Helpers.Commerce;
using SPTarkov.Server.Core.Helpers.Dialogue.Commando.SptCommands;
using SPTarkov.Server.Core.Helpers.Profile;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Eft.Dialog;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Dialog;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Commerce;
using WTTServerCommonLib.Helpers;

namespace WTTContentBackport.Commands;

[Injectable]
public class TarCoinWellSpentCommand(
    MailSendService mailSendService,
    RewardHelper rewardHelper,
    ProfileHelper profileHelper) : ISptCommand
{
    public ValueTask<string> PerformAction(UserDialogInfo commandHandler, MongoId sessionId, SendMessageRequest request)
    {
        var profile = profileHelper.GetFullProfile(sessionId);
        rewardHelper.AddAchievementToProfile(profile, "6948990c05f4f91bdb9a56f3");

        // rave
        profile.AddCustomisation("6a86df54697fb20fa70d37bd", "wall", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6a86df2254304bc4eb06c37a", "floor", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6a86de41697fb20fa70d37b8", "ceiling", CustomisationSource.DEFAULT);

        // referral

        profile.AddCustomisation("6aa842e1a6a6fa9766035bed", "wall", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6aa84285a2113a960901f1a7", "floor", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6aa8414c151289a0fb022a41", "ceiling", CustomisationSource.DEFAULT);

        // asia

        profile.AddCustomisation("6acf399973cbffda7101405b", "wall", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6acf39694a8e45c81506a6e9", "floor", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6acf394bd5e58156f10a8f6a", "ceiling", CustomisationSource.DEFAULT);

        // military 
        profile.AddCustomisation("6acf358d343a6539f304b8fc", "wall", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6acf3564544ea940ab09344e", "floor", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6acf353f544ea940ab09344b", "ceiling", CustomisationSource.DEFAULT);

        return new ValueTask<string>(request.DialogId);
    }

    public string Command => "tarcoinwellspent";

    public string CommandHelp => "Usage: Unlocks all TarCoin exclusive customizations";
}