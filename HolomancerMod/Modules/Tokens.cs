using R2API;
using System;

namespace HolomancerMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Holomancer
            string prefix = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_";

            string desc = "The Holomancer is a summoner who never participates in the fight directly. She instead uses her hard-light holograms to distract and defeat her enemies." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Your holograms can be shattered using Overcharge, sacrificing them to unleashing powerful effects. Make sure to pay attention to your cooldowns, so you aren't left defenseless." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Fencers deals heavy single target damage and can be shattered to deal an even heavier burst of damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Swarm can take out smaller flying enemies on its own or it can be shattered to drop them to the ground for your Fencers" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Drone is a powerful defensive tool that slows incoming projectiles while weakening enemies and strenghtening allies. In a pinch, it can be shattered for temporary invulnerability." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so she left, even though she was never really there.";
            string outroFailure = "..and so her hologram vanished, replaced without a second thought.";

            LanguageAPI.Add(prefix + "NAME", "Holomancer");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Holomancer");
            LanguageAPI.Add(prefix + "LORE", "she makes holograms and stuff idk it's an alpha");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);  

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Keywords
            LanguageAPI.Add("KEYWORD_FLURRYSHATTER", $"<color=#db8aaf><style=cKeywordName>Shatter: Flurry</style></color><style=cSub> Your fencers rush at their current target, gaining <style=cIsUtility>massively increased attack speed and damage</style>. Damage increased for each fencer <style=cIsHealing>alive</style>. They <style=cIsDamage>die</style> after attacking twice.");
            LanguageAPI.Add("KEYWORD_SHOCKSHATTER", $"<color=#98509f><style=cKeywordName>Shatter: Shock</style><style=cSub>Your eel rushes at its current target, creating a shocking explosion that deals <style=cIsDamage>600% damage</style> and <style=cIsUtility>knocks flying enemies to the ground.</style> The eel <style=cIsDamage>dies</style> after it explodes.");
            LanguageAPI.Add("KEYWORD_SHIELDSHATTER", $"<color=#35bbe2><style=cKeywordName>Shatter: Shield</style></color><style=cSub> <style=cIsDamage>Destroy</style> your drone and gain a shield for <style=cIsUtility>2.5s. The shield reflects projectiles and grants invulnerability.</style>");
            LanguageAPI.Add("KEYWORD_WEAKEN", $"<color=#90a123><style=cKeywordName>Weak</style></color><style=cSub> Reduce the armor of enemies by <style=cIsDamage>30</style> and their attack and movement speed by <style=cIsDamage>40%</style>");

            LanguageAPI.Add("KEYWORD_DRONECOM", $"<color=#ff8400><style=cKeywordName>Command Drone</style></color><style=cSub>Command your drone to orbit <style=cIsUtility>an ally or an enemy under your crosshairs</style>. Activate without a target to <style=cIsUtility>recall</style> the drone back to you.");
            LanguageAPI.Add("KEYWORD_EELCOM", $"<color=#ff8400><style=cKeywordName>Command Eel</style></color><style=cSub>Command your active eel to attack a <style=cIsUtility>specific enemy under your crosshairs.</style>");
            LanguageAPI.Add("KEYWORD_FENCERCOM", $"<color=#ff8400><style=cKeywordName>Command Fencers</style></color><style=cSub>Command your active fencers to attack a <style=cIsUtility>specific enemy under your crosshairs.</style>");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "nothing yet");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_HOLO_NAME", "Fencer");
            LanguageAPI.Add(prefix + "PRIMARY_HOLO_DESCRIPTION", $"Summon up to three holographic fencers to fight for you. They deal <style=cIsDamage>100% damage per strike</style>. The number of strikes increases with attack speed. Reactivate to <color=#ff8400>Retarget</color>");
            LanguageAPI.Add(prefix + "PRIMARY_TARGET_NAME", "Command Fencers");
            LanguageAPI.Add(prefix + "PRIMARY_TARGET_DESCRIPTION", $"Command your active fencers to attack a <style=cIsUtility>specific enemy under your crosshairs.</style>");
            LanguageAPI.Add(prefix + "PRIMARY_SHATTER_NAME", "Shatter: Flurry");
            LanguageAPI.Add(prefix + "PRIMARY_SHATTER_DESCRIPTION", $"Your fencers rush at their current target, gaining <style=cIsUtility>massively increased attack speed and damage</style>. Damage increased for each fencer <style=cIsHealing>alive</style>. They <style=cIsDamage>die</style> after attacking twice.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_HOLO_NAME", "Eel");
            LanguageAPI.Add(prefix + "SECONDARY_HOLO_DESCRIPTION", $"Summon up to one holographic eel. It deals <style=cIsDamage>280% damage per second</style> to all enemies near it. Reactivate to <color=#ff8400>Retarget</color>");
            LanguageAPI.Add(prefix + "SECONDARY_TARGET_NAME", "Command Eel");
            LanguageAPI.Add(prefix + "SECONDARY_TARGET_DESCRIPTION", $"Command your active eel to attack a <style=cIsUtility>specific enemy under your crosshairs.</style>");
            LanguageAPI.Add(prefix + "SECONDARY_SHATTER_NAME", "Shatter: Shock");
            LanguageAPI.Add(prefix + "SECONDARY_SHATTER_DESCRIPTION", $"Your eel rushes at its current target, creating a shocking explosion that deals <style=cIsDamage>640% damage</style> and <style=cIsUtility>knocks flying enemies to the ground.</style> The eel <style=cIsDamage>dies</style> after it explodes.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_HOLO_NAME", "Drone");
            LanguageAPI.Add(prefix + "UTILITY_HOLO_DESCRIPTION", $"Summon up to one holographic drone. The drone projects a field of energy around it which <style=cIsUtility>slows all projectiles</style>, <style=cIsDamage>Weakens</style> enemies and <style=cIsHealing>grants armor to allies</style>. Reactivate to <color=#ff8400>Retarget</color>");
            LanguageAPI.Add(prefix + "UTILITY_TARGET_NAME", "Command Drone");
            LanguageAPI.Add(prefix + "UTILITY_TARGET_DESCRIPTION", $"Command your drone to orbit <style=cIsUtility>an ally or an enemy under your crosshairs</style>. Activate without a target to <style=cIsUtility>recall</style> the drone back to you.");
            LanguageAPI.Add(prefix + "UTILITY_SHATTER_NAME", "Shatter: Shield");
            LanguageAPI.Add(prefix + "UTILITY_SHATTER_DESCRIPTION", $"<style=cIsDamage>Destroy</style> your drone and gain a shield for <style=cIsUtility>2.5s. The shield reflects projectiles and grants invulnerability.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_HOLO_NAME", "Overcharge");
            LanguageAPI.Add(prefix + "SPECIAL_HOLO_DESCRIPTION", $"Gain access to <color=#b00096>Shatter skills</color>, which allow you to sacrifice your holograms for powerful effects.");
            LanguageAPI.Add(prefix + "SPECIAL_HOLOCANCEL_NAME", "Cancel Overcharge");
            LanguageAPI.Add(prefix + "SPECIAL_HOLOCANCEL_DESCRIPTION", $"Return to your normal skills without shattering any holograms.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Holomancer: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Holomancer, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Holomancer: Mastery");
            #endregion
            #endregion
        }
    }
}