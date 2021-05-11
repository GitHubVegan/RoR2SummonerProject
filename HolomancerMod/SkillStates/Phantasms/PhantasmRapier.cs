using EntityStates;
using HolomancerMod.Modules;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using EntityStates.Assassin.Weapon;
using System.Linq;

namespace HolomancerMod.SkillStates
{
    public class PhantasmRapier : BaseSkillState
    {
        public static float damageCoefficient = 1f;
        public static float procCoefficient = 0.4f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 10f;
        public static float maxRadius = 6f;
        public Vector3 point;
        private DamageInfo info;


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
            PhantasmRapier.damageCoefficient = 3f;
            this.duration = PhantasmRapier.totalDuration;
            this.durationBetweenShots = PhantasmRapier.baseDurationBetweenShots / this.attackSpeedStat;
            this.bulletCount = (int)((float)PhantasmRapier.baseBulletCount * this.attackSpeedStat);
            base.characterBody.baseMoveSpeed = 7f;
            base.characterBody.baseAcceleration = 80f;
            this.FireBullet();
        }



        private void FireBullet()
        {
            HurtBox hurtBox = this.SearchForTarget();
            if (hurtBox && hurtBox.healthComponent)
            {
            Ray aimRay = base.GetAimRay();
            base.PlayCrossfade("Gesture, Override", "Slash1", "Slash.playbackRate", this.durationBetweenShots, 0.05f);
            Util.PlaySound(SlashCombo.attackString, base.gameObject);

            if (base.isAuthority)
            {
                    this.info = new DamageInfo
                    {
                        attacker = base.gameObject,
                        position = hurtBox.healthComponent.transform.position,
                        damage = PhantasmRapier.damageCoefficient * this.damageStat,
                        crit = Util.CheckRoll(this.critStat, base.characterBody.master),
                        procCoefficient = PhantasmRapier.procCoefficient

                    };hurtBox.healthComponent.TakeDamage(this.info);
                    this.totalBulletsFired++;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private HurtBox SearchForTarget()
        {
            BullseyeSearch bullseyeSearch = new BullseyeSearch
            {
                searchOrigin = base.characterBody.transform.position,
                maxDistanceFilter = PhantasmRapier.range,
                teamMaskFilter = TeamMask.GetUnprotectedTeams(this.GetTeam()),
                sortMode = BullseyeSearch.SortMode.DistanceAndAngle
            };
            bullseyeSearch.RefreshCandidates();
            bullseyeSearch.FilterOutGameObject(this.gameObject);
            return bullseyeSearch.GetResults().FirstOrDefault<HurtBox>();
            
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