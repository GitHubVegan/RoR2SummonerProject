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
	internal class CooldownWardMain : GenericCharacterMain
	{
		private GameObject affixHauntedWard;
		public List<HurtBox> targetList;

		public override void OnEnter()
		{
			base.OnEnter();
			this.affixHauntedWard = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/AffixHauntedWard"));
			this.affixHauntedWard.GetComponent<TeamFilter>().teamIndex = TeamIndex.None;
			this.affixHauntedWard.GetComponent<BuffWard>().Networkradius = 15f;
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
			this.targetList = new List<HurtBox>();
			new RoR2.SphereSearch
			{
				mask = LayerIndex.entityPrecise.mask,
				origin = base.transform.position,
				radius = 15f
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
					hurtBox.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 0.25f);
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

