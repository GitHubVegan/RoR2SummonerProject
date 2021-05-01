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
			List<ProjectileController> projectiles2 = new List<ProjectileController>();
			new RoR2.SphereSearch
			{
				radius = 15f,
				mask = RoR2.LayerIndex.projectile.mask,
				origin = base.characterBody.transform.position,
			}.RefreshCandidates().FilterCandidatesByProjectileControllers().GetProjectileControllers(projectiles2);
			if(projectiles2.Count > 0)
			{
				foreach (ProjectileController PC in projectiles2.ToArray())
				{
					if (PC.owner != gameObject)
					{
					PC.owner = gameObject;
					PC.gameObject.GetComponent<ProjectileSimple>().SetForwardSpeed(10f);
					}
					projectiles2.RemoveAt(projectiles2.Count - 1);

				}
			}
		}
		

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

