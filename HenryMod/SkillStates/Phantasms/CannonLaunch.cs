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
using EntityStates.BeetleGuardMonster;

namespace HenryMod.SkillStates
{
    public class CannonLaunch : GroundSlam
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


        public override void OnExit()
        {
            var pos1 = base.transform.position;
            var pos2 = UtilityPhantasmTarget.point;
            var diff = pos2 - pos1;
            var dir = diff.normalized;
            var dir2 = base.characterBody.transform.rotation * Quaternion.FromToRotation(new Vector3(0, 0, 1), diff);
            foreach (CharacterMaster cm in PrimaryPhantasm.SummonablesList1)
            {
                cm.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;

                foreach (AISkillDriver ASD in cm.GetComponentsInChildren<AISkillDriver>())
                {

                    bool flag = ASD.customName == "Attack";
                    if (flag)
                    {
                        ASD.movementType = AISkillDriver.MovementType.Stop;
                        ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
                        ASD.maxDistance = 6;
                        ASD.minDistance = 0f;
                        ASD.skillSlot = SkillSlot.Primary;
                    }

                    bool flag2 = ASD.customName == "Shatter";
                    if (flag2)
                    {
                        ASD.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                        ASD.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
                        ASD.maxDistance = 100f;
                        ASD.minDistance = 6f;
                        ASD.skillSlot = SkillSlot.None;
                    }
                    cm.GetBody().baseMoveSpeed = 25f;
                    cm.GetBody().baseAcceleration = 160f;
                }
                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    this.targetList = new List<HurtBox>();
                    new RoR2.SphereSearch
                    {
                        mask = LayerIndex.entityPrecise.mask,
                        origin = base.transform.position,
                        radius = 15f
                    }.RefreshCandidates().FilterCandidatesByDistinctHurtBoxEntities()./*FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).*/GetHurtBoxes(this.targetList);
                    targetList.RemoveAll(delegate (HurtBox C) { return C == null; });
                    if (targetList.Count > 0)
                    {
                        targetList.RemoveAll(delegate (HurtBox C)
                        {
                            return !(C.healthComponent.alive);
                        });
                    }
                    foreach (HurtBox hurtBox in this.targetList)
                    {
                        /*if(hurtBox.teamIndex == TeamIndex.Player)
                        {
                            hurtBox.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.im, 3);
                        }*/
                        bool flag2 = hurtBox && hurtBox.healthComponent.alive;
                        if (flag2)
                        {
                            if (hurtBox.gameObject != base.gameObject)
                            {
                                if (hurtBox.healthComponent.body.characterMotor)
                                {
                                    this.motor = hurtBox.healthComponent.body.characterMotor;
                                    this.mass = this.motor.mass;
                                }
                                else if (this.body.rigidbody)
                                {
                                    this.rb = hurtBox.healthComponent.body.rigidbody;
                                    this.mass = this.rb.mass;
                                }

                                this.stopwatch = 0;
                                this.lifetime = 5f;

                                if (this.mass < 50f) this.mass = 50f;
                                this.pushForce = 50f * this.mass;
                                if(hurtBox.teamIndex == TeamIndex.Player && !hurtBox.healthComponent.isLocalPlayer)
                                {
                                    this.mass = 200f;
                                }
                                DamageInfo damageInfo = new DamageInfo
                                {
                                    damage = 0,
                                    attacker = base.gameObject,
                                    procCoefficient = 1f,
                                    position = hurtBox.transform.position,
                                    crit = false,
                                    damageType = DamageType.Generic,
                                    force = dir * this.pushForce * 0.6f

                                };
                                hurtBox.healthComponent.TakeDamage(damageInfo);
                                hurtBox.healthComponent.TakeDamageForce(damageInfo);
                                //base.characterBody.GetComponent<RoR2.SkillLocator>().primary.UnsetSkillOverride(4, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("CannonLaunch")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
                            }
                        }
                    }
                }
            }
        }
    }
}
