using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using BepInEx.Configuration;
using HolomancerMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using HolomancerMod.SkillStates;

namespace HolomancerMod.Modules.Survivors
{
    internal class HoloCharacter : SurvivorBase
    {

        public override string bodyName => "Archangel";

        public override string survivorTokenPrefix => "Holomancer";

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 0f,
            armorGrowth = 0f,
            bodyName = "HolomancerBody",
            bodyNameToken = "Holomancer",
            bodyColor = Color.grey,
            characterPortrait = Modules.Assets.LoadCharacterIconGeneric("Holomancer"),
            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            damage = 13f,
            healthGrowth = 25f,
            healthRegen = 0.5f,
            moveSpeed = 6,
            jumpCount = 1,
            maxHealth = 90f,
            acceleration = 80f, 
            subtitleNameToken = "Holomancer",
            podPrefab = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };

        public static Material HolomancerMat = Materials.CreateHopooMaterial("matHolomancer");


        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] {
                new CustomRendererInfo
                {
                    childName = "SpearModel",
                    material = HolomancerMat,
                },
                new CustomRendererInfo
                {
                    childName = "HoloModel",
                    material = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram")
                },
                new CustomRendererInfo
                {
                    childName = "ClothesModel",
                    material = HolomancerMat
                }};

        public override UnlockableDef characterUnlockableDef => null;

        public override Type characterMainState => typeof(SkillStates.Maintest);

        // item display stuffs
        public override ItemDisplaysBase itemDisplays => null;

        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        public override void InitializeUnlockables()
        {
            masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
        }



        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            #region Primary
            SkillDef mindwrackSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Mindwrack",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_PRIMARY_SHATTER_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_PRIMARY_SHATTER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Mindwrack"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Mindwrack)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 3.5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0
            });

            SkillDef primarytargetSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "PrimaryPhantasmTarget",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_PRIMARY_TARGET_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_PRIMARY_TARGET_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("PrimaryTarget"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PrimaryPhantasmTarget)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 3.5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0
            });

            SkillDef rapierSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "PhantasmRapier",
                skillNameToken = "PhantasmRapier",
                skillDescriptionToken = "PhantasmRapier",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PhantasmRapier)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 2f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            SkillDef flurryCloneSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Flurry",
                skillNameToken = "Flurry",
                skillDescriptionToken = "Flurry",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Flurry)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            SkillDef groundSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "PhantasmGround",
                skillNameToken = "PhantasmGround",
                skillDescriptionToken = "PhantasmGround",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PhantasmGround)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 2f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            SkillDef primaryphantasmSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_PRIMARY_HOLO_NAME",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_PRIMARY_HOLO_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_PRIMARY_HOLO_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("SecondaryPhantasm"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PrimaryPhantasm)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 3.5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                dontAllowPastMaxStocks = false,


                keywordTokens = new string[] { "KEYWORD_FLURRYSHATTER", "KEYWORD_FENCERCOM" }

            });

            SkillDef explosiondashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ExplosionDash",
                skillNameToken = "ExplosionDash",
                skillDescriptionToken = "ExplosionDash",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ExplosionDash)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddPrimarySkills(bodyPrefab, primaryphantasmSkillDef);

            #endregion

            #region Secondary

            SkillDef mindwrackCloneSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "MindwrackClone",
                skillNameToken = "MindwrackClone",
                skillDescriptionToken = "MindwrackClone",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.MindwrackClone)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0
            });

            SkillDef cannonSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CannonLaunch",
                skillNameToken = "CannonLaunch",
                skillDescriptionToken = "CannonLaunch",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.CannonLaunch)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 1
            });

            SkillDef secondarytargetSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SecondaryPhantasmTarget",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SECONDARY_TARGET_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SECONDARY_TARGET_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("SecondaryTarget"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SecondaryPhantasmTarget)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0,
            });

            SkillDef diversionSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Diversion",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SECONDARY_SHATTER_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SECONDARY_SHATTER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Diversion"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Diversion)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0,
            });

            SkillDef diversioncloneSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "DiversionClone",
                skillNameToken = "DiversionClone",
                skillDescriptionToken = "DiversionClone",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(HolomancerMod.SkillStates.DiversionClone)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0
            });

            SkillDef gravitySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GravityWell",
                skillNameToken = "GravityWell",
                skillDescriptionToken = "GravityWell",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.GravityWell)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0
            });

            SkillDef secondaryphantasmSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SECONDARY_HOLO_NAME",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SECONDARY_HOLO_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SECONDARY_HOLO_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("PrimaryPhantasm"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SecondaryPhantasm)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                keywordTokens = new string[] { "KEYWORD_SHOCKSHATTER", "KEYWORD_EELCOM" } 
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, secondaryphantasmSkillDef);

            #endregion

            #region Utility

            SkillDef distortionSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Distortion",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_UTILITY_SHATTER_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_UTILITY_SHATTER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Distortion"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Distortion)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 20f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0,
            });
            

            SkillDef utilitytargetSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "UtilityPhantasmTarget",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_UTILITY_TARGET_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_UTILITY_TARGET_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("UtilityTarget"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.UtilityPhantasmTarget)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 20f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0
                
            });

            SkillDef utilityphantasmSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_UTILITY_HOLO_NAME",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_UTILITY_HOLO_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_UTILITY_HOLO_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("UtilityPhantasm"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.UtilityPhantasm)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 20f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                keywordTokens = new string[] { "KEYWORD_SHIELDSHATTER", "KEYWORD_DRONECOM", "KEYWORD_WEAKEN" }
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, utilityphantasmSkillDef);

            #endregion

            #region Special

            SkillDef shatterskillswapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SPECIAL_HOLO_NAME",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SPECIAL_HOLO_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SPECIAL_HOLO_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ShatterSkillswap"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ShatterSkillswap)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0,

                keywordTokens = new string[] { "KEYWORD_FLURRYSHATTER", "KEYWORD_SHOCKSHATTER",  "KEYWORD_SHIELDSHATTER" }
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, shatterskillswapSkillDef);

            SkillDef shatterskillswapcancelSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ShatterSkillswapCancel",
                skillNameToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SPECIAL_HOLOCANCEL_NAME",
                skillDescriptionToken = HolomancerPlugin.developerPrefix + "_HOLOMANCER_BODY_SPECIAL_HOLOCANCEL_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ShatterSkillswapCancel"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ShatterSkillswapCancel)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0
            });

            
            #endregion
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(HolomancerPlugin.developerPrefix + "_Holomancer_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Staff"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("HolomancerBody"),
                    //mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("HolomancerClothes"),
                    renderer = defaultRenderers[1].renderer
                },
                                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("HolomancerClothes"),
                    //mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("HolomancerBody"),
                    renderer = defaultRenderers[2].renderer
                }
            };

            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin
            /*
            Material masteryMat = Materials.CreateHopooMaterial("matHolomancerAlt");
            CharacterModel.RendererInfo[] masteryRendererInfos = Skinrend(defaultRenderers, new Material[]
            {
                masteryMat,
                masteryMat,
                masteryMat,
                masteryMat
            });

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(HolomancerPlugin.developerPrefix + "_Holomancer_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                masteryRendererInfos,
                mainRenderer,
                model,
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Staff"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("HolomancerBody"),
                    //mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("HolomancerClothes"),
                    renderer = defaultRenderers[1].renderer
                },
                                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("HolomancerClothes"),
                    //mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("HolomancerBody"),
                    renderer = defaultRenderers[instance.mainRendererIndex].renderer
                }
            };

            skins.Add(masterySkin);*/
            #endregion

            skinController.skins = skins.ToArray();
        }

        /*public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(HenryPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                //place your mesh replacements here
                //unnecessary if you don't have multiple skins
                //new SkinDef.MeshReplacement
                //{
                //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenrySword"),
                //    renderer = defaultRenderers[0].renderer
                //},
                //new SkinDef.MeshReplacement
                //{
                //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenryGun"),
                //    renderer = defaultRenderers[1].renderer
                //},
                //new SkinDef.MeshReplacement
                //{
                //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenry"),
                //    renderer = defaultRenderers[2].renderer
                //}
            };

            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            Material masteryMat = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                masteryMat,
                masteryMat,
                masteryMat,
                masteryMat
            });

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(HenryPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                masteryRendererInfos,
                mainRenderer,
                model,
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenrySwordAlt"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenryAlt"),
                    renderer = defaultRenderers[2].renderer
                }
            };

            skins.Add(masterySkin);
            
            #endregion

            skinController.skins = skins.ToArray();
        }*/
    }
}