using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using RoR2.Skills;

namespace HenryMod.SkillStates
{
	internal class Diversion : BaseSkillState
	{
		private float duration = 0.2f;
		public override void OnEnter()
		{
			base.OnEnter();
				if(SecondaryPhantasm.SummonablesList2.Count > 0) 
				{ 
				foreach (CharacterMaster CM2 in SecondaryPhantasm.SummonablesList2)
				{
					if(CM2.GetBody().healthComponent)
					{ 
						foreach (AISkillDriver ASD in CM2.GetComponentsInChildren<AISkillDriver>())
						{

							bool flag = ASD.customName == "Attack";
							if (flag)
							{
								ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
								ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
								ASD.maxDistance = 100f;
								ASD.minDistance = 12f;
								ASD.skillSlot = SkillSlot.None;
							}

							bool flag2 = ASD.customName == "Shatter";
							if (flag2)
							{
								ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
								ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
								ASD.maxDistance = 12f;
								ASD.minDistance = 0f;
								ASD.skillSlot = SkillSlot.Utility;
						}

					}

					CM2.GetBody().baseMoveSpeed = 25f;
					CM2.GetBody().baseAcceleration = 160f;
					CM2.inventory.GiveItem(RoR2Content.Items.HealthDecay.itemIndex, 15);
					}


				}
				SecondaryPhantasm.SummonablesList2.Clear();


				Debug.Log(SecondaryPhantasm.SummonablesList2);
				}
			
		}


		





		public override void OnExit()
		{

			base.OnExit();
			base.GetComponent<RoR2.SkillLocator>().primary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Mindwrack")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
			base.GetComponent<RoR2.SkillLocator>().secondary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Diversion")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
			base.GetComponent<RoR2.SkillLocator>().utility.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Distortion")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
			base.GetComponent<RoR2.SkillLocator>().special.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("ShatterSkillswapCancel")), RoR2.GenericSkill.SkillOverridePriority.Contextual);

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