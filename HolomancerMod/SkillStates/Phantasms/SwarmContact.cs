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

		private GameObject affixHauntedWard;
		public static float damageCoefficient = 2f;
		public static float procCoefficient = 0.3f;

		public override void OnEnter()
		{
			base.OnEnter();
			/*this.affixHauntedWard = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/AffixHauntedWard"));
			this.affixHauntedWard.GetComponent<TeamFilter>().teamIndex = TeamIndex.None;
			this.affixHauntedWard.GetComponent<BuffWard>().Networkradius = 10f;
			this.affixHauntedWard.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(this.characterBody.gameObject);*/

			//setting the color didn't work with this, trying something else in the future

			//ParticleSystem.MainModule main = this.affixHauntedWard.gameObject.GetComponent<ParticleSystem>().main;
			//Color color = new Color(0.85f, 0.07f, 1f);
			//main.startColor = color;
		}




		public override void OnExit()
		{
			base.OnExit();
			//UnityEngine.Object.Destroy(this.affixHauntedWard);
			//this.affixHauntedWard = null;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
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

					if (H)
					{
						bool flag = H.healthComponent.alive && !H.healthComponent.body.HasBuff(RoR2Content.Buffs.BeetleJuice);
						if (flag)
						{
							H.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.BeetleJuice, 0.25f);
							DamageInfo damageInfo = new DamageInfo();
							damageInfo.damage = SwarmContact.damageCoefficient * this.damageStat * this.attackSpeedStat;
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

