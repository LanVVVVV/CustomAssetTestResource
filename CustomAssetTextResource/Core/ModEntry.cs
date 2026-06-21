using CustomAssetTextResource.Features;
using CustomAssetTextResource.Patches;
using CustomAssetTextResource.Properties;
using MBM.ModLoader.Core;
using System;
using System.IO;
using UnityEngine;

namespace CustomAssetTextResource.Core;

public static class ModEntry
{
    internal const string ModName = "CustomAssetTextResource";

    public static void Load()
    {
        FixConfigFolderName();
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

    private static void FixConfigFolderName()
    {
        string rootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
        string oldDir = Path.Combine(rootDir, "CustomAssetTestResource");
        string newDir = Path.Combine(rootDir, "CustomAssetTextResource");

        try
        {
            if (Directory.Exists(newDir)) return;
            if (!Directory.Exists(oldDir)) return;

            Directory.Move(oldDir, newDir);
            Log($"Renamed folder {GetRelativePath(oldDir)} to {GetRelativePath(newDir)}");
        }
        catch (Exception ex)
        {
            LogError($"Failed to rename mod config folder: {ex.Message}");
        }

        static string GetRelativePath(string fullPath)
{
            string rootDir = AppDomain.CurrentDomain.BaseDirectory;
            if (fullPath.StartsWith(rootDir))
            {
                return fullPath.Substring(rootDir.Length).TrimStart(Path.DirectorySeparatorChar);
            }
            return fullPath;
        }
    }
}