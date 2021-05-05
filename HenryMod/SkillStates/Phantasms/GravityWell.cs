using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using EntityStates.JellyfishMonster;
using System.Collections.Generic;
using HenryMod.Modules;

namespace HenryMod.SkillStates
{
    public class GravityWell : BaseSkillState
    {
        public static float baseDuration = 0.1f;
        public static float novaRadius = 18f;
        public static float novaForce = 0f;
        private List<HurtBox> targetList;
        private bool hasExploded;
        private float duration;
        private float stopwatch;
        private static float damageCoefficient = 10f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.stopwatch = 0f;
            this.duration = GravityWell.baseDuration / this.attackSpeedStat;

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;

            if (this.stopwatch >= this.duration && base.isAuthority && !this.hasExploded)
            {
                this.Detonate();
                return;
            }
        }

        private void Detonate()
        {
            this.hasExploded = true;

            if (EntityStates.JellyfishMonster.JellyNova.novaEffectPrefab)
            {
                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/JellyfishNova"), new EffectData
                {
                    origin = base.transform.position,
                    scale = GravityWell.novaRadius
                }, true);
            }
                this.targetList = new List<HurtBox>();
                new RoR2.SphereSearch
                {
                    mask = LayerIndex.entityPrecise.mask,
                    origin = base.transform.position,
                    radius = GravityWell.novaRadius
                }.RefreshCandidates().FilterCandidatesByDistinctHurtBoxEntities().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).GetHurtBoxes(this.targetList);
            foreach (HurtBox hurtBox in this.targetList)
            {
                bool flag2 = hurtBox && hurtBox.healthComponent.alive;
                if (flag2)
                {
                    DamageInfo damageInfo = new DamageInfo
                    {
                        damage = GravityWell.damageCoefficient * this.damageStat,
                        attacker = base.gameObject,
                        procCoefficient = 1f,
                        position = hurtBox.transform.position,
                        crit = Util.CheckRoll(this.critStat, base.characterBody.master),
                        damageType = DamageType.Shock5s
                    };
                    hurtBox.healthComponent.TakeDamage(damageInfo);
                    if (hurtBox.healthComponent.gameObject.GetComponent<CharacterBody>().GetComponent<Knockdown>() == null) hurtBox.healthComponent.gameObject.AddComponent<Knockdown>().body = hurtBox.healthComponent.gameObject.GetComponent<CharacterBody>();
                }
            }


            if (this.healthComponent) this.healthComponent.Suicide(null, null, DamageType.Generic);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}