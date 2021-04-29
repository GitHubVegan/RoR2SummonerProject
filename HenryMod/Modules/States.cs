using HenryMod.SkillStates;
using HenryMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace HenryMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(PrimaryPhantasm));
            entityStates.Add(typeof(SecondaryPhantasm));
            entityStates.Add(typeof(UtilityPhantasm));
            entityStates.Add(typeof(ShatterSkillswap));
            entityStates.Add(typeof(ShatterSkillswapCancel));
            entityStates.Add(typeof(Mindwrack));
            entityStates.Add(typeof(MindwrackClone));
            entityStates.Add(typeof(Diversion));
            entityStates.Add(typeof(DiversionClone));
            entityStates.Add(typeof(Distortion));
        }
    }
}