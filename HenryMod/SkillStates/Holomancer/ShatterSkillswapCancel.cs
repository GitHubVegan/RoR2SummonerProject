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
    public class ShatterSkillswapCancel : BaseSkillState
    {
        private float BaseDuration = 0f;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.BaseDuration;
            var a = base.GetComponent<RoR2.SkillLocator>().primary.stock;
            base.GetComponent<RoR2.SkillLocator>().primary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Mindwrack")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
            base.GetComponent<RoR2.SkillLocator>().primary.stock = a;

            var b = base.GetComponent<RoR2.SkillLocator>().secondary.stock;
            base.GetComponent<RoR2.SkillLocator>().secondary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Diversion")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
            base.GetComponent<RoR2.SkillLocator>().secondary.stock = b;

            var c = base.GetComponent<RoR2.SkillLocator>().utility.stock;
            base.GetComponent<RoR2.SkillLocator>().utility.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Distortion")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
            base.GetComponent<RoR2.SkillLocator>().utility.stock = c;

            var d = base.GetComponent<RoR2.SkillLocator>().special.stock;
            base.GetComponent<RoR2.SkillLocator>().special.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("ShatterSkillswapCancel")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
            base.GetComponent<RoR2.SkillLocator>().special.stock = d;

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