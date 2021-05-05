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
    public class Aimthrow : AimThrowableBase
    {
        public static float damageCoefficient = 3f;
        public static float procCoefficient = 0.4f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 8f;
        private List<HurtBox> targetList;
        public static float maxRadius = 6f;
        public float lifetime = 4f;
        private float pushForce;
        public CharacterBody body;
        private float mass;
        private Rigidbody rb;
        private CharacterMotor motor;
        private DamageInfo info;
        private float stopwatch;
        private bool hasFired;


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