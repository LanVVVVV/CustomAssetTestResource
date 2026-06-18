using CustomAssetTestResource.Features;
using CustomAssetTestResource.Patches;
using CustomAssetTestResource.Properties;
using MBM.ModLoader.Core;
using UnityEngine;

namespace CustomAssetTestResource;

public static class ModEntry
{
    internal const string ModName = "CustomAssetTestResource";

    public static void Load()
    {
        ModConfig.ModSettingRegister();

        GameManagerPatch.BeforeDataInitialized += DatabaseReplace.AllGameManagerDatabaseReplace;

        Localization.OnLanguageChanged += OnLanguageChanged;
        Log($"{ModName} Mod loaded!");
    }

    internal static void Log(string msg) => Debug.Log($"[CATR] {msg}");

    internal static void LogWarning(string msg) => Debug.LogWarning($"[CATR] {msg}");

    internal static void LogError(string msg) => Debug.LogError($"[CATR] {msg}");

    private static void OnLanguageChanged(string langCode)
    {
        Strings.Culture = Localization.CurrentCulture;

        ModConfig.OnLanguageChanged();
        Log($"language changed: {langCode}");
    }
}