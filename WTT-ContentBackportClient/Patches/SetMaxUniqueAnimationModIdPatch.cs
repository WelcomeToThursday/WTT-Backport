using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EFT;
using EFT.InventoryLogic;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace WTTContentBackportClient.Patches
{
    public class SetMaxUniqueAnimationModIdPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(
                typeof(Player.FirearmController),
                "SetMaxUniqueAnimationModId"
            );
        }

        [PatchPrefix]
        private static bool Prefix(Player.FirearmController __instance)
        {
            if (!IsOurShotgun(__instance.Item.TemplateId))
                return true;

            List<Mod> result = [];
            foreach (var slot in __instance.Item.AllSlots)
            {
                if (slot.ContainedItem is not Mod mod)
                    continue;

                result.Add(mod);
            }

            if (result.Count == 0)
            {
                SetShotgunFloat(__instance, 0f);
                return false;
            }

            var maxId = int.MinValue;
            foreach (var mod in result)
            {
                if (maxId < mod.UniqueAnimationModID)
                    maxId = mod.UniqueAnimationModID;
            }

            __instance.FirearmsAnimator.SetUniqueAnimationModId(maxId);

            float animFloat = maxId == 14 ? 1f : 0f;
            SetShotgunFloat(__instance, animFloat);

            return false;
        }

        private static void SetShotgunFloat(Player.FirearmController fc, float value)
        {
            var animator = fc.FirearmsAnimator?.Animator;
            animator?.SetFloat(
                AnimationControllerParametersTable.FLOAT_ANIMATION_MOD_ID_FLOAT,
                value
            );
        }

        private static bool IsOurShotgun(string templateId) =>
            templateId is "5e870397991fd70db46995c8" or "5a7828548dc32e5a9c28b516";
    }
}
