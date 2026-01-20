using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts._Data.Tomes;
using Il2CppAssets.Scripts.Actors.Player;
using Il2CppAssets.Scripts.Inventory__Items__Pickups;
using Il2CppAssets.Scripts.Inventory__Items__Pickups.Stats;
using Il2CppAssets.Scripts.Menu.Shop;

namespace SuperOldMan.BepInExMod
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class Core : BasePlugin
    {
        private const string MyGUID = "Slimaeus.SuperOldMan";
        private const string PluginName = "SuperOldMan";
        private const string VersionString = "1.0.1";

        public override void Load()
        {
            // Register Harmony Patches
            Harmony.CreateAndPatchAll(typeof(PlayerPatches));
            Log.LogInfo($"Plugin {PluginName} {VersionString} is loaded!");
        }

        [HarmonyPatch(typeof(MyPlayer))]
        public static class PlayerPatches
        {
            [HarmonyPatch(nameof(MyPlayer.Spawn))]
            [HarmonyPostfix]
            public static void Spawn_Postfix(MyPlayer __instance)
            {
                var xpTome = DataManager.Instance.tomeData[ETome.Xp];
                var xpStatModifiers = new Il2CppSystem.Collections.Generic.List<StatModifier>();

                xpStatModifiers.Add(new StatModifier
                {
                    stat = EStat.XpIncreaseMultiplier,
                    modification = 9,
                    modifyType = EStatModifyType.Flat
                });

                __instance.inventory.tomeInventory.AddTome(xpTome, xpStatModifiers, ERarity.New);

                for (int i = 0; i < 98; i++)
                {
                    __instance.inventory.tomeInventory.AddTome(xpTome, new Il2CppSystem.Collections.Generic.List<StatModifier>(), ERarity.Legendary);
                }
            }
        }
    }
}