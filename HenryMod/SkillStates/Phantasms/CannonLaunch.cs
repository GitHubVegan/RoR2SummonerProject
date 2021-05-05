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
    public class CannonLaunch : BaseSkillState
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
            this.duration = 1f;
            if (base.isAuthority)
            {
                this.Fire();
            }
        }


        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                Util.PlaySound("Roll.dodgeSoundString", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    this.targetList = new List<HurtBox>();
                    new RoR2.SphereSearch
                    {
                        mask = LayerIndex.entityPrecise.mask,
                        origin = base.transform.position,
                        radius = GravityWell.novaRadius
                    }.RefreshCandidates().FilterCandidatesByDistinctHurtBoxEntities()./*FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).*/GetHurtBoxes(this.targetList);
                    foreach (HurtBox hurtBox in this.targetList)
                    {
                        bool flag2 = hurtBox && hurtBox.healthComponent.alive;
                        if (flag2)
                        {
                            if(hurtBox.gameObject != base.gameObject)
                            { 
                            DamageInfo damageInfo = new DamageInfo
                            {
                                damage = 0,
                                attacker = base.gameObject,
                                procCoefficient = 1f,
                                position = hurtBox.transform.position,
                                crit = false,
                                damageType = DamageType.Generic,
                                force = aimRay.direction * 2000

                            };
                            hurtBox.healthComponent.TakeDamage(damageInfo);
                            hurtBox.healthComponent.TakeDamageForce(damageInfo);
                            /*List<HurtBox> hurtBoxes = new List<HurtBox>();
                            new RoR2.SphereSearch
                            {
                                radius = 10f,
                                mask = LayerIndex.entityPrecise.mask,
                                origin = base.characterBody.transform.position,
                            }.RefreshCandidates().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes(hurtBoxes);
                            hurtBoxes.RemoveAll(delegate (HurtBox P) { return P == null; });
                            if (hurtBoxes.Count > 0)
                            {
                                hurtBoxes.RemoveAll(delegate (HurtBox P) { return P == null; });
                                foreach (HurtBox H in hurtBoxes)
                                {
                                    hurtBoxes.RemoveAll(delegate (HurtBox P) { return P == null; });

                                    if (H)
                                    {
                                        bool flag = H.healthComponent.alive;
                                        if (flag)
                                        {
                                            if (H.healthComponent.body)
                                            {
                                                if (H.healthComponent.body.characterMotor)
                                                {
                                                    this.motor = H.healthComponent.body.characterMotor;
                                                    this.mass = this.motor.mass;
                                                }
                                                else if (H.healthComponent.body.rigidbody)
                                                {
                                                    this.rb = H.healthComponent.body.rigidbody;
                                                    this.mass = this.rb.mass;
                                                }

                                                this.stopwatch = 0;
                                                this.lifetime = 5f;

                                                if (this.mass < 50f) this.mass = 50f;
                                                this.pushForce = 50f * this.mass;

                                                this.info = new DamageInfo
                                                {
                                                    attacker = null,
                                                    inflictor = null,
                                                    damage = 0,
                                                    damageColorIndex = DamageColorIndex.Default,
                                                    damageType = DamageType.Generic,
                                                    crit = false,
                                                    dotIndex = DotController.DotIndex.None,
                                                    force = aimRay.direction * this.pushForce * Time.fixedDeltaTime,
                                                    position = base.transform.position,
                                                    procChainMask = default(ProcChainMask),
                                                    procCoefficient = 0
                                                };

                                                if (this.motor)
                                                {
                                                    this.body.healthComponent.TakeDamageForce(this.info);
                                                }
                                                else if (this.rb)
                                                {
                                                    this.body.healthComponent.TakeDamageForce(this.info);
                                                }
                                            }
                                        }
                                    }
                                }
                            }*/
                        }
                        }
                    }
                }
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
                maxDistanceFilter = CannonLaunch.maxRadius,
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