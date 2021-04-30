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
    public class PhantasmTaunt : BaseSkillState
    {
        public static float baseDuration = 0.0f;
        public static float novaRadius = 24f;
        public static float novaForce = 2500f;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = PhantasmTaunt.baseDuration / this.attackSpeedStat;
            RoR2.TeamMask enemyTeams = RoR2.TeamMask.GetEnemyTeams(this.teamComponent.teamIndex);
            RoR2.HurtBox[] hurtBoxes = new RoR2.SphereSearch
            {
                radius = 100f,
                mask = RoR2.LayerIndex.entityPrecise.mask,
                origin = base.characterBody.corePosition
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(enemyTeams).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes();
             for (int i = 0; i < hurtBoxes.Length; i++)
            {
                var body = hurtBoxes[i].healthComponent.gameObject;
                if (body)
                {
                    body.GetComponent<BaseAI>().skillDriverEvaluation.aimTarget.gameObject = base.characterBody.gameObject;
                }
            }



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