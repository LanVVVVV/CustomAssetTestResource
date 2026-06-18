using HarmonyLib;
using MBMScripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAssetTestResource.Features;

public static class DatabaseReplace
{
    public static void AllGameManagerDatabaseReplace()
    {
        var modName = ModEntry.ModName;

        ReplaceDatabase<ConfigData>(modName);
        ReplaceDatabase<TraitData>(modName);

        ReplaceDatabase<HumanData>(modName);
        ReplaceDatabase<ElfData>(modName);
        ReplaceDatabase<DwarfData>(modName);
        ReplaceDatabase<NekoData>(modName);
        ReplaceDatabase<InuData>(modName);
        ReplaceDatabase<UsagiData>(modName);
        ReplaceDatabase<HitsujiData>(modName);
        ReplaceDatabase<DragonianData>(modName);

        ReplaceDatabase<SuccubusData>(modName);

        ReplaceDatabase<SylviaData>(modName);
        ReplaceDatabase<ClaireData>(modName);
        ReplaceDatabase<AureData>(modName);
        ReplaceDatabase<KarenData>(modName);
        ReplaceDatabase<ViviData>(modName);
        ReplaceDatabase<BellaData>(modName);
        ReplaceDatabase<AnnaData>(modName);
        ReplaceDatabase<NeroData>(modName);

        ReplaceDatabase<AmiliaData>(modName);
        ReplaceDatabase<FloraData>(modName);
        ReplaceDatabase<NielData>(modName);
        ReplaceDatabase<SenaData>(modName);
        ReplaceDatabase<LenaData>(modName);
        ReplaceDatabase<BarbaraData>(modName);

        ReplaceDatabase<PlayerCharacterData>(modName);
        ReplaceDatabase<ClientData>(modName);
        ReplaceDatabase<HorseData>(modName);

        ReplaceDatabase<GoblinData>(modName);
        ReplaceDatabase<OrcData>(modName);
        ReplaceDatabase<WerewolfData>(modName);
        ReplaceDatabase<MinotaurData>(modName);
        ReplaceDatabase<SalamanderData>(modName);

        ReplaceDatabase<TentacleData>(modName);

        // Additional branches: Items, Events, Rooms, Achievements, etc.
        ReplaceDatabase<ItemData>(modName);
        ReplaceDatabase<EventData>(modName);
        ReplaceDatabase<RoomData>(modName);
        ReplaceDatabase<UpgradeData>(modName);
        ReplaceDatabase<AchievementData>(modName);
        ReplaceDatabase<LikeabilityData>(modName);
        ReplaceDatabase<MakingData>(modName);
        ReplaceDatabase<Niel1Data>(modName);
        ReplaceDatabase<SpineData>(modName);
    }

    private static void ReplaceDatabase<T>(string modName) where T : Data
    {
        if (TryLoadData<T>(modName, out var wrapper))
        {
            var list = wrapper?.list ?? new List<T>();
            var nameDict = new Dictionary<string, T>(list.Count);
            var idDict = new Dictionary<int, T>(list.Count);

            foreach (var entry in list)
            {
                if (!string.IsNullOrEmpty(entry.DataName))
                    nameDict[entry.DataName] = entry;
                idDict[entry.DataId] = entry;
            }

            var trv = Traverse.Create(typeof(Database<T>));
            trv.Field("List").SetValue(list);
            trv.Field("DataNameDictionary").SetValue(nameDict);
            trv.Field("DataIdDictionary").SetValue(idDict);

            ModEntry.Log($"Database<{typeof(T).Name}> has been replaced with custom JSON data");
            return;
        }
        //else
        //{
        //    ModEntry.Log($"External {typeof(T).Name}.json not found, keeping original data");
        //}
    }

    private static bool TryLoadData<T>(string modName, out SeqJsonListWrapper<T>? wrapper) where T : Data
    {
        wrapper = null;
        string fileName = typeof(T).Name + ".json";

        if (!ConfigSystem.TryLoadExternalFile(modName, fileName, out var asset))
            return false;

        try
        {
            wrapper = JsonUtility.FromJson<SeqJsonListWrapper<T>>(asset!.text);
            return wrapper != null;
        }
        catch (Exception ex)
        {
            ModEntry.LogError($"[ConfigSystem] Failed to parse {fileName}: {ex.Message}");
            return false;
        }
    }
}