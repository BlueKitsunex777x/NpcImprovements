using System;
using BepInEx;
using BlockStoryCore;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

// Makes some improvements to Npcs
[BepInPlugin("com.BlueKitsunex777x.npcimprovements", "Npc Improvements", "1.0.0")]
[BepInDependency(Core.Guid)]               // hard dependency on the toolbox; won't load without it
public class NpcImprovementsPlugin : BaseUnityPlugin
{
    private InputActionReference _toggleAttack;

    public static bool Enabled = PlayerPrefs.GetInt("NpcImprovements_Enabled", 1) != 0;
    public static bool Attack = PlayerPrefs.GetInt("NpcImprovements_Attack", 0) != 0;

    private void Awake()
    {
        try
        {
            new Harmony("com.BlueKitsunex777x.npcimprovements").PatchAll();
            L.Info("loaded, Npc Improvements");
        }
        catch(Exception e) { L.Err("PatchAll", e); }

        _toggleAttack = BSKeybinds.Register("NpcImprovements", "Key: Attack Npcs", "<Keybind>/f12");
        
        ModRegistry.Register(new ModInfo
        {
            Name = "NpcImprovements",
            Description = "Makes some improvements to Npcs",
            GetEnabled = () => Enabled,
            SetEnabled = on => { Enabled = on; PlayerPrefs.SetInt("NpcImprovements_Enabled", on ? 1 : 0); PlayerPrefs.Save(); L.Info($"enabled = {on}"); },
            Config = new ModConfig()
                .Toggle("Attack Npcs", () => Attack, SetAttack, "On: Attack Npcs, Off: Can't Attack Npcs")
                .Header("Controls").Keybind("Key: Attack Npcs", _toggleAttack, "Toggles Attack Npcs in game")
        });
        Core.Log?.LogInfo("NpcImprovements loaded.");
    }

    private void Update()
    {
        if (Enabled && BSKeybinds.Pressed(_toggleAttack)) { Attack = !Attack; }
    }

    private static void SetAttack(bool value)
    {
        Attack = value;
        PlayerPrefs.SetInt("NpcImprovements_Attack", value ? 1 : 0);
        PlayerPrefs.Save();
    }
}