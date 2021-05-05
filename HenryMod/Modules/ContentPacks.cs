using RoR2.Skills;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;
using RoR2.ContentManagement;
using System.Collections;

namespace HenryMod.Modules
{
    internal class ContentPacks : IContentPackProvider
    {
        internal ContentPack contentPack = new ContentPack();
        public string identifier => HolomancerPlugin.MODUID;
        public static List<GameObject> masterPrefabs = new List<GameObject>();

        public void Initialize()
        {
            ContentManager.collectContentPackProviders += ContentManager_collectContentPackProviders;
        }

        private void ContentManager_collectContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(this);
        }

        public System.Collections.IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            this.contentPack.identifier = this.identifier;
            contentPack.bodyPrefabs.Add(Prefabs.bodyPrefabs.ToArray());
            contentPack.buffDefs.Add(Buffs.buffDefs.ToArray());
            contentPack.effectDefs.Add(Assets.effectDefs.ToArray());
            contentPack.entityStateTypes.Add(States.entityStates.ToArray());
            contentPack.masterPrefabs.Add(Prefabs.masterPrefabs.ToArray());
            contentPack.networkSoundEventDefs.Add(Assets.networkSoundEventDefs.ToArray());
            contentPack.projectilePrefabs.Add(Prefabs.projectilePrefabs.ToArray());
            contentPack.skillDefs.Add(Skills.skillDefs.ToArray());
            contentPack.skillFamilies.Add(Skills.skillFamilies.ToArray());
            contentPack.survivorDefs.Add(Prefabs.survivorDefinitions.ToArray());
            contentPack.unlockableDefs.Add(Unlockables.unlockableDefs.ToArray());

            args.ReportProgress(1f);
            yield break;
        }

        public System.Collections.IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(this.contentPack, args.output);
            args.ReportProgress(1f);
            yield break;
        }

        public System.Collections.IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }
    }
}