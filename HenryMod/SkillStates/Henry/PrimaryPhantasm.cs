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
    public class PrimaryPhantasm : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/ExplosionGreaterWisp");
        public static GameObject GreaterSummonBody = CreateBody();
        public static GameObject GreaterSummonMaster = CreateMaster();

        private static float d = 7;
        public static List<CharacterMaster> SummonablesList1 = new List<CharacterMaster>();


        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private BullseyeSearch search;
        private TeamIndex team;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Shoot.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";
            this.search = new BullseyeSearch();
            this.team = base.GetTeam();
            

            base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }


        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(0f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound("HenryShootPistol", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    

                    new BulletAttack
                    {

                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = Shoot.damageCoefficient * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = Shoot.range,
                        force = Shoot.force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = base.RollCrit(),
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = Shoot.tracerEffectPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = PrimaryPhantasm.hitEffectPrefab,
                        hitCallback = SummonBigWisp
                    }.Fire();
                }
            }
        }

        bool SummonBigWisp(ref BulletAttack.BulletHit hitInfo)
        {
            CharacterMaster characterMaster = new MasterSummon
            {
                masterPrefab = GreaterSummonMaster,
                position = hitInfo.point + Vector3.up * d,
                rotation = base.characterBody.transform.rotation,
                //summonerBodyObject = base.characterBody.gameObject,
                ignoreTeamMemberLimit = false,
                teamIndexOverride = new TeamIndex?(TeamIndex.Player)
            }.Perform();
            characterMaster.GetBody().RecalculateStats();
            characterMaster.inventory.CopyItemsFrom(base.characterBody.inventory);
            characterMaster.inventory.ResetItem(RoR2Content.Items.ExtraLife.itemIndex);
            characterMaster.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
            characterMaster.GetBody().GetComponent<RoR2.SkillLocator>().secondary.SetSkillOverride(characterMaster.GetBody(), SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("MindwrackClone")), RoR2.GenericSkill.SkillOverridePriority.Replacement);
            characterMaster.GetBody().GetComponent<RoR2.SkillLocator>().utility.SetSkillOverride(characterMaster.GetBody(), SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("DiversionClone")), RoR2.GenericSkill.SkillOverridePriority.Replacement);
            characterMaster.GetBody().GetComponent<RoR2.SkillLocator>().special.SetSkillOverride(characterMaster.GetBody(), SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("DistortionClone")), RoR2.GenericSkill.SkillOverridePriority.Replacement);
            SummonablesList1.Add(characterMaster);
            //characterMaster.GetBody().GetComponent<CharacterDeathBehavior>().deathState = Resources.Load<GameObject>("prefabs/characterbodies/GreaterWispBody").GetComponentInChildren<CharacterDeathBehavior>().deathState;
            //only works if prefab is original GreaterWispBody, NullifierBody for example just makes it disappear
            return false;
            
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }

        private static GameObject CreateBody()
        {
            GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody"), "GreaterSummonBody", true);
            

            //newBody.GetComponent<CharacterBody>().baseAcceleration = 50;
            //newBody.GetComponent<CharacterDeathBehavior>().deathState = Resources.Load<GameObject>("prefabs/characterbodies/GreaterWispBody").GetComponentInChildren<CharacterDeathBehavior>().deathState;
            //doesn't load above deathstate at all, wisp body just disappears
            //newBody.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.NullifierMonster.DeathState));
            //same issue
            //newBody.GetComponent<CharacterDeathBehavior>().deathStateMachine = Resources.Load<GameObject>("prefabs/characterbodies/GreaterWispBody").GetComponent<CharacterDeathBehavior>().deathStateMachine;
            //newBody.GetComponent<CharacterDeathBehavior>().idleStateMachine = Resources.Load<GameObject>("prefabs/characterbodies/GreaterWispBody").GetComponent<CharacterDeathBehavior>().idleStateMachine;
            Debug.Log(newBody.GetComponent<CharacterDeathBehavior>().deathState);
            Debug.Log(newBody.GetComponent<CharacterDeathBehavior>().deathStateMachine);
            Debug.Log(newBody.GetComponent<CharacterDeathBehavior>().idleStateMachine);

            Modules.Prefabs.bodyPrefabs.Add(newBody);
            return newBody;
        }

        private static GameObject CreateMaster()
        {
            GameObject newMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/CommandoMonsterMaster"), "GreaterSummonMaster", true);
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = GreaterSummonBody;
            foreach (AISkillDriver ai in newMaster.GetComponentsInChildren<AISkillDriver>())
            {
                HenryPlugin.DestroyImmediate(ai);
            }

            newMaster.GetComponent<BaseAI>().fullVision = true;


            AISkillDriver attackDriver = newMaster.AddComponent<AISkillDriver>();
            attackDriver.customName = "Attack";
            attackDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            attackDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            attackDriver.activationRequiresAimConfirmation = false;
            attackDriver.activationRequiresTargetLoS = false;
            attackDriver.selectionRequiresTargetLoS = false;
            attackDriver.maxDistance = 100f;
            attackDriver.minDistance = 0f;
            attackDriver.requireSkillReady = false;
            attackDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            attackDriver.ignoreNodeGraph = true;
            attackDriver.moveInputScale = 1f;
            attackDriver.driverUpdateTimerOverride = 1f;
            attackDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            attackDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            attackDriver.maxTargetHealthFraction = Mathf.Infinity;
            attackDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            attackDriver.maxUserHealthFraction = Mathf.Infinity;
            attackDriver.skillSlot = SkillSlot.Primary;

            AISkillDriver shatterDriver = newMaster.AddComponent<AISkillDriver>();
            shatterDriver.customName = "Shatter";
            shatterDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            shatterDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shatterDriver.activationRequiresAimConfirmation = false;
            shatterDriver.activationRequiresTargetLoS = false;
            shatterDriver.selectionRequiresTargetLoS = false;
            shatterDriver.maxDistance = 100f;
            shatterDriver.minDistance = 0f;
            shatterDriver.requireSkillReady = false;
            shatterDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shatterDriver.ignoreNodeGraph = true;
            shatterDriver.moveInputScale = 1f;
            shatterDriver.driverUpdateTimerOverride = 1f;
            shatterDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            shatterDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            shatterDriver.maxTargetHealthFraction = Mathf.Infinity;
            shatterDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            shatterDriver.maxUserHealthFraction = Mathf.Infinity;
            shatterDriver.skillSlot = SkillSlot.Primary;

            Modules.Prefabs.masterPrefabs.Add(newMaster);
            return newMaster;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }

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