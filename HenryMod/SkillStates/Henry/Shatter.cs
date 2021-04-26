using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;


namespace HenryMod.SkillStates
{
	internal class Shatter : BaseSkillState
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

						ASD.customName = "Alive";
						ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
						ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
						ASD.activationRequiresAimConfirmation = false;
						ASD.activationRequiresTargetLoS = false;
						ASD.selectionRequiresTargetLoS = false;
						ASD.maxDistance = 100f;
						ASD.minDistance = 0f;
						ASD.requireSkillReady = false;
						ASD.aimType = AISkillDriver.AimType.AtCurrentEnemy;
						ASD.ignoreNodeGraph = true;
						ASD.moveInputScale = 6f;
						ASD.driverUpdateTimerOverride = 1f;
						ASD.buttonPressType = AISkillDriver.ButtonPressType.Hold;
						ASD.minTargetHealthFraction = Mathf.NegativeInfinity;
						ASD.maxTargetHealthFraction = Mathf.Infinity;
						ASD.minUserHealthFraction = Mathf.NegativeInfinity;
						ASD.maxUserHealthFraction = Mathf.Infinity;
						ASD.skillSlot = SkillSlot.Special;
					}


					
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