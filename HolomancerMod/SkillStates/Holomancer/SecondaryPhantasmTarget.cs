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
    public class SecondaryPhantasmTarget : BaseSkillState
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
            this.duration = 0.2f;
            Ray aimRay = base.GetAimRay();
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
                HurtBox target = this.SearchForTarget();
                if (target && target.healthComponent)
                {
                    foreach (CharacterMaster cm in SecondaryPhantasm.SummonablesList2)
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
                            cm.GetBody().rigidbody.position = (base.characterBody.transform.position + base.GetAimRay().direction * 4 + Vector3.up * 5);
                            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/ImpBlinkEffect"), new EffectData
                            {
                                origin = base.characterBody.transform.position + base.GetAimRay().direction * 4 + Vector3.up * 5,
                                scale = 3f
                            }, true);



                        }
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