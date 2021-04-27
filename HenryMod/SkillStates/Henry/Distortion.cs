using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;


namespace HenryMod.SkillStates
{
	internal class Distortion : BaseSkillState
	{
		public float BaseDuration = 0.5f;
		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = this.BaseDuration / this.attackSpeedStat;
			if (SummonSpecial.SummonablesList.Count > 0)
			{
				var bufftime = SummonSpecial.SummonablesList.Count;
				base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, bufftime);
				foreach (CharacterMaster CM in SummonSpecial.SummonablesList)
				{
					foreach (AISkillDriver ASD in CM.GetComponentsInChildren<AISkillDriver>())
					{
						ASD.maxDistance = Mathf.Infinity;
						ASD.minDistance = Mathf.NegativeInfinity;
						ASD.driverUpdateTimerOverride = -5f;
						ASD.skillSlot = SkillSlot.Special;
					}
					CM.GetBody().baseMoveSpeed = 0f;
					CM.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;


				}


			}
			SummonSpecial.SummonablesList.Clear();


			Debug.Log(SummonSpecial.SummonablesList);


		}





		public override void OnExit()
		{

			base.OnExit();
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