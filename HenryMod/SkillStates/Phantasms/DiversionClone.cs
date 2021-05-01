using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using EntityStates.JellyfishMonster;


namespace HenryMod.SkillStates
{
	public class DiversionClone : BaseSkillState
    {
        public static float baseDuration = 0.1f;
        public static float novaRadius = 18f;
        public static float novaForce = 2500f;

        private bool hasExploded;
        private float duration;
        private float stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            this.stopwatch = 0f;
            this.duration = DiversionClone.baseDuration / this.attackSpeedStat;
  
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
                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/NullifierDeathExplosion"), new EffectData
                {
                    origin = base.transform.position,
                    scale = DiversionClone.novaRadius
                }, true);
            }

            new BlastAttack
            {
                attacker = base.gameObject,
                inflictor = base.gameObject,
                teamIndex = TeamComponent.GetObjectTeam(base.gameObject),
                baseDamage = this.damageStat * EntityStates.JellyfishMonster.JellyNova.novaDamageCoefficient * 0.1f,
                baseForce = DiversionClone.novaForce,
                position = base.transform.position,
                radius = DiversionClone.novaRadius,
                procCoefficient = 1f,
                attackerFiltering = AttackerFiltering.NeverHit,
                damageType = DamageType.Freeze2s,
            }.Fire();

            if (base.healthComponent) base.healthComponent.Suicide(null, null, DamageType.Generic);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}