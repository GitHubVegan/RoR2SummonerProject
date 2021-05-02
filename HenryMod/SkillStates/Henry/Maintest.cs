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
								damage = base.characterBody.damage * 2f,
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

			SecondaryPhantasm.SummonablesList2.RemoveAll(delegate (CharacterMaster C) { return C == null; });
			if (SecondaryPhantasm.SummonablesList2.Count > 0)
			{
				SecondaryPhantasm.SummonablesList2.RemoveAll(delegate (CharacterMaster C)
				{
					return !(C.GetBody().healthComponent.alive);
				});
			}
			if (SecondaryPhantasm.SummonablesList2.Count > 0)
			{
				base.GetComponent<RoR2.SkillLocator>().secondary.SetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("SecondaryPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);

			}
			if (SecondaryPhantasm.SummonablesList2.Count <1 )
            {
				base.GetComponent<RoR2.SkillLocator>().secondary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("SecondaryPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
			}

			PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C) { return C == null; });
			if (PrimaryPhantasm.SummonablesList1.Count > 0)
			{
				PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C)
				{
					return !(C.GetBody().healthComponent.alive);
				});
			}
			if (PrimaryPhantasm.SummonablesList1.Count > 2)
			{
				base.GetComponent<RoR2.SkillLocator>().primary.SetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("PrimaryPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);

			}
			if (PrimaryPhantasm.SummonablesList1.Count <= 2)
			{
				base.GetComponent<RoR2.SkillLocator>().primary.UnsetSkillOverride(1, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("PrimaryPhantasmTarget")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
			}
		}
		

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

