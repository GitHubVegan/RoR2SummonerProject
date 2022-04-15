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
			if(this.GetComponent<BaseAI>().currentEnemy.characterBody && this.characterBody.characterMotor)
            {
				this.characterBody.characterMotor.Motor.SetPositionAndRotation(this.gameObject.GetComponent<BaseAI>().currentEnemy.gameObject.transform.position, this.characterBody.transform.rotation);
			}
			if (this.GetComponent<BaseAI>().currentEnemy.characterBody && this.characterBody.rigidbody && !this.characterBody.characterMotor)
            {
				this.characterBody.rigidbody.position = (this.gameObject.GetComponent<BaseAI>().currentEnemy.gameObject.transform.position);
			}


		}


		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

