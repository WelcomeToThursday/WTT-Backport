using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Comfort.Common;
using EFT;
using EFT.Hideout;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace WTTContentBackportClient.Patches;

public class EnvironmentUIAwakePatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.Method(typeof(EnvironmentUI), nameof(EnvironmentUI.Awake));

    [PatchPrefix]
    public static void PatchPrefix(EnvironmentUI __instance)
    {
        var list = __instance._environments?.ToList() ?? [];

        foreach (var def in CustomEnvironmentRegistry.Definitions)
        {
            if (list.Any(x => x.Type == def.Type))
                continue;

            list.Add(
                new EnvironmentUI.EnvironmentData
                {
                    Type = def.Type,
                    SceneName = def.SceneName,
                    EligibleVersions = Array.Empty<string>(),
                }
            );
        }

        __instance._environments = list.ToArray();
    }
}

public class EnvironmentUISetEnvironmentPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod() =>
        AccessTools.Method(typeof(EnvironmentUI), nameof(EnvironmentUI.SetEnvironmentAsync));

    [PatchPrefix]
    public static void PatchPrefix(
        EnvironmentUI __instance,
        ref EEnvironmentUIType environmentUiType
    )
    {
        if (!CustomEnvironmentRegistry.ByType.TryGetValue(environmentUiType, out var def))
            return;

        if (!CustomEnvironmentBundleLoader.EnsureLoaded(def, Plugin.Log))
        {
            Plugin.Log.LogError(
                $"Failed to preload custom environment bundle for type: {environmentUiType}"
            );
        }
    }
}
