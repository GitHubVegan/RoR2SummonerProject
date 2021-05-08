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

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "nothing yet");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_HOLO_NAME", "Fencer");
            LanguageAPI.Add(prefix + "PRIMARY_HOLO_DESCRIPTION", $"Summon up to three holographic fencers to fight for you. They attack at a rapid pace, dealing heavy single target damage. Can be reactivated to target a specific enemy.");
            LanguageAPI.Add(prefix + "PRIMARY_TARGET_NAME", "Command Fencers");
            LanguageAPI.Add(prefix + "PRIMARY_TARGET_DESCRIPTION", $"Command your active fencers to attack a specific enemy under your crosshairs.");
            LanguageAPI.Add(prefix + "PRIMARY_SHATTER_NAME", "Shatter: Flurry");
            LanguageAPI.Add(prefix + "PRIMARY_SHATTER_DESCRIPTION", $"Your fencers rush at their current target, gaining massive attack speed and dealing increased damage. They die after attacking twice.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_HOLO_NAME", "Eel");
            LanguageAPI.Add(prefix + "SECONDARY_HOLO_DESCRIPTION", $"Summon up to one holographic eel. It deals damage to and distracts all enemies in close proximity. Can be reactivated to target a specific enemy.");
            LanguageAPI.Add(prefix + "SECONDARY_TARGET_NAME", "Command Eel");
            LanguageAPI.Add(prefix + "SECONDARY_TARGET_DESCRIPTION", $"Command your active eel to attack a specific enemy under your crosshairs.");
            LanguageAPI.Add(prefix + "SECONDARY_SHATTER_NAME", "Shatter: Shock");
            LanguageAPI.Add(prefix + "SECONDARY_SHATTER_DESCRIPTION", $"Your eel rushes at their current target, creating a shocking explosion that knocks flying enemies to the ground at the location. The eel dies after it explodes.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_HOLO_NAME", "Drone");
            LanguageAPI.Add(prefix + "UTILITY_HOLO_DESCRIPTION", $"Summon up to one holographic drone. The drone hovers around you and projects a field of energy around it. The energy field cripples enemies, gives armor to allies and slows all projectiles that enter it. Reactivate to target an ally or an enemy. Reactiving without a target returns it to you.");
            LanguageAPI.Add(prefix + "UTILITY_TARGET_NAME", "Command Drone");
            LanguageAPI.Add(prefix + "UTILITY_TARGET_DESCRIPTION", $"Command your drone to orbit an ally or an enemy under your crosshairs. Activate without a target to recall the drone to you.");
            LanguageAPI.Add(prefix + "UTILITY_SHATTER_NAME", "Shatter: Shield");
            LanguageAPI.Add(prefix + "UTILITY_SHATTER_DESCRIPTION", $"Destroy your drone and gain a temporary shield that reflects projectiles and makes you invulnerable.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_HOLO_NAME", "Overcharge");
            LanguageAPI.Add(prefix + "SPECIAL_HOLO_DESCRIPTION", $"Gain access to Shatter skills, which allow you to sacrifice your holograms for powerful effects.");
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