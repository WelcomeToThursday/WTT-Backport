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
public class CatBeachCommand(
    MailSendService mailSendService,
    RewardHelper rewardHelper,
    ProfileHelper profileHelper) : ISptCommand
{
    public ValueTask<string> PerformAction(UserDialogInfo commandHandler, MongoId sessionId, SendMessageRequest request)
    {
        var profile = profileHelper.GetFullProfile(sessionId);
        var pmcProfile = profile.CharacterData?.PmcData;
        var side = pmcProfile?.Info?.Side;
        
        rewardHelper.AddAchievementToProfile(profile, "6948990c05f4f91bdb9a56f3");
        profile.AddCustomisation("6a3bb59dae4fbdbb95040aab", "wall", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6a3bb5add4f941ec040cc3d9", "wall", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6a3bb5827182e638dc05bd2a", "floor", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6a3bb457200c84aede0f3f8d", "ceiling", CustomisationSource.DEFAULT); 

        profile.AddCustomisation("6a3575e125f478c7a20f613d", "shootingRangeMark", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6a3575489725a9cf0c0907ed", "shootingRangeMark", CustomisationSource.DEFAULT);
        profile.AddCustomisation("6a35762dd864773668046a6d", "shootingRangeMark", CustomisationSource.DEFAULT);

        IEnumerable<MongoId> kordBreachHeadsBear =
        [
            "6a3e61337462374d270c6696",
            "6a3e614ef7864d27bd030bfc",
        ];

        IEnumerable<MongoId> kordBreachHeadsUsec = 
        [
            "6a3e6177da479effd5076f1c",
            "6a3e6183d2260323500c178a"
        ];

        IEnumerable<MongoId> kordBreachVoicesUsec =
        [
            // usec
            "6a1d7767b27e39cbd4054a37",
            "6a57a9a7115aad3e1c000e30",
            // bear
        ];

        MongoId kordBreachVoiceBear = "6a57a87c86d1b3f59a039fbd";
        
        IEnumerable<MongoId> kordBreachDogtags =
        [
            "6a354c9673339990030ca46d",
            "6a354cdc993ed2447e01bc08",
            "6a354f75e68cab523a076007",
            "6a354d09bc8aada000066566",
            "6a354c14bc8aada00006655f"
        ];

        if (side?.ToLower() == "bear")
        {
            profile.AddCustomisations(kordBreachHeadsBear, "head", CustomisationSource.DEFAULT);
            profile.AddCustomisation(kordBreachVoiceBear, "voice");
        }
        else
        {
            profile.AddCustomisations(kordBreachHeadsUsec, "head", CustomisationSource.DEFAULT);
            profile.AddCustomisations(kordBreachVoicesUsec, "voice", CustomisationSource.DEFAULT);
        }
        
        profile.AddCustomisations(kordBreachDogtags, "dogTag", CustomisationSource.DEFAULT);
        
        mailSendService.SendUserMessageToPlayer(sessionId, commandHandler, "This REQUIRES a full game restart in order to see the new Head, Voice, and Dog Tag options.");
        return new ValueTask<string>(request.DialogId);
    }

    public string Command => "catbeach";
    public string CommandHelp => "Usage: Receive all Kord Breach customizations";
}
