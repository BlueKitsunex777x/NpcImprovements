using System;
using HarmonyLib;

//
// --------------- Patch to ignore player damage to Npcs
//
[HarmonyPatch(typeof(Health), "Attacked")]
internal static class AttackedPatch
{
    private static bool Prefix(Health __instance, HealthBase from)
    {
        try
        {
            if (NpcImprovementsPlugin.Attack || !from.CompareTag("Player") || !__instance.CompareTag("NPC")) return true;
            return false;
        }
        catch (Exception e) { L.Err("AttackedPatch", e); }
        return true;
    }
}