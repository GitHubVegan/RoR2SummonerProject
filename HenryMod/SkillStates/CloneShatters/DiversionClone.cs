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
        public static float baseDuration = 0.0f;
        public static float novaRadius = 24f;
        public static float novaForce = 2500f;

        private bool hasExploded;
        private float duration;
        private float stopwatch;

        private GameObject chargeEffect;
        private PrintController printController;
        private uint soundID;

        public override void OnEnter()
        {
            base.OnEnter();
            this.stopwatch = 0f;
            this.duration = DiversionClone.baseDuration / this.attackSpeedStat;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                this.printController = modelTransform.GetComponent<PrintController>();
                if (this.printController)
                {
                    this.printController.enabled = true;
                    this.printController.printTime = this.duration;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            AkSoundEngine.StopPlayingID(this.soundID);

            if (this.chargeEffect) EntityState.Destroy(this.chargeEffect);
            if (this.printController) this.printController.enabled = false;
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
            Util.PlaySound(EntityStates.JellyfishMonster.JellyNova.novaSoundString, base.gameObject);

            if (base.modelLocator)
            {
                if (base.modelLocator.modelBaseTransform)
                {
                    EntityState.Destroy(base.modelLocator.modelBaseTransform.gameObject);
                }
                if (base.modelLocator.modelTransform)
                {
                    EntityState.Destroy(base.modelLocator.modelTransform.gameObject);
                }
            }

            if (this.chargeEffect)
            {
                EntityState.Destroy(this.chargeEffect);
            }

            if (EntityStates.JellyfishMonster.JellyNova.novaEffectPrefab)
            {
                EffectManager.SpawnEffect(EntityStates.JellyfishMonster.JellyNova.novaEffectPrefab, new EffectData
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
                baseDamage = 0.01f * EntityStates.JellyfishMonster.JellyNova.novaDamageCoefficient,
                baseForce = DiversionClone.novaForce,
                position = base.transform.position,
                radius = DiversionClone.novaRadius,
                procCoefficient = 2f,
                attackerFiltering = AttackerFiltering.NeverHit,
                damageType = DamageType.Freeze2s
            }.Fire();

            if (base.healthComponent) base.healthComponent.Suicide(null, null, DamageType.Generic);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}