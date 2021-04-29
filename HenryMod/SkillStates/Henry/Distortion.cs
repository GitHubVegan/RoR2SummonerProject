using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using RoR2.Skills;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace HenryMod.SkillStates
{
	internal class Distortion : BaseSkillState
	{
		public float BaseDuration = 0.0f;
		private float duration = 2f;

		public override void OnEnter()
		{
			base.OnEnter();



			if (UtilityPhantasm.SummonablesList3.Count > 0)
			{
				var bufftime = UtilityPhantasm.SummonablesList3.Count * 2f;
				base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, bufftime);

				foreach (CharacterMaster CM in UtilityPhantasm.SummonablesList3)
				{

					foreach (AISkillDriver ASD in CM.GetComponentsInChildren<AISkillDriver>())
					{
						{
							HenryPlugin.DestroyImmediate(ASD);
						}
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
			var buff = base.characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility);
			if (buff)
			{
				List<ProjectileController> bullets = new List<ProjectileController>();
				new RoR2.SphereSearch
				{
					radius = 2f,
					mask = RoR2.LayerIndex.projectile.mask,
					origin = base.characterBody.corePosition
				}.RefreshCandidates().FilterCandidatesByProjectileControllers().GetProjectileControllers(bullets);
				if (bullets.Count > 0)
				{
					foreach (ProjectileController pc in bullets)
					{
						if (pc.owner != gameObject)
						{
							Vector3 aimSpot = pc.owner.transform.position - pc.gameObject.transform.position;

							pc.owner = gameObject;

							FireProjectileInfo info = new FireProjectileInfo()
							{
								projectilePrefab = pc.gameObject,
								position = pc.gameObject.transform.position,
								rotation = base.characterBody.transform.rotation * Quaternion.FromToRotation(new Vector3(0, 0, 1), aimSpot),
								owner = base.characterBody.gameObject,
								damage = base.characterBody.damage * 2f,
								force = 200f,
								crit = base.RollCrit(),
								damageColorIndex = DamageColorIndex.Default,
								target = null,
								speedOverride = 120f,
								fuseOverride = -1f
							};
							ProjectileManager.instance.FireProjectile(info);
							Destroy(pc.gameObject);
							bullets.RemoveAt(bullets.Count - 1);

						}
					}
				}

			}
		}
		

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

