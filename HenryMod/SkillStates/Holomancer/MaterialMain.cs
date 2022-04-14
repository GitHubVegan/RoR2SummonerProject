using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.CharacterAI;
using R2API;
using EntityStates.NullifierMonster;
using RoR2.Skills;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace HolomancerMod.SkillStates
{

	internal class MaterialMain : GenericCharacterMain
	{

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
			this.characterBody.GetComponentInChildren<CharacterModel>().baseRendererInfos = SkinnedRendererDisplaySetup(this.gameObject);
		}

		public static CharacterModel.RendererInfo[] SkinnedRendererDisplaySetup(GameObject obj)
		{
			SkinnedMeshRenderer[] meshes = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
			CharacterModel.RendererInfo[] renderInfos = new CharacterModel.RendererInfo[meshes.Length];

			for (int i = 0; i < meshes.Length; i++)
			{
				renderInfos[i] = new CharacterModel.RendererInfo
				{
					defaultMaterial = Modules.Assets.mainAssetBundle.LoadAsset<Material>("PhantasmHologram"),
					renderer = meshes[i],
					defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
					ignoreOverlays = false //We allow the mesh to be affected by overlays like OnFire or PredatoryInstinctsCritOverlay.
				};
			}

			return renderInfos;
		}


		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}

