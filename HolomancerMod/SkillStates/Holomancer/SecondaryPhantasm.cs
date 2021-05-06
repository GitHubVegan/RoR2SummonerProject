using EntityStates;
using HolomancerMod.Modules;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;


namespace HolomancerMod.SkillStates
{
    public class SecondaryPhantasm : BaseSkillState
    {
        public static float damageCoefficient = 0f;
        public static float procCoefficient = 0f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 250f;
        public static GameObject SecondaryPhantasmBody = CreateBody();
        public static GameObject SecondaryPhantasmMaster = CreateMaster();

        private static float d = 7;
        public static List<CharacterMaster> SummonablesList2 = new List<CharacterMaster>();


        private float duration;
        private bool hasFired;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = 0.2f;
            if (base.isAuthority)
            {
                SecondaryPhantasm.SummonablesList2.RemoveAll(delegate (CharacterMaster C) { return C == null; });
                if (SecondaryPhantasm.SummonablesList2.Count > 0)
                {
                    SecondaryPhantasm.SummonablesList2.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
                }
                if (SecondaryPhantasm.SummonablesList2.Count > 0)
                {
                    CharacterMaster result = SecondaryPhantasm.SummonablesList2.Find(delegate (CharacterMaster C)
                    {
                        return C;
                    }
                        );
                    if (result != null)
                    {
                            result.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;
                        
                    }
                    SecondaryPhantasm.SummonablesList2.RemoveAt(0);


                }
                CharacterMaster characterMaster = new MasterSummon
                {
                    masterPrefab = SecondaryPhantasm.SecondaryPhantasmMaster,
                    position = base.characterBody.transform.position + Vector3.up * 5,
                    rotation = base.characterBody.transform.rotation,
                    summonerBodyObject = base.characterBody.gameObject,
                    ignoreTeamMemberLimit = true,
                    teamIndexOverride = new TeamIndex?(TeamIndex.Player)
                }.Perform();
                characterMaster.GetBody().RecalculateStats();
                characterMaster.GetBody().baseMoveSpeed = 20f;
                characterMaster.GetBody().baseAcceleration = 100f;
                characterMaster.GetBody().baseArmor = 50f;
                characterMaster.GetBody().baseMaxHealth = 120f;
                characterMaster.GetBody().levelMaxHealth = 32f;
                characterMaster.GetBody().baseRegen = 2f;
                characterMaster.inventory.CopyItemsFrom(base.characterBody.inventory);
                characterMaster.inventory.ResetItem(RoR2Content.Items.ExtraLife.itemIndex);
                //characterMaster.inventory.GiveItem(RoR2Content.Items.Ghost.itemIndex);
                //characterMaster.inventory.GiveItem(RoR2Content.Items.HealthDecay.itemIndex, 20);
                characterMaster.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
                characterMaster.GetBody().GetComponent<RoR2.SkillLocator>().primary.SetSkillOverride(4, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("PhantasmGround")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
                characterMaster.GetBody().isPlayerControlled = false;
                SecondaryPhantasm.SummonablesList2.Add(characterMaster);
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
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = null,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = null,
                        hitCallback = SummonSecondary
                    }.Fire();
                }
            }
        }

        bool SummonSecondary(ref BulletAttack.BulletHit hitInfo)
        {
            /*CharacterMaster characterMaster = new MasterSummon
            {
                masterPrefab = SecondaryPhantasmMaster,
                position = hitInfo.point + Vector3.up * d,
                rotation = base.characterBody.transform.rotation,
                summonerBodyObject = base.characterBody.gameObject,
                ignoreTeamMemberLimit = true,
                teamIndexOverride = new TeamIndex?(TeamIndex.Player)
            }.Perform();
            characterMaster.GetBody().RecalculateStats();
            characterMaster.inventory.CopyItemsFrom(base.characterBody.inventory);
            characterMaster.inventory.ResetItem(RoR2Content.Items.ExtraLife.itemIndex);
            characterMaster.inventory.GiveItem(RoR2Content.Items.Ghost.itemIndex);
            characterMaster.inventory.GiveItem(RoR2Content.Items.HealthDecay.itemIndex, 24);
            characterMaster.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
            characterMaster.GetBody().GetComponent<RoR2.SkillLocator>().primary.SetSkillOverride(4, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("PhantasmGround")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
            characterMaster.GetBody().GetComponent<RoR2.SkillLocator>().utility.SetSkillOverride(4, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("DiversionClone")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
            characterMaster.GetBody().isPlayerControlled = false;
            SummonablesList2.Add(characterMaster);*/
            SecondaryPhantasm.SummonablesList2.RemoveAll(delegate (CharacterMaster C) { return C == null; });
            if (SecondaryPhantasm.SummonablesList2.Count > 0)
            {
                SecondaryPhantasm.SummonablesList2.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
            }
            if (SecondaryPhantasm.SummonablesList2.Count > 0)
            {
                if (hitInfo.entityObject != null && hitInfo.hitHurtBox != null && hitInfo.hitHurtBox.teamIndex != TeamIndex.Player)
                {

                    foreach (CharacterMaster cm in SecondaryPhantasm.SummonablesList2)
                    {
                        cm.gameObject.GetComponent<BaseAI>().currentEnemy.gameObject = hitInfo.entityObject;
                    }
                }
                /*else
                {
                    foreach (CharacterMaster cm in SecondaryPhantasm.SummonablesList2)
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
            GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/WispSoulBody"), "SecondaryPhantasmBody", true);
            newBody.GetComponentInChildren<EntityStateMachine>().mainStateType = new SerializableEntityStateType(typeof(SwarmContact));
            Modules.Prefabs.bodyPrefabs.Add(newBody);
            return newBody;
        }

        private static GameObject CreateMaster()
        {
            GameObject newMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/WispMaster"), "SecondaryPhantasmMaster", true);
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = SecondaryPhantasmBody;
            foreach (AISkillDriver ai in newMaster.GetComponentsInChildren<AISkillDriver>())
            {
                HolomancerPlugin.DestroyImmediate(ai);
            }

            newMaster.GetComponent<BaseAI>().fullVision = true;


            AISkillDriver attackDriver = newMaster.AddComponent<AISkillDriver>();
            attackDriver.customName = "Attack";
            attackDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            attackDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            attackDriver.activationRequiresAimConfirmation = true;
            attackDriver.activationRequiresTargetLoS = false;
            attackDriver.selectionRequiresTargetLoS = false;
            attackDriver.maxDistance = 2f;
            attackDriver.minDistance = 0f;
            attackDriver.requireSkillReady = true;
            attackDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            attackDriver.ignoreNodeGraph = true;
            attackDriver.moveInputScale = 1f;
            attackDriver.driverUpdateTimerOverride = 0.2f;
            attackDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            attackDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            attackDriver.maxTargetHealthFraction = Mathf.Infinity;
            attackDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            attackDriver.maxUserHealthFraction = Mathf.Infinity;
            attackDriver.skillSlot = SkillSlot.None;

            AISkillDriver shatterDriver = newMaster.AddComponent<AISkillDriver>();
            shatterDriver.customName = "Shatter";
            shatterDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shatterDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shatterDriver.activationRequiresAimConfirmation = false;
            shatterDriver.activationRequiresTargetLoS = false;
            shatterDriver.selectionRequiresTargetLoS = false;
            shatterDriver.maxDistance = 70f;
            shatterDriver.minDistance = 2f;
            shatterDriver.shouldSprint = true;
            shatterDriver.requireSkillReady = false;
            shatterDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shatterDriver.ignoreNodeGraph = true;
            shatterDriver.moveInputScale = 1f;
            shatterDriver.driverUpdateTimerOverride = 0.2f;
            shatterDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            shatterDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            shatterDriver.maxTargetHealthFraction = Mathf.Infinity;
            shatterDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            shatterDriver.maxUserHealthFraction = Mathf.Infinity;
            shatterDriver.skillSlot = SkillSlot.None;

            Modules.Prefabs.masterPrefabs.Add(newMaster);
            return newMaster;
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