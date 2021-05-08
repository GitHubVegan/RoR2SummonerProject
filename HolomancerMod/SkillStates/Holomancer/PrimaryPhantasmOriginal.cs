﻿using EntityStates;
using HolomancerMod.Modules;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;


namespace HolomancerMod.SkillStates
{
    public class PrimaryPhantasmOriginal : BaseSkillState
    {
        public static float damageCoefficient = 0f;
        public static float procCoefficient = 0f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 250f;
        public static GameObject PrimaryPhantasmOriginalBody = CreateBody();
        public static GameObject PrimaryPhantasmOriginalMaster = CreateMaster();

        private static float d = 7;
        public static List<CharacterMaster> SummonablesList1 = new List<CharacterMaster>();
        public static List<CharacterMaster> KillList1 = new List<CharacterMaster>();


        private float duration;
        private bool hasFired;



        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = 0.2f;
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                PrimaryPhantasmOriginal.SummonablesList1.RemoveAll(delegate (CharacterMaster C) { return C == null; });
                if (PrimaryPhantasmOriginal.SummonablesList1.Count > 0)
                {
                    PrimaryPhantasmOriginal.SummonablesList1.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
                    }
                if (PrimaryPhantasmOriginal.SummonablesList1.Count > 2)
                {
                    CharacterMaster result = PrimaryPhantasmOriginal.SummonablesList1.Find(delegate (CharacterMaster C)
                        {
                            return C;
                        }
                        );
                    if (result != null)
                    {
                        result.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;
                        
                    }
                    
                    PrimaryPhantasmOriginal.SummonablesList1.RemoveAt(0);
                }
                CharacterMaster characterMaster = new MasterSummon
                {
                    masterPrefab = PrimaryPhantasmOriginalMaster,
                    position = base.characterBody.transform.position + aimRay.direction * 12,
                    rotation = base.characterBody.transform.rotation,
                    summonerBodyObject = base.characterBody.gameObject,
                    ignoreTeamMemberLimit = true,
                    teamIndexOverride = new TeamIndex?(TeamIndex.Player)
                }.Perform();
                characterMaster.GetBody().RecalculateStats();
                characterMaster.GetBody().baseDamage = characterMaster.GetBody().baseDamage * 0.3f;
                characterMaster.GetBody().levelDamage = characterMaster.GetBody().levelDamage * 0.3f;
                characterMaster.GetBody().baseMoveSpeed = 20f;
                characterMaster.GetBody().baseAcceleration = 100f;
                characterMaster.inventory.CopyItemsFrom(base.characterBody.inventory);
                characterMaster.inventory.ResetItem(RoR2Content.Items.ExtraLife.itemIndex);
                characterMaster.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
                characterMaster.GetBody().GetComponent<RoR2.SkillLocator>().primary.SetSkillOverride(2, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("PhantasmRapier")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
                //characterMaster.GetBody().GetComponent<RoR2.SkillLocator>().utility.SetSkillOverride(2, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("MindwrackClone")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
                characterMaster.GetBody().isPlayerControlled = false;
                SummonablesList1.Add(characterMaster);
                this.Fire();
            }
        }


        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlaySound("Roll.dodgeSoundString", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    new BulletAttack
                    {

                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damageCoefficient * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = range,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = base.RollCrit(),
                        owner = base.gameObject,
                        muzzleName = null,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = 1f,
                        sniper = false,
                        stopperMask = LayerIndex.enemyBody.mask,
                        weapon = null,
                        tracerEffectPrefab = null,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = null,
                        hitCallback = SummonPrimary
                    }.Fire();
                }
            }
        }

        bool SummonPrimary(ref BulletAttack.BulletHit hitInfo)
        {
            PrimaryPhantasmOriginal.SummonablesList1.RemoveAll(delegate (CharacterMaster C) { return C == null; });
            if (PrimaryPhantasmOriginal.SummonablesList1.Count > 0)
            {
                PrimaryPhantasmOriginal.SummonablesList1.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
            }
            if (PrimaryPhantasmOriginal.SummonablesList1.Count > 0)
            {
                bool flag = (hitInfo.entityObject != null && hitInfo.hitHurtBox != null && hitInfo.hitHurtBox.teamIndex != TeamIndex.Player);
                if (flag)
                {

                    foreach (CharacterMaster cm in PrimaryPhantasmOriginal.SummonablesList1)
                    {
                        cm.gameObject.GetComponent<BaseAI>().leader.gameObject = hitInfo.entityObject;

                    }
                }
                else
                {
                    foreach (CharacterMaster cm in PrimaryPhantasmOriginal.SummonablesList1)
                    {
                        cm.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
                    }
                }
                /*if(!flag)
                {
                    foreach (CharacterMaster cm in PrimaryPhantasmOriginal.SummonablesList1)
                    {
                        cm.gameObject.GetComponentInChildren<AISkillDriver>().moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                    }
                }*/
            }
            return false;
            
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }

        private static GameObject CreateBody()
        {
            //GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/MercBody"), "PrimaryPhantasmOriginalBody", true);

            //       BodyInfo bodyInfo = new BodyInfo
            //       {
            //           armor = 20f,
            //           armorGrowth = 0f,
            //           bodyName = "PhantasmSwordBody",
            //           bodyNameToken = HolomancerPlugin.developerPrefix + "_PHANTASMSWORD_BODY_NAME",
            //           bodyColor = Color.grey,
            //           characterPortrait = Modules.Assets.LoadCharacterIcon("Holomancer"),
            //           crosshair = Modules.Assets.LoadCrosshair("Standard"),
            //           damage = 12f,
            //          healthGrowth = 33f,
            //          healthRegen = 1.5f,
            //           jumpCount = 1,
            //           maxHealth = 110f,
            //           subtitleNameToken = HolomancerPlugin.developerPrefix + "_PHANTASMSWORD_BODY_SUBTITLE",
            //           podPrefab = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),
            //           bodyNameToClone = "Merc"
            //       };

            //        GameObject newBody = Prefabs.CreatePrefab("PrimaryPhantasmOriginalBody", "mdlPhantasmSword",bodyInfo);
            //       //bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(characterMainState);


            GameObject newBody = null;
            foreach (GameObject customCharacterbody in Prefabs.bodyPrefabs)
            {
                Debug.Log($"bodyPrefabs contains GameObject {customCharacterbody.name}");
                if (customCharacterbody.name == "PhantasmSwordBody")
                {
                    newBody = customCharacterbody;

                }
            }


           // Modules.Prefabs.bodyPrefabs.Add(newBody);
            return newBody;
        }



        private static GameObject CreateMaster()
        {
            GameObject newMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/MercMonsterMaster"), "PrimaryPhantasmOriginalMaster", true);
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = PrimaryPhantasmOriginalBody;
            foreach (AISkillDriver ai in newMaster.GetComponentsInChildren<AISkillDriver>())
            {
                HolomancerPlugin.DestroyImmediate(ai);
            }

            newMaster.GetComponent<BaseAI>().fullVision = true;


            AISkillDriver attackDriver = newMaster.AddComponent<AISkillDriver>();
            attackDriver.customName = "Attack";
            attackDriver.movementType = AISkillDriver.MovementType.Stop;
            attackDriver.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            attackDriver.activationRequiresAimConfirmation = true;
            attackDriver.activationRequiresTargetLoS = false;
            attackDriver.selectionRequiresTargetLoS = false;
            attackDriver.maxDistance = 6f;
            attackDriver.minDistance = 0f;
            attackDriver.requireSkillReady = true;
            attackDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            attackDriver.ignoreNodeGraph = false;
            attackDriver.moveInputScale = 1f;
            attackDriver.driverUpdateTimerOverride = 0.2f;
            attackDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            attackDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            attackDriver.maxTargetHealthFraction = Mathf.Infinity;
            attackDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            attackDriver.maxUserHealthFraction = Mathf.Infinity;
            attackDriver.resetCurrentEnemyOnNextDriverSelection = false;
            attackDriver.skillSlot = SkillSlot.None;

            AISkillDriver shatterDriver = newMaster.AddComponent<AISkillDriver>();
            shatterDriver.customName = "Shatter";
            shatterDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shatterDriver.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            shatterDriver.activationRequiresAimConfirmation = false;
            shatterDriver.activationRequiresTargetLoS = false;
            shatterDriver.selectionRequiresTargetLoS = false;
            shatterDriver.maxDistance = Mathf.Infinity;
            shatterDriver.minDistance = 6f;
            shatterDriver.shouldSprint = false;
            shatterDriver.requireSkillReady = false;
            shatterDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shatterDriver.ignoreNodeGraph = false;
            shatterDriver.moveInputScale = 1f;
            shatterDriver.driverUpdateTimerOverride = 0.2f;
            shatterDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            shatterDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            shatterDriver.maxTargetHealthFraction = Mathf.Infinity;
            shatterDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            shatterDriver.maxUserHealthFraction = Mathf.Infinity;
            shatterDriver.resetCurrentEnemyOnNextDriverSelection = false;
            shatterDriver.skillSlot = SkillSlot.None;

            Modules.Prefabs.masterPrefabs.Add(newMaster);
            return newMaster;
        }

        private void FindOwner()
        {
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}