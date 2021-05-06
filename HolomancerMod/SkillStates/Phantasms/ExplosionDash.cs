using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using EntityStates.JellyfishMonster;
using EntityStates.Loader;
using System.Collections.Generic;

namespace HolomancerMod.SkillStates
{
	public class ExplosionDash : BaseSkillState
    {
        public static float baseDuration = 4f;
        public static float novaRadius = 14f;
        public static float novaForce = 2500f;
        public static float damageCoefficient = 1f;
        private Vector3 punchVelocity;
        private List<HurtBox> targetList;

        private bool hasExploded;
        private float duration;
        private float stopwatch;
        private float distance1;

        public override void OnEnter()
        {
            base.OnEnter();
            this.stopwatch = 0f;
            this.duration = ExplosionDash.baseDuration;
            Ray aimRay = base.GetAimRay();
            Vector3 target = base.characterBody.transform.position - base.characterBody.gameObject.GetComponent<BaseAI>().currentEnemy.gameObject.transform.position;
            this.distance1 = Vector3.Distance(base.characterBody.transform.position, base.characterBody.gameObject.GetComponent<BaseAI>().currentEnemy.gameObject.transform.position);
            if (base.isAuthority)
            {
                base.characterMotor.velocity = distance1 * target;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.targetList = new List<HurtBox>();
            new RoR2.SphereSearch
            {
                mask = LayerIndex.entityPrecise.mask,
                origin = base.transform.position,
                radius = 2f
            }.RefreshCandidates().FilterCandidatesByDistinctHurtBoxEntities().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).GetHurtBoxes(this.targetList);
            foreach (HurtBox hurtBox in this.targetList)
            {
                bool flag2 = hurtBox && hurtBox.healthComponent.alive;
                if (flag2)
                {
                    this.Detonate();
                    return;
                    /* DamageInfo damageInfo = new DamageInfo
                     {
                         damage = ExplosionDash.damageCoefficient * this.damageStat,
                         attacker = base.gameObject,
                         procCoefficient = 1f,
                         position = hurtBox.transform.position,
                         crit = Util.CheckRoll(this.critStat, base.characterBody.master),
                         damageType = DamageType.Freeze2s
                     };
                     hurtBox.healthComponent.TakeDamage(damageInfo);*/


                    /*this.stopwatch += Time.fixedDeltaTime;

                    if (this.stopwatch >= this.duration && base.isAuthority && !this.hasExploded)
                    {
                        this.Detonate();
                        return;
                    }*/
                }
            }
            /*bool flag = base.fixedAge >= this.duration && base.isAuthority;
            if (flag)
            {
                this.Detonate();
                return;
            }*/
        }

        private void Detonate()
        {
            this.hasExploded = true;
            Util.PlaySound(EntityStates.JellyfishMonster.JellyNova.novaSoundString, base.gameObject);

            if (EntityStates.JellyfishMonster.JellyNova.novaEffectPrefab)
            {
                EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/MageLightningBombExplosion"), new EffectData
                {
                    origin = base.transform.position,
                    scale = ExplosionDash.novaRadius * 2
                }, true);
            }

            new BlastAttack
            {
                attacker = base.gameObject,
                inflictor = base.gameObject,
                teamIndex = TeamComponent.GetObjectTeam(base.gameObject),
                baseDamage = this.damageStat * 20f * ExplosionDash.damageCoefficient,
                baseForce = ExplosionDash.novaForce,
                position = base.transform.position,
                radius = ExplosionDash.novaRadius,
                procCoefficient = 1f,
                attackerFiltering = AttackerFiltering.NeverHit,
                damageType = DamageType.Generic
            }.Fire();

            if (base.healthComponent) base.healthComponent.Suicide(null, null, DamageType.Generic);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}