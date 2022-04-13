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
	internal class Distortion : BaseSkillState
	{
		public float BaseDuration = 0.1f;
		private float duration;
		public static GameObject affixHauntedWard;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = this.BaseDuration;
			UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster C) { return C == null; });
			if (UtilityPhantasm.SummonablesList3.Count > 0)
			{
				UtilityPhantasm.SummonablesList3.RemoveAll(delegate (CharacterMaster CM3)
			{
				return !(CM3.GetBody().healthComponent.alive);
			});
			}
			if (UtilityPhantasm.SummonablesList3.Count > 0)
				{
					var bufftime = UtilityPhantasm.SummonablesList3.Count * 3f;
					base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, bufftime);
				Distortion.affixHauntedWard = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/AffixHauntedWard"));
				Distortion.affixHauntedWard.GetComponent<TeamFilter>().teamIndex = TeamIndex.None;
				Distortion.affixHauntedWard.GetComponent<BuffWard>().Networkradius = 2f;
				Distortion.affixHauntedWard.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(this.characterBody.gameObject);

				foreach (CharacterMaster CM3 in UtilityPhantasm.SummonablesList3)
					{

						foreach (AISkillDriver ASD in CM3.GetComponentsInChildren<AISkillDriver>())
						{
							{
								HolomancerPlugin.DestroyImmediate(ASD);
							}
						}
						//CM3.GetBody().baseMoveSpeed = 0f;
						//CM3.inventory.GiveItem(RoR2Content.Items.HealthDecay.itemIndex, 3);
						CM3.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;


					}

				}
				//UtilityPhantasm.SummonablesList3.Clear();


				Debug.Log(UtilityPhantasm.SummonablesList3);
			


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
			/*var buff = base.characterBody.HasBuff(RoR2Content.Buffs.HiddenInvincibility);
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
					foreach (ProjectileController PC in projectiles)
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
							projectiles.RemoveAt(projectiles.Count - 1);*/

		}
	






	public override InterruptPriority GetMinimumInterruptPriority()
	{
		return InterruptPriority.PrioritySkill;
	}
}
	}


