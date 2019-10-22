using Harmony;

namespace DeviationCounter.Patches
{
    [HarmonyPatch(typeof(NoteCutInfo))]
    [HarmonyPatch("allIsOK", MethodType.Getter)]
    class NoteCutInfo_allIsOK
    {
        private static bool Prefix(NoteCutInfo __instance, ref bool __result)
        {
            __result = __instance.speedOK && __instance.directionOK && __instance.saberTypeOK && !__instance.wasCutTooSoon &&
                !(Plugin.useMaxDeviation && (__instance.timeDeviation * 1000 > Plugin.maxDeviation));
            return false;
        }
    }
}
