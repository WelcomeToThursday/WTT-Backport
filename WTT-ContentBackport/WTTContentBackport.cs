using System.Reflection;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using WTTContentBackport.Helpers;
using Range = SemanticVersioning.Range;

namespace WTTContentBackport;

public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.wtt.contentbackport";
    public string Name { get; init; } = "WTT-ContentBackport";
    public string Author { get; init; } = "GrooveypenguinX";
    public List<string>? Contributors { get; init; } = null;
    public SemanticVersioning.Version Version { get; init; } =
        new(typeof(ModMetadata).Assembly.GetName().Version?.ToString(3));
    public Range SptVersion { get; init; } = new("~4.1.1");
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, Range>? ModDependencies { get; init; } =
        new() { { "com.wtt.commonlib", new Range("~3.0.2") } };
    public string? Url { get; init; }
    public string License { get; init; } = "MIT";

    public bool HasPrepatcher { get; init; } = false;
}

[Injectable(TypePriority = OnLoadOrder.Preload + 2)]
public class WTTContentBackport(
    WTTServerCommonLib.WTTServerCommonLib wttCommon,
    BackportQuestHelper backportQuestHelper,
    BackportJunkDisabler backportJunkDisabler,
    ISptLogger<WTTContentBackport> logger
) : IOnLoad
{
    public async Task OnLoadAsync(CancellationToken cancellationToken)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        wttCommon.CustomRigLayoutService.CreateRigLayouts(assembly);
        await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);
        await wttCommon.CustomAssortSchemeService.CreateCustomAssortSchemes(assembly);
        await wttCommon.CustomHeadService.CreateCustomHeads(assembly);
        await wttCommon.CustomClothingService.CreateCustomClothing(assembly);
        await wttCommon.CustomVoiceService.CreateCustomVoices(assembly);
        await wttCommon.CustomCustomizationService.CreateCustomCustomizations(assembly);
        await wttCommon.CustomAchievementService.CreateCustomAchievements(assembly);
        await wttCommon.CustomBotLoadoutService.CreateCustomBotLoadouts(assembly);
        await wttCommon.CustomQuestItemService.CreateCustomQuestItems(assembly);
        await wttCommon.CustomLocaleService.CreateCustomLocales(assembly);
        backportQuestHelper.ModifyQuests();
        backportJunkDisabler.AddDogtagsToPmCs();
        backportJunkDisabler.AddItemsToRewardItemBlacklist();
    }
}
