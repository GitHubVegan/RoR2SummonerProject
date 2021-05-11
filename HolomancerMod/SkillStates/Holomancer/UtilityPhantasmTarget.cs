using EntityStates;
using HolomancerMod.Modules;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace HolomancerMod.SkillStates
{
    public class UtilityPhantasmTarget : BaseSkillState
    {
        public static float damageCoefficient = 0f;
        public static float procCoefficient = 0f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 250f;
        public Vector3 point;


        private float duration;



        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = 0.1f;
            if (base.isAuthority)
            {
                this.Fire();
            }
        }


        private void Fire()
        {
            RaycastHit raycastHit;
            if (base.inputBank.GetAimRaycast(100f, out raycastHit))
            {
                this.point = raycastHit.point;
            }
            else
            {
                this.point = base.inputBank.GetAimRay().GetPoint(100f);
            }
            UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster C) { return C == null; });
            if (UtilityPhantasm.SummonablesList3.Count > 0)
            {
                UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
            }
            if (UtilityPhantasm.SummonablesList3.Count > 0)
            {
                HurtBox target = this.SearchForTarget();

                foreach (CharacterMaster cm in UtilityPhantasm.SummonablesList3)
                {
                    if (target && target.healthComponent)
                    {
                        cm.gameObject.GetComponent<BaseAI>().leader.gameObject = target.healthComponent.gameObject;
                    }
                    else
                    {
                        cm.gameObject.GetComponent<BaseAI>().leader.gameObject = base.gameObject;
                    }
                    if (Vector3.Distance(cm.GetBody().transform.position, cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position) > 30f)
                    {
                        EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/ImpBlinkEffect"), new EffectData
                        {
                            origin = cm.GetBody().transform.position,
                            scale = 1f
                        }, true);
                        EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/ImpBlinkEffect"), new EffectData
                        {
                            origin = cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position + (cm.GetBody().transform.position - cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position).normalized * 20,
                            scale = 1f
                        }, true);
                        cm.GetBody().rigidbody.position = cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position + (cm.GetBody().transform.position - cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position).normalized * 20;
                        




                    }
                }
            }



        }



        private HurtBox SearchForTarget()
        {
            BullseyeSearch bullseyeSearch = new BullseyeSearch
            {
                searchOrigin = this.point,
                maxDistanceFilter = 15f,
                teamMaskFilter = TeamMask.GetUnprotectedTeams(this.GetTeam()),
                sortMode = BullseyeSearch.SortMode.Distance
            };
            bullseyeSearch.RefreshCandidates();
            bullseyeSearch.FilterOutGameObject(this.gameObject);
            return bullseyeSearch.GetResults().FirstOrDefault<HurtBox>();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        //private static GameObject CreateBody()
        //{
            //GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/MercBody"), "UtilityPhantasmTargetBody", true);

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

            //        GameObject newBody = Prefabs.CreatePrefab("UtilityPhantasmTargetBody", "mdlPhantasmSword",bodyInfo);
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
            GameObject newMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/MercMonsterMaster"), "UtilityPhantasmTargetMaster", true);
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = UtilityPhantasmTargetBody;
            foreach (AISkillDriver ai in newMaster.GetComponentsInChildren<AISkillDriver>())
            {
                HolomancerPlugin.DestroyImmediate(ai);
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