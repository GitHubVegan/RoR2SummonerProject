using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;


namespace HenryMod.SkillStates
{
	internal class Diversion : BaseSkillState
	{
		public float BaseDuration = 0.5f;
		private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = this.BaseDuration / this.attackSpeedStat;
			if (SummonSpecial.SummonablesList.Count > 0)
			{
				foreach (CharacterMaster CM in SummonSpecial.SummonablesList)
				{
					foreach (AISkillDriver ASD in CM.GetComponentsInChildren<AISkillDriver>())
					{
						ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
						ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;

						bool flag = ASD.customName == "Attack";
						if (flag)
						{
							ASD.maxDistance = 100f;
							ASD.minDistance = 5f;
							ASD.driverUpdateTimerOverride = -4f;
							ASD.skillSlot = SkillSlot.Primary;
						}

						bool flag2 = ASD.customName == "Shatter";
						if (flag2)
						{
							ASD.maxDistance = 10f;
							ASD.minDistance = 0f;
							ASD.skillSlot = SkillSlot.Utility;
						}

					}
					CM.GetBody().baseMoveSpeed = 40f;
					CM.GetBody().baseAcceleration = 200f;


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