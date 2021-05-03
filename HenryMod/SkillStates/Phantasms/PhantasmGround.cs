using EntityStates;
using HenryMod.Modules;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using EntityStates.Assassin.Weapon;




namespace HenryMod.SkillStates
{
    public class PhantasmGround : BaseSkillState
    {
        public static float damageCoefficient = 3f;
        public static float procCoefficient = 0.4f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 20f;


        private float duration;
        public static float totalDuration = 1f;
        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/LunarWispMinigunImpactHit");

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = PhantasmGround.totalDuration;
            base.characterBody.baseMoveSpeed = 6f;
            base.characterBody.baseAcceleration = 12f;
            this.FireBullet();
        }



        private void FireBullet()
        {
                Ray aimRay = base.GetAimRay();
                //base.PlayAnimation("FullBody, Override", "GroundLight1", "GroundLight.playbackRate", this.durationBetweenShots);
               // base.PlayAnimation("FullBody, Override", "RapierStab1", "RapierStab1.playbackRate", this.duration);
            base.PlayCrossfade("Gesture, Override", "Slash1", "Slash.playbackRate", this.duration, 0.05f);
            Util.PlaySound(SlashCombo.attackString, base.gameObject);

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
                    damage = PhantasmGround.damageCoefficient * this.damageStat,
                    force = 0f,
                    tracerEffectPrefab = null,
                    muzzleName = null,
                    hitEffectPrefab = hitEffectPrefab,
                    isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
                    radius = 3f,
                    smartCollision = true,
                    damageType = DamageType.Generic,
                    hitCallback = grounded
                }.Fire();
                }
            }

        bool grounded(ref BulletAttack.BulletHit hitInfo)
        {
            bool flag = (hitInfo.entityObject != null && hitInfo.hitHurtBox != null && hitInfo.hitHurtBox.teamIndex != TeamIndex.Player);
            if (flag)
            {
                if (hitInfo.entityObject.GetComponent<CharacterBody>().GetComponent<Knockdown>() == null) hitInfo.entityObject.AddComponent<Knockdown>().body = hitInfo.entityObject.GetComponent<CharacterBody>();
            }
                return true;
        }
        
        
        public override void OnExit()
        {
            base.OnExit();
        }

     

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (base.fixedAge >= this.duration && base.isAuthority)
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