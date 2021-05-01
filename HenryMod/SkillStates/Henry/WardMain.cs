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
		List<ProjectileController> projectiles = new List<ProjectileController>();

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
			new RoR2.SphereSearch
			{
				radius = 15f,
				mask = RoR2.LayerIndex.projectile.mask,
				origin = base.characterBody.transform.position,
			}.RefreshCandidates().FilterCandidatesByProjectileControllers().GetProjectileControllers(projectiles);
			bool flag = (projectiles.Count) > 0;
			if (flag)
			{
				foreach (ProjectileController PC in projectiles.ToArray())
				{
					PC.gameObject.GetComponent<ProjectileSimple>().SetForwardSpeed(10f);
					projectiles.RemoveAt(projectiles.Count - 1);

				}
			}
		}
		

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

