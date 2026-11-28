using System;
using System.IO;
using System.Reflection;
using Diz.Resources;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace WTTContentBackportClient.Patches;

public sealed class FullBundlePathPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(EasyBundle), nameof(EasyBundle.Load));
    }

    [PatchPrefix]
    private static void Prefix(EasyBundle __instance)
    {
        var path = __instance._path;
        if (string.IsNullOrEmpty(path))
            return;

        if (path.StartsWith("SPT_Runtime/user/mods/WTT-ContentBackport/bundles/",
                StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith(@"SPT_Runtime\user\mods\WTT-ContentBackport\bundles\",
                StringComparison.OrdinalIgnoreCase))
        {
            __instance._path = Path.GetFullPath(path, global::BepInEx.Paths.GameRootPath);
        }
    }
}