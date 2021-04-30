using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using EntityStates.JellyfishMonster;
using RoR2.Projectile;
using System.Collections.Generic;

namespace HenryMod.SkillStates
{
    public class PhantasmWard : BaseSkillState
    {
        public static float baseDuration = 6f;
        public List<ProjectileController> projectiles = new List<ProjectileController>();

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = PhantasmWard.baseDuration;
            projectiles.Clear();


        }




        public override void OnExit()
        {
            base.OnExit();
            projectiles.Clear();

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
            new RoR2.SphereSearch
            {
                radius = 15f,
                mask = RoR2.LayerIndex.projectile.mask,
                origin = base.characterBody.corePosition
            }.RefreshCandidates().FilterCandidatesByProjectileControllers().GetProjectileControllers(projectiles);
            foreach (ProjectileController PC in projectiles)
            {
                    PC.gameObject.GetComponent<ProjectileSimple>().SetForwardSpeed(15f);
            }
        }






        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }

}


    
