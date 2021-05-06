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
	internal class WardMain : FlyState
	{
		private GameObject affixHauntedWard;
		public List<HurtBox> targetList;

		public override void OnEnter()
		{
			base.OnEnter();
			this.affixHauntedWard = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/AffixHauntedWard"));
			this.affixHauntedWard.GetComponent<TeamFilter>().teamIndex = TeamIndex.None;
			this.affixHauntedWard.GetComponent<BuffWard>().Networkradius = 7f;
			this.affixHauntedWard.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(this.characterBody.gameObject);

			//setting the color didn't work with this, trying something else in the future

			//ParticleSystem.MainModule main = this.affixHauntedWard.gameObject.GetComponent<ParticleSystem>().main;
			//Color color = new Color(0.85f, 0.07f, 1f);
			//main.startColor = color;
		}




		public override void OnExit()
		{
			base.OnExit();
			UnityEngine.Object.Destroy(this.affixHauntedWard);
			this.affixHauntedWard = null;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			List<ProjectileController> projectiles2 = new List<ProjectileController>();
			new RoR2.SphereSearch
			{
				radius = 7f,
				mask = RoR2.LayerIndex.projectile.mask,
				origin = base.characterBody.transform.position,
			}.RefreshCandidates().FilterCandidatesByProjectileControllers().GetProjectileControllers(projectiles2);
			projectiles2.RemoveAll(delegate (ProjectileController P) { return P == null; });
			if (projectiles2.Count > 0)
			{
				projectiles2.RemoveAll(delegate (ProjectileController P) { return P == null; });
				foreach (ProjectileController PC in projectiles2)
				{
					projectiles2.RemoveAll(delegate (ProjectileController P) { return P == null; });
					if (PC.owner != gameObject)
					{
						EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/FeatherEffect"), new EffectData
						{
							origin = PC.transform.position,
							scale = 1f
						}, true);
						PC.owner = gameObject;
						PC.gameObject.GetComponent<ProjectileSimple>().SetForwardSpeed(14f);
					}

				}
			}
			projectiles2.Clear();

			this.targetList = new List<HurtBox>();
			new RoR2.SphereSearch
			{
				mask = LayerIndex.entityPrecise.mask,
				origin = base.transform.position,
				radius = 7f
			}.RefreshCandidates().FilterCandidatesByDistinctHurtBoxEntities()./*FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).*/GetHurtBoxes(this.targetList);
			targetList.RemoveAll(delegate (HurtBox C) { return C == null; });
			if (targetList.Count > 0)
			{
				targetList.RemoveAll(delegate (HurtBox C)
				{
					return !(C.healthComponent.alive);
				});
			}
			foreach (HurtBox hurtBox in this.targetList)
			{
				if (hurtBox.teamIndex == TeamIndex.Player)
				{
					hurtBox.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.SmallArmorBoost, 0.25f);
				}
				else
				{
					hurtBox.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.Cripple, 0.25f);
				}
			}
		}
		

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

