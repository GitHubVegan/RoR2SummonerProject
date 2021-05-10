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
    public class PrimaryPhantasm : BaseSkillState
    {
        public static float damageCoefficient = 0f;
        public static float procCoefficient = 0f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 250f;
        public static GameObject PrimaryPhantasmBody = CreateBody();
        public static GameObject PrimaryPhantasmMaster = CreateMaster();
        public static List<CharacterMaster> SummonablesList1 = new List<CharacterMaster>();
        public Vector3 point;

        private float duration;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = 0.2f;
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C) { return C == null; });
                if (PrimaryPhantasm.SummonablesList1.Count > 0)
                {
                    PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
                    }
                if (PrimaryPhantasm.SummonablesList1.Count > 2)
                {
                    CharacterMaster result = PrimaryPhantasm.SummonablesList1.Find(delegate (CharacterMaster C)
                        {
                            return C;
                        }
                        );
                    if (result != null)
                    {
                        result.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;
                        
                    }
                    
                    PrimaryPhantasm.SummonablesList1.RemoveAt(0);
                }
                CharacterMaster characterMaster = new MasterSummon
                {
                    masterPrefab = PrimaryPhantasmMaster,
                    position = base.characterBody.transform.position + base.GetAimRay().direction * 12,
                    rotation = base.characterBody.transform.rotation,
                    summonerBodyObject = base.characterBody.gameObject,
                    ignoreTeamMemberLimit = true,
                    teamIndexOverride = new TeamIndex?(TeamIndex.Player)
                }.Perform();
                characterMaster.GetBody().RecalculateStats();
                characterMaster.GetBody().baseDamage = characterMaster.GetBody().baseDamage * 0.3f;
                characterMaster.GetBody().levelDamage = characterMaster.GetBody().levelDamage * 0.3f;
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
            RaycastHit raycastHit;
            if (base.inputBank.GetAimRaycast(100f, out raycastHit))
            {
                this.point = raycastHit.point;
            }
            else
            {
                this.point = base.inputBank.GetAimRay().GetPoint(100f);
            }
            PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C) { return C == null; });
            if (PrimaryPhantasm.SummonablesList1.Count > 0)
            {
                PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
            }
            if (PrimaryPhantasm.SummonablesList1.Count > 0)
            { 
                HurtBox target = this.SearchForTarget();
                if (target && target.healthComponent)
                {
                    foreach (CharacterMaster cm in PrimaryPhantasm.SummonablesList1)
                    {
                        cm.gameObject.GetComponent<BaseAI>().currentEnemy.gameObject = target.healthComponent.gameObject;
                        if (Vector3.Distance(cm.GetBody().transform.position, target.healthComponent.body.transform.position) > (Vector3.Distance(base.characterBody.transform.position, target.healthComponent.body.transform.position)))
                        {
                            cm.GetBody().baseMoveSpeed = 20f;
                            cm.GetBody().baseAcceleration = 100f;
                            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/ImpBlinkEffect"), new EffectData
                            {
                                origin = cm.GetBody().transform.position,
                                scale = 3f
                            }, true);
                            cm.GetBody().characterMotor.Motor.SetPositionAndRotation(base.characterBody.transform.position + base.GetAimRay().direction * 4, base.characterBody.transform.rotation);
                            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/ImpBlinkEffect"), new EffectData
                            {
                                origin = base.characterBody.transform.position + base.GetAimRay().direction * 4,
                                scale = 3f
                            }, true);
                            


                        }
                    }
                }
                
            }
        }

        /*bool SummonPrimary(ref BulletAttack.BulletHit hitInfo)
        {
            PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C) { return C == null; });
            if (PrimaryPhantasm.SummonablesList1.Count > 0)
            {
                PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
            }
            if (PrimaryPhantasm.SummonablesList1.Count > 0)
            {
                bool flag = (hitInfo.entityObject != null && hitInfo.hitHurtBox != null && hitInfo.hitHurtBox.teamIndex != TeamIndex.Player);
                if (flag)
                {

                    foreach (CharacterMaster cm in PrimaryPhantasm.SummonablesList1)
                    {
                        cm.gameObject.GetComponent<BaseAI>().customTarget.gameObject = hitInfo.entityObject;

                    }
                }
                else
                {
                    foreach (CharacterMaster cm in PrimaryPhantasm.SummonablesList1)
                    {
                        cm.gameObject.GetComponent<BaseAI>().customTarget.gameObject = base.characterBody.gameObject;
                    }
                }
                if(!flag)
                {
                    foreach (CharacterMaster cm in PrimaryPhantasm.SummonablesList1)
                    {
                        cm.gameObject.GetComponentInChildren<AISkillDriver>().moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                    }
                }
            }
            return false;
            
        }*/

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

        private static GameObject CreateBody()
        {
            //GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/MercBody"), "PrimaryPhantasmBody", true);

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

            //        GameObject newBody = Prefabs.CreatePrefab("PrimaryPhantasmBody", "mdlPhantasmSword",bodyInfo);
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
            GameObject newMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/MercMonsterMaster"), "PrimaryPhantasmMaster", true);
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = PrimaryPhantasmBody;
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
            attackDriver.activationRequiresTargetLoS = true;
            attackDriver.selectionRequiresTargetLoS = false;
            attackDriver.maxDistance = 10f;
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
            attackDriver.skillSlot = SkillSlot.Primary;

            AISkillDriver shatterDriver = newMaster.AddComponent<AISkillDriver>();
            shatterDriver.customName = "Shatter";
            shatterDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shatterDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shatterDriver.activationRequiresAimConfirmation = false;
            shatterDriver.activationRequiresTargetLoS = false;
            shatterDriver.selectionRequiresTargetLoS = false;
            shatterDriver.maxDistance = 80f;
            shatterDriver.minDistance = 5f;
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