using BepInEx.Configuration;
using HolomancerMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using EntityStates;

namespace HolomancerMod.Modules.Survivors
{
    internal class PhantasmSword : SurvivorBase
    {
        public override string bodyName => "PhantasmSword";

        public const string HENRY_PREFIX = "HoloA";
         public override string survivorTokenPrefix => "HoloA";

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 50f,
            armorGrowth = 0f,
            bodyName = "PhantasmSwordBody",
            bodyNameToken = "Fencer",
            bodyColor = Color.grey,
            characterPortrait = Modules.Assets.mainAssetBundle.LoadAsset<Texture>("PhantasmSword"),
            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            damage = 12f,
            healthGrowth = 30f,
            healthRegen = 0.5f,
            moveSpeed = 8,
            jumpCount = 1,
            maxHealth = 100f,
            acceleration = 80f,
            
            subtitleNameToken = HolomancerPlugin.developerPrefix + "_PHANTASMSWORD_BODY_SUBTITLE",
            podPrefab = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };

        //internal static Material HolomancerMat = Modules.Assets.CreateMaterial("matHolomancer");
        internal static Material phantasmMat = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram");

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = phantasmMat,
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                    material = phantasmMat
                }};

        public override UnlockableDef characterUnlockableDef => null;

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ConfigEntry<bool> characterEnabledConfig => null;

        // item display stuffs
        public override ItemDisplaysBase itemDisplays => null;

        private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
            this.bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(EntityStates.BrotherMonster.InstantDeathState));
        }

        public override void InitializeUnlockables()
        {
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
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
            #region Primary
            /*
            SkillDef mindwrackSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Mindwrack",
                skillNameToken = "Mindwrack",
                skillDescriptionToken = "Mindwrack",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Mindwrack"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Mindwrack)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 2f,
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
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            SkillDef primaryphantasmSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "PrimaryPhantasm",
                skillNameToken = "PrimaryPhantasm",
                skillDescriptionToken = "PrimaryPhantasm",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("SecondaryPhantasm"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PrimaryPhantasm)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 2f,
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
                stockToConsume = 1
            });

            Modules.Skills.AddPrimarySkill(bodyPrefab, primaryphantasmSkillDef);

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
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            SkillDef diversionSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Diversion",
                skillNameToken = "Diversion",
                skillDescriptionToken = "Diversion",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Diversion"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Diversion)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
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

            SkillDef diversioncloneSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "DiversionClone",
                skillNameToken = "DiversionClone",
                skillDescriptionToken = "DiversionClone",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.DiversionClone)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 2f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            SkillDef secondaryphantasmSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SecondaryPhantasm",
                skillNameToken = "SecondaryPhantasm",
                skillDescriptionToken = "SecondaryPhantasm",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("PrimaryPhantasm"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SecondaryPhantasm)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
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
                stockToConsume = 1
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, secondaryphantasmSkillDef);

            #endregion

            #region Utility

            SkillDef distortionSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Distortion",
                skillNameToken = "Distortion",
                skillDescriptionToken = "Distortion",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Distortion"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Distortion)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 0,
                stockToConsume = 0,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            SkillDef utilityphantasmSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "UtilityPhantasm",
                skillNameToken = "UtilityPhantasm",
                skillDescriptionToken = "UtilityPhantasm",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("UtilityPhantasm"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.UtilityPhantasm)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
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
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, utilityphantasmSkillDef);

            #endregion

            #region Special

            SkillDef shatterskillswapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ShatterSkillswap",
                skillNameToken = "ShatterSkillswap",
                skillDescriptionToken = "ShatterSkillswap",
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
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, shatterskillswapSkillDef);

            SkillDef shatterskillswapcancelSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ShatterSkillswapCancel",
                skillNameToken = "ShatterSkillswapCancel",
                skillDescriptionToken = "ShatterSkillswapCancel",
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
                requiredStock = 1,
                stockToConsume = 1
            });*/


            #endregion
        }

        public override void InitializeSkins()
        {
            /*GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
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
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Rapier"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("PhantasmBody"),
                    renderer = defaultRenderers[1].renderer
                }
            };

            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin
            Material masteryMat = Materials.CreateHopooMaterial("matHolomancerAlt");
            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
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
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Rapier"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("PhantasmBody"),
                    renderer = defaultRenderers[1].renderer
                }
            };

            skins.Add(masterySkin);
            #endregion

            skinController.skins = skins.ToArray();*/
        }

        private static CharacterModel.RendererInfo[] SkinRendererInfos(CharacterModel.RendererInfo[] defaultRenderers, Material[] materials)
        {
            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            newRendererInfos[0].defaultMaterial = materials[0];
            newRendererInfos[1].defaultMaterial = materials[1];
            newRendererInfos[2].defaultMaterial = materials[2];

            return newRendererInfos;
        }
    }
}