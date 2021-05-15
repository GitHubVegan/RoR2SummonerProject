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
    public class UtilityPhantasm : BaseSkillState
    {
        public static float damageCoefficient = 0f;
        public static float procCoefficient = 0f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 250f;
        public static GameObject UtilityPhantasmBody = CreateBody();
        public static GameObject UtilityPhantasmMaster = CreateMaster();
        public Vector3 point;

        public static List<CharacterMaster> SummonablesList3 = new List<CharacterMaster>();


        private float duration;



        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = 0.2f;
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster C) { return C == null; });
                UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
                if (UtilityPhantasm.SummonablesList3.Count > 0)
                {

                    CharacterMaster result = UtilityPhantasm.SummonablesList3.Find(delegate (CharacterMaster C)
                    {
                        return C;
                    }
                        );
                    if (result != null)
                    {
                        result.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;

                    }
                    UtilityPhantasm.SummonablesList3.RemoveAt(0);


                }
                CharacterMaster characterMaster = new MasterSummon
                {
                    masterPrefab = UtilityPhantasmMaster,
                    position = base.characterBody.transform.position + Vector3.up * 5,
                    rotation = base.characterBody.transform.rotation,
                    summonerBodyObject = base.characterBody.gameObject,
                    ignoreTeamMemberLimit = true,
                    teamIndexOverride = new TeamIndex?(TeamIndex.Player)
                }.Perform();
                characterMaster.GetBody().RecalculateStats();
                characterMaster.GetBody().moveSpeed = 4f;
                characterMaster.GetBody().regen = base.characterBody.regen;
                characterMaster.GetBody().crit = base.characterBody.crit;
                characterMaster.GetBody().acceleration = 100f;
                characterMaster.GetBody().damage = base.characterBody.damage;
                characterMaster.GetBody().attackSpeed = base.characterBody.attackSpeed;
                characterMaster.inventory.CopyItemsFrom(base.characterBody.inventory);
                characterMaster.inventory.ResetItem(RoR2Content.Items.ExtraLife.itemIndex);
                characterMaster.inventory.GiveItem(RoR2Content.Items.Ghost.itemIndex);
                characterMaster.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
                characterMaster.GetBody().isPlayerControlled = false;
                SummonablesList3.Add(characterMaster);
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
                            if (Vector3.Distance(cm.GetBody().transform.position, cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position) > 35f)
                            {
                                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/HuntressBlinkEffect"), new EffectData
                                {
                                    origin = cm.GetBody().transform.position,
                                    scale = 0.5f
                                }, true);
                                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/HuntressBlinkEffect"), new EffectData
                                {
                                    origin = cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position + (cm.GetBody().transform.position - cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position).normalized * 25,
                                    scale = 0.5f
                                }, true);
                                cm.GetBody().rigidbody.position = cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position + (cm.GetBody().transform.position - cm.gameObject.GetComponent<BaseAI>().leader.gameObject.transform.position).normalized * 25;



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

        private static GameObject CreateBody()
        {
            GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/WispBody"), "UtilityPhantasmBody", true);
            newBody.GetComponentInChildren<EntityStateMachine>().mainStateType = new SerializableEntityStateType(typeof(WardMain));
            Modules.Prefabs.bodyPrefabs.Add(newBody);
            return newBody;
        }

        private static GameObject CreateMaster()
        {
            GameObject newMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/WispMaster"), "UtilityPhantasmMaster", true);
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = UtilityPhantasmBody;
            foreach (AISkillDriver ai in newMaster.GetComponentsInChildren<AISkillDriver>())
            {
                HolomancerPlugin.DestroyImmediate(ai);
            }

            newMaster.GetComponent<BaseAI>().fullVision = true;


            AISkillDriver attackDriver = newMaster.AddComponent<AISkillDriver>();
            attackDriver.customName = "Attack";
            attackDriver.movementType = AISkillDriver.MovementType.Stop;
            attackDriver.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            attackDriver.activationRequiresAimConfirmation = false;
            attackDriver.activationRequiresTargetLoS = false;
            attackDriver.selectionRequiresTargetLoS = false;
            attackDriver.maxDistance = 3f;
            attackDriver.minDistance = 0f;
            attackDriver.requireSkillReady = false;
            attackDriver.aimType = AISkillDriver.AimType.MoveDirection;
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
            shatterDriver.maxDistance = Mathf.Infinity;
            shatterDriver.minDistance = 3f;
            shatterDriver.requireSkillReady = false;
            shatterDriver.aimType = AISkillDriver.AimType.MoveDirection;
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