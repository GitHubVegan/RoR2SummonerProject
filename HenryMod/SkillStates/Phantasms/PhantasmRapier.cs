using EntityStates;
using HenryMod.Modules;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using EntityStates.BrotherMonster.Weapon;



namespace HenryMod.SkillStates
{
    public class PhantasmRapier : BaseSkillState
    {
        public static float damageCoefficient = 3f;
        public static float procCoefficient = 0.4f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 12.5f;


        private float duration;
        private int bulletCount;
        private int totalBulletsFired;
        public static int baseBulletCount = 5;
        public static float baseDurationBetweenShots = 0.2f;
        private float durationBetweenShots;
        public static float totalDuration = 1f;
        public float stopwatchBetweenShots;
        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/LunarWispMinigunImpactHit");

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = PhantasmRapier.totalDuration;
            this.durationBetweenShots = PhantasmRapier.baseDurationBetweenShots / this.attackSpeedStat;
            this.bulletCount = (int)((float)PhantasmRapier.baseBulletCount * this.attackSpeedStat);
            this.FireBullet();
        }



        private void FireBullet()
        {
                Ray aimRay = base.GetAimRay();
                //base.PlayAnimation("FullBody, Override", "GroundLight1", "GroundLight.playbackRate", this.durationBetweenShots);
               // base.PlayAnimation("FullBody, Override", "RapierStab1", "RapierStab1.playbackRate", this.duration);
            base.PlayCrossfade("Gesture, Override", "Slash1", "Slash.playbackRate", this.durationBetweenShots, 0.05f);
                Util.PlaySound(FireLunarShards.fireSound, base.gameObject);

            if (base.isAuthority)
                {


                new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    maxSpread = 0f,
                    bulletCount = 1U,
                    damage = PhantasmRapier.damageCoefficient * this.damageStat,
                    force = PhantasmRapier.force,
                    tracerEffectPrefab = null,
                    muzzleName = null,
                    hitEffectPrefab = hitEffectPrefab,
                    isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
                    radius = 1.5f,
                    smartCollision = true,
                    damageType = DamageType.Generic
                }.Fire();
                this.totalBulletsFired++;
                }
            }
        
        
        public override void OnExit()
        {
            base.OnExit();
        }

     

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatchBetweenShots += Time.fixedDeltaTime;
            if (this.stopwatchBetweenShots >= this.durationBetweenShots && this.totalBulletsFired < this.bulletCount)
            {
                this.stopwatchBetweenShots -= this.durationBetweenShots;
                this.FireBullet();
            }
            if (base.fixedAge >= this.duration && this.totalBulletsFired == this.bulletCount && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}