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
	internal class SwarmContact : FlyState
	{

		public static float damageCoefficient = 0.7f;
		public static float procCoefficient = 0.3f;
		private float stopwatch;

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
			stopwatch -= Time.fixedDeltaTime;
			if(stopwatch <= 0f)
			{
				stopwatch = 0.25f;
			List<HurtBox> hurtBoxes = new List<HurtBox>();
			new RoR2.SphereSearch
			{
				radius = 6f,
				mask = LayerIndex.entityPrecise.mask,
				origin = base.characterBody.transform.position,
			}.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes(hurtBoxes);
			hurtBoxes.RemoveAll(delegate (HurtBox P) { return P == null; });
			if (hurtBoxes.Count > 0)
			{
				hurtBoxes.RemoveAll(delegate (HurtBox P) { return P == null; });
				foreach (HurtBox H in hurtBoxes)
				{
					hurtBoxes.RemoveAll(delegate (HurtBox P) { return P == null; });
					hurtBoxes.RemoveAll(delegate (HurtBox P) { return P = this.characterBody.mainHurtBox; });

					if (H)
					{
						
						
							
							DamageInfo damageInfo = new DamageInfo();
							damageInfo.damage = SwarmContact.damageCoefficient * base.damageStat * base.attackSpeedStat;
							damageInfo.attacker = base.gameObject;
							damageInfo.procCoefficient = SwarmContact.procCoefficient;
							damageInfo.position = H.transform.position;
							damageInfo.crit = Util.CheckRoll(this.critStat, base.characterBody.master);
							H.healthComponent.TakeDamage(damageInfo);
						
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

