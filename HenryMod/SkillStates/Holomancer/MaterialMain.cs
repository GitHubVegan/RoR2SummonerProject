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

	internal class MaterialMain : GenericCharacterMain
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
		}


		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

