using EntityStates;
using HenryMod.Modules;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;


namespace HenryMod.SkillStates
{
    public class UtilityPhantasm : BaseSkillState
    {
        public static float damageCoefficient = 0f;
        public static float procCoefficient = 0f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 250f;
        public static GameObject UtilityPhantasmBody = CreateBody();
        public static GameObject UtilityPhantasmMaster = CreateMaster();

        private static float d = 7;
        public static List<CharacterMaster> SummonablesList3 = new List<CharacterMaster>();


        private float duration;
        private bool hasFired;



        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = 0.2f;
            if (base.isAuthority)
            {
                if (UtilityPhantasm.SummonablesList3.Count > 0)
                {

                    foreach (CharacterMaster CM in UtilityPhantasm.SummonablesList3)
                    {
                        CM.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;
                    }
                    UtilityPhantasm.SummonablesList3.Clear();

                }
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
                        hitCallback = SummonUtility
                    }.Fire();
                }
            }
        }

        bool SummonUtility(ref BulletAttack.BulletHit hitInfo)
        {
            CharacterMaster characterMaster = new MasterSummon
            {
                masterPrefab = UtilityPhantasmMaster,
                position = hitInfo.point + Vector3.up * d,
                rotation = base.characterBody.transform.rotation,
                //summonerBodyObject = base.characterBody.gameObject,
                ignoreTeamMemberLimit = false,
                teamIndexOverride = new TeamIndex?(TeamIndex.Player)
            }.Perform();
            characterMaster.GetBody().RecalculateStats();
            characterMaster.inventory.CopyItemsFrom(base.characterBody.inventory);
            characterMaster.inventory.ResetItem(RoR2Content.Items.ExtraLife.itemIndex);
            characterMaster.inventory.GiveItem(RoR2Content.Items.Ghost.itemIndex);
            characterMaster.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
            SummonablesList3.Add(characterMaster);
            return false;
            
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }

        private static GameObject CreateBody()
        {
            GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/MageBody"), "UtilityPhantasmBody", true);
            newBody.GetComponent<EntityStateMachine>().mainStateType = new SerializableEntityStateType(typeof(WardMain));

            Debug.Log(newBody.GetComponent<CharacterDeathBehavior>().deathState);
            Debug.Log(newBody.GetComponent<CharacterDeathBehavior>().deathStateMachine);
            Debug.Log(newBody.GetComponent<CharacterDeathBehavior>().idleStateMachine);

            Modules.Prefabs.bodyPrefabs.Add(newBody);
            return newBody;
        }

        private static GameObject CreateMaster()
        {
            GameObject newMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/MageMonsterMaster"), "UtilityPhantasmMaster", true);
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = UtilityPhantasmBody;
            foreach (AISkillDriver ai in newMaster.GetComponentsInChildren<AISkillDriver>())
            {
                HenryPlugin.DestroyImmediate(ai);
            }

            newMaster.GetComponent<BaseAI>().fullVision = true;


            AISkillDriver attackDriver = newMaster.AddComponent<AISkillDriver>();
            attackDriver.customName = "Attack";
            attackDriver.movementType = AISkillDriver.MovementType.Stop;
            attackDriver.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            attackDriver.activationRequiresAimConfirmation = false;
            attackDriver.activationRequiresTargetLoS = false;
            attackDriver.selectionRequiresTargetLoS = false;
            attackDriver.maxDistance = 20f;
            attackDriver.minDistance = 0f;
            attackDriver.requireSkillReady = false;
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
            shatterDriver.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            shatterDriver.activationRequiresAimConfirmation = false;
            shatterDriver.activationRequiresTargetLoS = false;
            shatterDriver.selectionRequiresTargetLoS = false;
            shatterDriver.maxDistance = 100f;
            shatterDriver.minDistance = 20f;
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