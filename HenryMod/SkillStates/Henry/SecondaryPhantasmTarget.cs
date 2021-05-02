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
    public class SecondaryPhantasmTarget : BaseSkillState
    {
        public static float damageCoefficient = 0f;
        public static float procCoefficient = 0f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 250f;

        private static float d = 7;


        private float duration;
        private bool hasFired;



        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = 0.2f;
            if (base.isAuthority)
            {
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
                        radius = 0.9f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
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

        //private static GameObject CreateBody()
        //{
            //GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/MercBody"), "SecondaryPhantasmTargetBody", true);

            //       BodyInfo bodyInfo = new BodyInfo
            //       {
            //           armor = 20f,
            //           armorGrowth = 0f,
            //           bodyName = "PhantasmSwordBody",
            //           bodyNameToken = HenryPlugin.developerPrefix + "_PHANTASMSWORD_BODY_NAME",
            //           bodyColor = Color.grey,
            //           characterPortrait = Modules.Assets.LoadCharacterIcon("Henry"),
            //           crosshair = Modules.Assets.LoadCrosshair("Standard"),
            //           damage = 12f,
            //          healthGrowth = 33f,
            //          healthRegen = 1.5f,
            //           jumpCount = 1,
            //           maxHealth = 110f,
            //           subtitleNameToken = HenryPlugin.developerPrefix + "_PHANTASMSWORD_BODY_SUBTITLE",
            //           podPrefab = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),
            //           bodyNameToClone = "Merc"
            //       };

            //        GameObject newBody = Prefabs.CreatePrefab("SecondaryPhantasmTargetBody", "mdlPhantasmSword",bodyInfo);
            //       //bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(characterMainState);


         /*   GameObject newBody = null;
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
            GameObject newMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/MercMonsterMaster"), "SecondaryPhantasmTargetMaster", true);
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = SecondaryPhantasmTargetBody;
            foreach (AISkillDriver ai in newMaster.GetComponentsInChildren<AISkillDriver>())
            {
                HenryPlugin.DestroyImmediate(ai);
            }

            newMaster.GetComponent<BaseAI>().fullVision = true;


            AISkillDriver attackDriver = newMaster.AddComponent<AISkillDriver>();
            attackDriver.customName = "Attack";
            attackDriver.movementType = AISkillDriver.MovementType.Stop;
            attackDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            attackDriver.activationRequiresAimConfirmation = true;
            attackDriver.activationRequiresTargetLoS = false;
            attackDriver.selectionRequiresTargetLoS = false;
            attackDriver.maxDistance = 8f;
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
            attackDriver.skillSlot = SkillSlot.Primary;

            AISkillDriver shatterDriver = newMaster.AddComponent<AISkillDriver>();
            shatterDriver.customName = "Shatter";
            shatterDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shatterDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shatterDriver.activationRequiresAimConfirmation = false;
            shatterDriver.activationRequiresTargetLoS = false;
            shatterDriver.selectionRequiresTargetLoS = false;
            shatterDriver.maxDistance = 70f;
            shatterDriver.minDistance = 8f;
            shatterDriver.shouldSprint = false;
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
        }*/

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