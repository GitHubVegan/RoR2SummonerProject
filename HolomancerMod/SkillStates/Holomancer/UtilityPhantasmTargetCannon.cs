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
    public class UtilityPhantasmTargetCannon : BaseSkillState
    {
        public static float damageCoefficient = 0f;
        public static float procCoefficient = 0f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 250f;
        public static float maxDistance = 200f;
        public static Vector3 point;

        private static float d = 7;


        private float duration;
        private bool hasFired;



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
            if (base.inputBank.GetAimRaycast(UtilityPhantasmTargetCannon.maxDistance, out raycastHit))
            {
                UtilityPhantasmTargetCannon.point = raycastHit.point;
            }
            else
            {
                UtilityPhantasmTargetCannon.point = base.inputBank.GetAimRay().GetPoint(UtilityPhantasmTargetCannon.maxDistance);
            }

            UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster C) { return C == null; });
            if (UtilityPhantasm.SummonablesList3.Count > 0)
            {
                UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster CM2)
                {
                    return !(CM2.GetBody().healthComponent.alive);
                });
            }
            if (UtilityPhantasm.SummonablesList3.Count > 0)
            {

                foreach (CharacterMaster CM2 in UtilityPhantasm.SummonablesList3)
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


                        foreach (CharacterMaster cm in PrimaryPhantasm.SummonablesList1)
                        {
                            cm.gameObject.GetComponent<BaseAI>().leader.gameObject = CM2.gameObject;

                            foreach (AISkillDriver ASD in cm.GetComponentsInChildren<AISkillDriver>())
                            {

                                bool flag = ASD.customName == "Attack";
                                if (flag)
                                {
                                    ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                                    ASD.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                                    ASD.maxDistance = 120f;
                                    ASD.minDistance = 5f;
                                    ASD.skillSlot = SkillSlot.None;
                                }

                                bool flag2 = ASD.customName == "Shatter";
                                if (flag2)
                                {
                                    ASD.movementType = AISkillDriver.MovementType.Stop;
                                    ASD.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                                    ASD.maxDistance = 5f;
                                    ASD.minDistance = 0f;
                                    ASD.skillSlot = SkillSlot.None;
                                }
                                cm.GetBody().baseMoveSpeed = 25f;
                                cm.GetBody().baseAcceleration = 160f;
                            }
                        }
                    }

                            if (CM2.GetBody().healthComponent.alive == true)
                            {

                                CM2.GetBody().GetComponent<RoR2.SkillLocator>().primary.SetSkillOverride(4, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("CannonLaunch")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
                                if (CM2.GetBody().GetComponent<RoR2.SkillLocator>().primary.stock == 0) CM2.GetBody().GetComponent<RoR2.SkillLocator>().primary.AddOneStock();
                                foreach (AISkillDriver ASD in CM2.GetComponentsInChildren<AISkillDriver>())
                                {

                                    bool flag = ASD.customName == "Attack";
                                    if (flag)
                                    {
                                        /*ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                                        ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;*/
                                        ASD.maxDistance = 100f;
                                        ASD.minDistance = 10f;
                                        ASD.skillSlot = SkillSlot.Primary;
                                    }

                                    bool flag2 = ASD.customName == "Shatter";
                                    if (flag2)
                                    {
                                        /*ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                                        ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy; */
                                        ASD.maxDistance = 10f;
                                        ASD.minDistance = 0f;
                                        ASD.skillSlot = SkillSlot.Primary;
                                    }

                                }

                                /*CM2.GetBody().baseMoveSpeed = 25f;
                                CM2.GetBody().baseAcceleration = 160f;*/
                            }


                            Debug.Log(UtilityPhantasm.SummonablesList3);
                        
                    }
                }
        }
        

        /*bool SummonPrimary(ref BulletAttack.BulletHit hitInfo)
        {
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
                if (hitInfo.entityObject != null && hitInfo.hitHurtBox != null)
                {
                    foreach (CharacterMaster cm in UtilityPhantasm.SummonablesList3)
                    {
                        cm.gameObject.GetComponent<BaseAI>().currentEnemy.gameObject = hitInfo.entityObject;
                        
                    }
                }
                else
                {
                    foreach (CharacterMaster cm in UtilityPhantasm.SummonablesList3)
                    {
                        cm.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
                        /*foreach (AISkillDriver ai in cm.GetComponentsInChildren<AISkillDriver>())
                        {
                            bool flag = ai.customName == "Attack";
                            if (flag)
                            {
                                ai.maxDistance = 15f;
                            }
                            bool flag2 = ai.customName == "Shatter";
                            if (flag2)
                            {
                                ai.minDistance = 15f;
                            }
                        }
                    }
                }
            }

            return false;
            
        }*/
        
        public override void OnExit()
        {
            base.OnExit();
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