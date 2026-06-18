using HarmonyLib;
using MBMScripts;
using System;

namespace CustomAssetTestResource.Patches;

[HarmonyPatch(typeof(GameManager), "InitializeData")]
public static class GameManagerPatch
{
    internal static event Action? BeforeDataInitialized;

    public static void Prefix()
    {
        BeforeDataInitialized?.Invoke();
    }
}