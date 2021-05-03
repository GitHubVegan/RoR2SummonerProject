using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using EntityStates.JellyfishMonster;
using System.Collections.Generic;
using HenryMod.Modules;
using RoR2.Skills;

namespace HenryMod.SkillStates
{
    public class FlurryTest : BaseSkillState
    {
        public static float baseDuration = 0f;
        private float duration;
        public static bool shatterReady;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = FlurryTest.baseDuration;
            this.characterBody.baseAttackSpeed = this.characterBody.baseAttackSpeed * 3;
            this.characterBody.baseDamage = this.characterBody.baseDamage * 2;
            this.characterBody.baseMoveSpeed = 10f;
            this.characterBody.baseAcceleration = 100f;
            this.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 2f);
            FlurryTest.shatterReady = true;

        }

        public override void OnExit()
        {
            base.OnExit();
            this.characterBody.GetComponent<RoR2.SkillLocator>().primary.UnsetSkillOverride(2, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("FlurryTest")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
            this.characterBody.GetComponent<RoR2.SkillLocator>().primary.baseSkill.baseRechargeInterval = 0.5f;

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