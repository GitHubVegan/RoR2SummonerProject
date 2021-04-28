using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using RoR2.Skills;

namespace HenryMod.SkillStates
{
	internal class Distortion : BaseSkillState
	{
		public float BaseDuration = 0.0f;
		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = this.BaseDuration / this.attackSpeedStat;
			if (UtilityPhantasm.SummonablesList3.Count > 0)
			{
				var bufftime = UtilityPhantasm.SummonablesList3.Count * 2f;
				base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, bufftime);
				foreach (CharacterMaster CM in UtilityPhantasm.SummonablesList3)
				{
					foreach (AISkillDriver ASD in CM.GetComponentsInChildren<AISkillDriver>())
					{
						ASD.maxDistance = Mathf.Infinity;
						ASD.minDistance = Mathf.NegativeInfinity;
						ASD.driverUpdateTimerOverride = 1f;
						ASD.skillSlot = SkillSlot.Special;
					}
					CM.GetBody().baseMoveSpeed = 0f;
					CM.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;


				}


			}
			UtilityPhantasm.SummonablesList3.Clear();


			Debug.Log(UtilityPhantasm.SummonablesList3);


		}





		public override void OnExit()
		{

			base.OnExit();
			base.GetComponent<RoR2.SkillLocator>().primary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Mindwrack")), RoR2.GenericSkill.SkillOverridePriority.Replacement);
			base.GetComponent<RoR2.SkillLocator>().secondary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Diversion")), RoR2.GenericSkill.SkillOverridePriority.Replacement);
			base.GetComponent<RoR2.SkillLocator>().utility.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Distortion")), RoR2.GenericSkill.SkillOverridePriority.Replacement);
			base.GetComponent<RoR2.SkillLocator>().special.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("ShatterSkillswapCancel")), RoR2.GenericSkill.SkillOverridePriority.Replacement);

		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextStateToMain();
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}