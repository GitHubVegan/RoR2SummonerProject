using EntityStates;
using HenryMod.Modules;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using EntityStates.Assassin.Weapon;
using System.Linq;

namespace HenryMod.SkillStates
{
    public class PhantasmRapier : BaseSkillState
    {
        public static float damageCoefficient = 3f;
        public static float procCoefficient = 0.4f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 8f;
        private List<HurtBox> targetList;
        public static float maxRadius = 6f;


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
            base.characterBody.baseMoveSpeed = 7f;
            base.characterBody.baseAcceleration = 80f;
            this.FireBullet();
        }



        private void FireBullet()
        {
                Ray aimRay = base.GetAimRay();
                //base.PlayAnimation("FullBody, Override", "GroundLight1", "GroundLight.playbackRate", this.durationBetweenShots);
               // base.PlayAnimation("FullBody, Override", "RapierStab1", "RapierStab1.playbackRate", this.duration);
            base.PlayCrossfade("Gesture, Override", "Slash1", "Slash.playbackRate", this.durationBetweenShots, 0.05f);
            Util.PlaySound(SlashCombo.attackString, base.gameObject);

            if (base.isAuthority)
                {

                /*HurtBox hurtBox = this.SearchForTarget();
                if (hurtBox)
                {
                    bool flag = hurtBox.healthComponent.alive;
                    if(flag)
                    {
                        DamageInfo damageInfo = new DamageInfo();
                        damageInfo.damage = PhantasmRapier.damageCoefficient * this.damageStat;
                        damageInfo.attacker = base.gameObject;
                        damageInfo.procCoefficient = PhantasmRapier.procCoefficient;
                        damageInfo.position = hurtBox.transform.position;
                        damageInfo.crit = Util.CheckRoll(this.critStat, base.characterBody.master);
                        hurtBox.healthComponent.TakeDamage(damageInfo);
                    }
                }*/
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
                        force = 0f,
                        tracerEffectPrefab = null,
                        muzzleName = null,
                        stopperMask = LayerIndex.world.mask,
                        hitMask = LayerIndex.entityPrecise.mask,
                        hitEffectPrefab = hitEffectPrefab,
                        isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
                        radius = 2f,
                        smartCollision = true,
                        damageType = DamageType.Generic
                    }.Fire();
                    this.totalBulletsFired++;
                }
            }
        /*private HurtBox SearchForTarget()
        {
            Ray aimRay = base.GetAimRay();
            BullseyeSearch bullseyeSearch = new BullseyeSearch
            {
                searchOrigin = base.transform.position,
                searchDirection = aimRay.direction,
                maxAngleFilter = 90f,
                maxDistanceFilter = PhantasmRapier.maxRadius,
                teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam()),
                sortMode = BullseyeSearch.SortMode.Distance
            };
            bullseyeSearch.RefreshCandidates();
            bullseyeSearch.FilterOutGameObject(base.gameObject);
            return bullseyeSearch.GetResults().FirstOrDefault<HurtBox>();
        }*/

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