using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using RoR2.Skills;

namespace HenryMod.SkillStates
{
	internal class Mindwrack : BaseSkillState
	{
		
		private float duration = 0.2f;
		public override void OnEnter()
		{
			base.OnEnter();
			MindwrackClone.damagecoefficient = 1f;
			if (PrimaryPhantasm.SummonablesList1.Count > 0)
			{
				PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster CM1)
			{
				return !(CM1.GetBody().healthComponent.alive);
			});
			}
			if (PrimaryPhantasm.SummonablesList1.Count > 0)
				{

					foreach (CharacterMaster CM in PrimaryPhantasm.SummonablesList1)
					{
						if (CM.GetBody().healthComponent.alive == true)
						{
						CM.GetBody().GetComponent<RoR2.SkillLocator>().primary.SetSkillOverride(2, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("ExplosionDash")), RoR2.GenericSkill.SkillOverridePriority.Contextual);

						MindwrackClone.damagecoefficient += 0.5f;
							foreach (AISkillDriver ASD in CM.GetComponentsInChildren<AISkillDriver>())
							{

								bool flag = ASD.customName == "Attack";
								if (flag)
								{
									ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
									ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
									ASD.maxDistance = 100f;
									ASD.minDistance = 8f;
									ASD.skillSlot = SkillSlot.None;
									ASD.noRepeat = true;
								}

								bool flag2 = ASD.customName == "Shatter";
								if (flag2)
								{
									ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
									ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
									ASD.maxDistance = 8f;
									ASD.minDistance = 0f;
									ASD.skillSlot = SkillSlot.Primary;
								}

							}

							CM.GetBody().baseMoveSpeed = 25f;
							CM.GetBody().baseAcceleration = 160f;
							CM.inventory.GiveItem(RoR2Content.Items.HealthDecay.itemIndex, 15);

						}




					}
					PrimaryPhantasm.SummonablesList1.Clear();






					Debug.Log(PrimaryPhantasm.SummonablesList1);
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
				return;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}