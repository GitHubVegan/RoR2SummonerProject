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

namespace HolomancerMod.SkillStates
{

	internal class Maintest : GenericCharacterMain
	{

		public override void OnEnter()
		{
			base.OnEnter();

		}


			

		public override void OnExit()
		{

			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			var buff = base.characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility);
			if (buff)
			{
				List<ProjectileController> projectiles = new List<ProjectileController>();
				new RoR2.SphereSearch
				{
					radius = 2f,
					mask = RoR2.LayerIndex.projectile.mask,
					origin = base.characterBody.corePosition
				}.RefreshCandidates().FilterCandidatesByProjectileControllers().GetProjectileControllers(projectiles);
				if (projectiles.Count > 0)
				{
					foreach (ProjectileController PC in projectiles.ToArray())
					{
						if (PC.owner != gameObject)
						{
							Vector3 target = PC.owner.transform.position - PC.gameObject.transform.position;

							PC.owner = gameObject;

							FireProjectileInfo info = new FireProjectileInfo()
							{
								projectilePrefab = PC.gameObject,
								position = PC.gameObject.transform.position,
								rotation = base.characterBody.transform.rotation * Quaternion.FromToRotation(new Vector3(0, 0, 1), target),
								owner = base.characterBody.gameObject,
								damage = base.characterBody.damage * 3.5f,
								force = 100f,
								crit = base.RollCrit(),
								damageColorIndex = DamageColorIndex.Default,
								target = null,
								speedOverride = 200f,
								fuseOverride = -1f
							};
							ProjectileManager.instance.FireProjectile(info);
							Destroy(PC.gameObject);
							projectiles.RemoveAt(projectiles.Count - 1);

						}
					}
				}

			}
			if (!buff)
			{
				UnityEngine.Object.Destroy(Distortion.affixHauntedWard);
				Distortion.affixHauntedWard = null;
			}
			if (SecondaryPhantasm.SummonablesList2.Count > 0)
			{
				SecondaryPhantasm.SummonablesList2.RemoveAll(delegate (CharacterMaster C1) { return C1 == null; });
			
				SecondaryPhantasm.SummonablesList2.RemoveAll(delegate (CharacterMaster C1)
				{
					return !(C1.GetBody().healthComponent.alive);
				});
			}
			if (SecondaryPhantasm.SummonablesList2.Count > 0)
			{
				var a = base.GetComponent<RoR2.SkillLocator>().secondary.stock;
				base.GetComponent<RoR2.SkillLocator>().secondary.SetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("SecondaryPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
				base.GetComponent<RoR2.SkillLocator>().secondary.stock = a;
			}
			if (SecondaryPhantasm.SummonablesList2.Count <= 0)
            {
				var a = base.GetComponent<RoR2.SkillLocator>().secondary.stock;
				base.GetComponent<RoR2.SkillLocator>().secondary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("SecondaryPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
				base.GetComponent<RoR2.SkillLocator>().secondary.stock = a;
			}

			if (PrimaryPhantasm.SummonablesList1.Count > 0)
			{
				PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C2) { return C2 == null; });
			
				PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C2)
				{
					return !(C2.GetBody().healthComponent.alive);
				});
			}
			if (PrimaryPhantasm.SummonablesList1.Count >= 3 || base.GetComponent<RoR2.SkillLocator>().primary.stock <= 0)
			{
				var b = base.GetComponent<RoR2.SkillLocator>().primary.stock;
				base.GetComponent<RoR2.SkillLocator>().primary.SetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("PrimaryPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
				base.GetComponent<RoR2.SkillLocator>().primary.stock = b;
			}
			if (PrimaryPhantasm.SummonablesList1.Count <= 2)
			{
				var b = base.GetComponent<RoR2.SkillLocator>().primary.stock;
				base.GetComponent<RoR2.SkillLocator>().primary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("PrimaryPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
				base.GetComponent<RoR2.SkillLocator>().primary.stock = b;
			}

			if (UtilityPhantasm.SummonablesList3.Count > 0)
			{
				UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster C3) { return C3 == null; });
			
				UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster C3)
				{
					return !(C3.GetBody().healthComponent.alive);
				});
			}
			if (UtilityPhantasm.SummonablesList3.Count > 0)
			{
				var c = base.GetComponent<RoR2.SkillLocator>().utility.stock;
				base.GetComponent<RoR2.SkillLocator>().utility.SetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("UtilityPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
				base.GetComponent<RoR2.SkillLocator>().utility.stock = c;
			}
			if (UtilityPhantasm.SummonablesList3.Count <= 0)
			{
				var c = base.GetComponent<RoR2.SkillLocator>().utility.stock;
				base.GetComponent<RoR2.SkillLocator>().utility.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("UtilityPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
				base.GetComponent<RoR2.SkillLocator>().utility.stock = c;
			}
		}
		

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

