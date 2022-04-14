using EntityStates;
using HolomancerMod.Modules;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace HolomancerMod.SkillStates
{
    public class PrimaryPhantasm : BaseSkillState
    {
        public static float damageCoefficient = 0f;
        public static float procCoefficient = 0f;
        public static float force = 0f;
        public static float recoil = 0f;
        public static float range = 250f;
        public static GameObject PrimaryPhantasmBody = CreateBody();
        public static GameObject PrimaryPhantasmMaster = CreateMaster();
        public static List<CharacterMaster> SummonablesList1 = new List<CharacterMaster>();
        public Vector3 point;

        private float duration;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = 0.2f;
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C) { return C == null; });
                if (PrimaryPhantasm.SummonablesList1.Count > 0)
                {
                    PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
                    }
                if (PrimaryPhantasm.SummonablesList1.Count > 2)
                {
                    CharacterMaster result = PrimaryPhantasm.SummonablesList1.Find(delegate (CharacterMaster C)
                        {
                            return C;
                        }
                        );
                    if (result != null)
                    {
                        result.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;
                        
                    }
                    
                    PrimaryPhantasm.SummonablesList1.RemoveAt(0);
                }
                CharacterMaster characterMaster = new MasterSummon
                {
                    masterPrefab = PrimaryPhantasmMaster,
                    position = base.characterBody.transform.position + base.GetAimRay().direction * 12,
                    rotation = base.characterBody.transform.rotation,
                    summonerBodyObject = base.characterBody.gameObject,
                    ignoreTeamMemberLimit = true,
                    teamIndexOverride = new TeamIndex?(TeamIndex.Player)
                }.Perform();
                characterMaster.GetBody().RecalculateStats();
                characterMaster.GetBody().levelDamage = base.characterBody.levelDamage;
                characterMaster.inventory.CopyItemsFrom(base.characterBody.inventory);
                characterMaster.inventory.ResetItem(RoR2Content.Items.ExtraLife.itemIndex);
                characterMaster.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
                characterMaster.GetBody().GetComponent<RoR2.SkillLocator>().primary.SetSkillOverride(2, SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("PhantasmRapier")), RoR2.GenericSkill.SkillOverridePriority.Contextual);
                characterMaster.GetBody().isPlayerControlled = false;
                SummonablesList1.Add(characterMaster);
                this.Fire();
            }
        }


        private void Fire()
        {
            RaycastHit raycastHit;
            if (base.inputBank.GetAimRaycast(100f, out raycastHit))
            {
                this.point = raycastHit.point;
            }
            else
            {
                this.point = base.inputBank.GetAimRay().GetPoint(100f);
            }
            PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C) { return C == null; });
            if (PrimaryPhantasm.SummonablesList1.Count > 0)
            {
                PrimaryPhantasm.SummonablesList1.RemoveAll(delegate (CharacterMaster C)
                {
                    return !(C.GetBody().healthComponent.alive);
                });
            }
            if (PrimaryPhantasm.SummonablesList1.Count > 0)
            { 
                HurtBox target = this.SearchForTarget();
                if (target && target.healthComponent)
                {
                    foreach (CharacterMaster cm in PrimaryPhantasm.SummonablesList1)
                    {
                        cm.gameObject.GetComponent<BaseAI>().currentEnemy.gameObject = target.healthComponent.gameObject;
                        if (Vector3.Distance(cm.GetBody().transform.position, target.healthComponent.body.transform.position) > (Vector3.Distance(base.characterBody.transform.position, target.healthComponent.body.transform.position)))
                        {
                            cm.GetBody().baseMoveSpeed = 20f;
                            cm.GetBody().baseAcceleration = 100f;
                            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/HuntressBlinkEffect"), new EffectData
                            {
                                origin = cm.GetBody().transform.position,
                                scale = 0.5f
                            }, true);
                            EffectManager.SpawnEffect(Resources.Load<GameObject>("prefabs/effects/HuntressBlinkEffect"), new EffectData
                            {
                                origin = base.characterBody.transform.position + base.GetAimRay().direction * 4,
                                scale = 0.5f
                            }, true);
                            cm.GetBody().characterMotor.Motor.SetPositionAndRotation(base.characterBody.transform.position + base.GetAimRay().direction * 4, base.characterBody.transform.rotation);
                           
                            


                        }
                    }
                }

            }
        }

        private HurtBox SearchForTarget()
        {
            BullseyeSearch bullseyeSearch = new BullseyeSearch
            {
                searchOrigin = this.point,
                maxDistanceFilter = 15f,
                teamMaskFilter = TeamMask.GetUnprotectedTeams(this.GetTeam()),
                sortMode = BullseyeSearch.SortMode.Distance
            };
            bullseyeSearch.RefreshCandidates();
            bullseyeSearch.FilterOutGameObject(this.gameObject);
            return bullseyeSearch.GetResults().FirstOrDefault<HurtBox>();
        }

        public override void OnExit()
        {
            base.OnExit();
            foreach(CharacterMaster cm in SummonablesList1)
            {
                CharacterModel.RendererInfo[] renderinfos2 = cm.GetBody().modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos;
                for (int i = 0; i < renderinfos2.Length; i++)
                {
                    renderinfos2[i].defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram");
                }
            }
        }

        private static GameObject CreateBody()
        {

            GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/MercBody"), "PrimaryPhantasmBody", true);
            CharacterModel.RendererInfo[] renderinfos = newBody.GetComponentInChildren<ModelLocator>().modelBaseTransform.gameObject.GetComponentInChildren<CharacterModel>().baseRendererInfos;
            for (int i = 0; i < renderinfos.Length; i++)
            {
                renderinfos[i].defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram");
            }
            //SkinDef defaultSkin = Modules.Skins.CreateSkinDef("name",
               // Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
              //  renderinfos,
              //  mainRenderer,
              //  newBody);
            /*for (int i = 0; i < newBody.GetComponentInChildren<CharacterModel>().baseRendererInfos.Length; i++)
            {
                newBody.GetComponentInChildren<CharacterModel>().baseRendererInfos[i].defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram");
            }
            /*GameObject newBody = null;
            foreach (GameObject customCharacterbody in ContentPacks.bodyPrefabs)
            {
                Debug.Log($"bodyPrefabs contains GameObject {customCharacterbody.name}");
                if (customCharacterbody.name == "PhantasmSwordBody")
                {
                    newBody = customCharacterbody;

                }
            }
            return newBody;*/
            /*for (int i = Resources.Load<GameObject>("prefabs/characterbodies/MercBody").GetComponentInChildren<Renderer>().sharedMaterials.Length - 1; i >= 0; i--)
            {
                Resources.Load<GameObject>("prefabs/characterbodies/MercBody").GetComponentInChildren<Renderer>().sharedMaterials[i] = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram");
            }
            GameObject newBody = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/MercBody"), "PrimaryPhantasmBody", true);
            
            newBody.GetComponent<Renderer>().sharedMaterials = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram");
            var modelLocator = newBody.GetComponent<ModelLocator>();
            if (modelLocator)
            {
                Transform modelTransform = modelLocator.modelTransform;
                if (modelTransform)
                {
                    CharacterModel model = modelTransform.GetComponent<CharacterModel>();
                    if (model)
                    {
                        model.baseRendererInfos[0].defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram");
                    }
                }
            }*/
            /*foreach (var rendererInfo in newBody.GetComponentInChildren<CharacterModel>().baseRendererInfos)
            {
                foreach (var material in rendererInfo.renderer.materials)
                {
                    material.SetHopooMaterial(Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram"));
                }
            }
            for (int i = newBody.GetComponentInChildren<CharacterModel>().baseRendererInfos.Length - 1; i >= 0; i--)
            {
                CharacterModel.RendererInfo rendererInfo = newBody.GetComponentInChildren<CharacterModel>().baseRendererInfos[i];
                Renderer renderer = rendererInfo.renderer;
                newBody.GetComponentInChildren<CharacterModel>().UpdateRendererMaterials(renderer, Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram"), rendererInfo.ignoreOverlays);
            }
            newBody.GetComponent<ModelLocator>()*/
            return newBody;
            
        }

        public static CharacterModel.RendererInfo[] MaterialSwitchTest(GameObject obj)
        {
            SkinnedMeshRenderer[] renderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            CharacterModel.RendererInfo[] renderInfos = new CharacterModel.RendererInfo[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                renderInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram"),
                    renderer = renderers[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return renderInfos;
        }

        /*public static CharacterModel.RendererInfo[] SkinRendererDisplaySetup(GameObject obj)
        {
            GameObject gameObject = obj.GetComponent<ModelLocator>().modelTransform.gameObject;
            ModelSkinController modelSkinController = gameObject.GetComponent<ModelSkinController>();
            Renderer[] controllersInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
            CharacterModel.RendererInfo[] renderInfos = new CharacterModel.RendererInfo[modelSkinController.skins.Length];
            SkinDef skin = new SkinDef();
            for(int i = 0; i < modelSkinController.skins.Length; i++)
            {
                renderInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram"),
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false //We allow the mesh to be affected by overlays like OnFire or PredatoryInstinctsCritOverlay.
                };
            }
            modelSkinController.skins
            skin.name = "test";
            skin.nameToken = "MEFIEZ_SKIN_FEMARTICULTIST_NAME";
            skin.rootObject = gameObject;
            skin.baseSkins = new SkinDef[]
            {
                    modelSkinController.skins[0].
            };
            skin.unlockableDef = BodyCatalog.GetBodySkins(BodyCatalog.FindBodyIndex(str1))[1].unlockableDef;
            skin.rendererInfos = ArrayHelper<CharacterModel.RendererInfo>();

            skin.meshReplacements = new SkinDef.MeshReplacement[2];
            skin.meshReplacements[0].mesh = assetBundle.LoadAsset<Mesh>("Assets/Resources/MageAltMeshModified.asset");
            skin.meshReplacements[0].renderer = controllersInChildren[7];
            skin.meshReplacements[1].mesh = assetBundle.LoadAsset<Mesh>("Assets/Resources/MageAltCapeMeshModified.asset");
            skin.meshReplacements[1].renderer = controllersInChildren[6];
            skin.minionSkinReplacements = ArrayHelper.Empty<SkinDef.MinionSkinReplacement>();
            skin.projectileGhostReplacements = Array.Empty<SkinDef.ProjectileGhostReplacement>();
            Array.Resize(ref modelSkinController.skins, modelSkinController.skins.Length + 1);
            modelSkinController.skins[modelSkinController.skins.Length - 1] = skin;
            BodyCatalog.skins[(int)BodyCatalog.FindBodyIndex(bodyPrefab)] = modelSkinController.skins;
            return renderInfos;
        }

        /*public static CharacterModel.RendererInfo[] SkinDefDisplaySetup(GameObject obj)
        {
            SkinDef[] meshes = obj.GetComponentsInChildren<SkinDef>();
            ModelSkinController modelSkinController = obj.GetComponent<ModelSkinController>();
            SkinDef skin = new SkinDef();
            skin.baseSkins = new SkinDef[]
                {
                    modelSkinController.skins[0]
                };
            skin.baseSkins[1].rendererInfos[1].defaultMaterial

            for (int i = 0; i < meshes.Length; i++)
            {
                renderInfos[i] = new SkinDef
                {
                    defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram"),
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false //We allow the mesh to be affected by overlays like OnFire or PredatoryInstinctsCritOverlay.
                };
            }

            return renderInfos;
        }*/



        private static GameObject CreateMaster()
        {
            GameObject newMaster = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/MercMonsterMaster"), "PrimaryPhantasmMaster", true);
            //CharacterModel.RendererInfo[] renderinfos = PrimaryPhantasmBody.GetComponentInChildren<CharacterModel>(true).baseRendererInfos;
            //PrimaryPhantasmBody.GetComponentInChildren<CharacterModel>().baseRendererInfos = MaterialSwitchTest(PrimaryPhantasmBody);
            //PrimaryPhantasmBody.GetComponentInChildren<CharacterModel>().UpdateMaterials
            /*GameObject gameObject = PrimaryPhantasmBody.GetComponent<ModelLocator>().modelTransform.gameObject;
            ModelSkinController modelSkinController = gameObject.GetComponent<ModelSkinController>();
            for (int i = 0; i < modelSkinController.skins.Length; i++)
            {
                modelSkinController.skins[i].rendererInfos = MaterialSwitchTest(gameObject);

            }*/
            newMaster.GetComponent<CharacterMaster>().bodyPrefab = PrimaryPhantasmBody;
            foreach (AISkillDriver ai in newMaster.GetComponentsInChildren<AISkillDriver>())
            {
                HolomancerPlugin.DestroyImmediate(ai);
            }

            newMaster.GetComponent<BaseAI>().fullVision = true;


            AISkillDriver attackDriver = newMaster.AddComponent<AISkillDriver>();
            attackDriver.customName = "Attack";
            attackDriver.movementType = AISkillDriver.MovementType.Stop;
            attackDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            attackDriver.activationRequiresAimConfirmation = true;
            attackDriver.activationRequiresTargetLoS = true;
            attackDriver.selectionRequiresTargetLoS = false;
            attackDriver.maxDistance = 6.5f;
            attackDriver.minDistance = 0f;
            attackDriver.requireSkillReady = true;
            attackDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            attackDriver.ignoreNodeGraph = false;
            attackDriver.moveInputScale = 1f;
            attackDriver.driverUpdateTimerOverride = 0.2f;
            attackDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            attackDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            attackDriver.maxTargetHealthFraction = Mathf.Infinity;
            attackDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            attackDriver.maxUserHealthFraction = Mathf.Infinity;
            attackDriver.resetCurrentEnemyOnNextDriverSelection = false;
            attackDriver.skillSlot = SkillSlot.Primary;

            AISkillDriver shatterDriver = newMaster.AddComponent<AISkillDriver>();
            shatterDriver.customName = "Shatter";
            shatterDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            shatterDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            shatterDriver.activationRequiresAimConfirmation = false;
            shatterDriver.activationRequiresTargetLoS = false;
            shatterDriver.selectionRequiresTargetLoS = false;
            shatterDriver.maxDistance = 100f;
            shatterDriver.minDistance = 6.5f;
            shatterDriver.shouldSprint = false;
            shatterDriver.requireSkillReady = false;
            shatterDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            shatterDriver.ignoreNodeGraph = false;
            shatterDriver.moveInputScale = 1f;
            shatterDriver.driverUpdateTimerOverride = 0.2f;
            shatterDriver.buttonPressType = AISkillDriver.ButtonPressType.Hold;
            shatterDriver.minTargetHealthFraction = Mathf.NegativeInfinity;
            shatterDriver.maxTargetHealthFraction = Mathf.Infinity;
            shatterDriver.minUserHealthFraction = Mathf.NegativeInfinity;
            shatterDriver.maxUserHealthFraction = Mathf.Infinity;
            shatterDriver.resetCurrentEnemyOnNextDriverSelection = false;
            shatterDriver.skillSlot = SkillSlot.None;

            Modules.Content.AddMasterPrefab(newMaster);
            return newMaster;
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