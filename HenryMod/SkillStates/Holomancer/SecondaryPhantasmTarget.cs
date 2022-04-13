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
           
                HurtBox target = this.SearchForTarget();
                if (target && target.healthComponent)
                {

                    string bodyName = target.healthComponent.gameObject.GetComponent<CharacterBody>().master.bodyPrefab.name;
                    string myBody = base.characterBody.master.bodyPrefab.name;
                    Vector3 myPos = base.gameObject.transform.position;
                    Vector3 tarPos = target.healthComponent.gameObject.transform.position;
                    base.characterBody.characterMotor.Motor.SetPosition(tarPos);
                    base.characterBody.master.TransformBody(bodyName);
                    if (target.healthComponent.body.characterMotor)
                    {
                        target.healthComponent.body.characterMotor.Motor.SetPosition(myPos);
                    }
                    if (target.healthComponent.body.rigidbody)
                    {
                        target.healthComponent.body.rigidbody.position = myPos;
                    }
                    target.healthComponent.body.master.TransformBody(myBody);
                    


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