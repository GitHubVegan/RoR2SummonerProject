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
    public class Flurry : BaseSkillState
    {
        public static float damageCoefficient = 1f;
        public static float procCoefficient = 0.4f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 13f;
        public static float maxRadius = 6f;
        public Vector3 point;
        private DamageInfo info;
        private bool suicide;


        private float duration;
        private int bulletCount;
        private int totalBulletsFired;
        public static int baseBulletCount = 12;
        public static float baseDurationBetweenShots = 0.08f;
        private float durationBetweenShots;
        public static float totalDuration = 1f;
        public float stopwatchBetweenShots;
        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/LunarWispMinigunImpactHit");


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Flurry.totalDuration;
            this.durationBetweenShots = Flurry.baseDurationBetweenShots / this.attackSpeedStat;
            this.bulletCount = (int)((float)Flurry.baseBulletCount * this.attackSpeedStat);
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
                        damage = Flurry.damageCoefficient * this.damageStat,
                        crit = Util.CheckRoll(this.critStat, base.characterBody.master),
                        procCoefficient = Flurry.procCoefficient

                    }; hurtBox.healthComponent.TakeDamage(this.info);
                    GlobalEventManager.instance.OnHitEnemy(info, hurtBox.healthComponent.gameObject);
                    GlobalEventManager.instance.OnHitAll(info, hurtBox.healthComponent.gameObject);
                    this.totalBulletsFired++;

                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            /*if (!suicide)
            {
                suicide = true;
            }
            if (suicide)
            {*/
                if (base.healthComponent) base.healthComponent.Suicide(null, null, DamageType.Generic);
            //}
        }

        private HurtBox SearchForTarget()
        {
            BullseyeSearch bullseyeSearch = new BullseyeSearch
            {
                searchOrigin = base.characterBody.transform.position,
                maxDistanceFilter = Flurry.range,
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