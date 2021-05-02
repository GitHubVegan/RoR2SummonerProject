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
	internal class WardMain : GenericCharacterMain
	{
		private GameObject affixHauntedWard;

		public override void OnEnter()
		{
			base.OnEnter();
			this.affixHauntedWard = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/AffixHauntedWard"));
			this.affixHauntedWard.GetComponent<TeamFilter>().teamIndex = TeamIndex.None;
			this.affixHauntedWard.GetComponent<BuffWard>().Networkradius = 10f;
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
				radius = 10f,
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
					PC.gameObject.GetComponent<ProjectileSimple>().SetForwardSpeed(8f);
					}

				}
			}
			projectiles2.Clear();
		}
		

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

