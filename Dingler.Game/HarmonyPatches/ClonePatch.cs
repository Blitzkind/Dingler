extern alias HexGame;
using HarmonyLib;
using HexGame::Reckoning.Game;

namespace Dingler.Game.HarmonyPatches
{

    // Another Unity landmine. The CardTemplate.Clone function in the game uses a Unity specific method. So this gets around it by just copying all the values to a new one
    // which is what the game is doing just with more lines of code upfront.
    [HarmonyPatch(typeof(CardTemplate), "Clone")]
    public static class ClonePatch
    {
        static bool Prefix(CardTemplate __instance, ref CardTemplate __result)
        {
            __result = new CardTemplate()
            {
                HasAlternateArt = __instance.HasAlternateArt,
                HasAnimatedArt = __instance.HasAnimatedArt,
                HasExtendedArt = __instance.HasExtendedArt,
                m_AIHints = __instance.m_AIHints,
                m_ArtistName = __instance.m_ArtistName,
                m_ArtistNotes = __instance.m_ArtistNotes,
                m_ArtNumber = __instance.m_ArtNumber,
                m_ArtStatus = __instance.m_ArtStatus,
                m_AttributeFlags = __instance.m_AttributeFlags,
                m_AudioProductionStatus = __instance.m_AudioProductionStatus,
                m_BaseAttackValue = __instance.m_BaseAttackValue,
                m_CardAbilities = __instance.m_CardAbilities,
                m_VariableAttack = __instance.m_VariableAttack,
                m_VfxActivation = __instance.m_VfxActivation,
                m_BaseDefenseValue = __instance.m_BaseDefenseValue,
                m_BuildTag = __instance.m_BuildTag,
                m_CardImagePath = __instance.m_CardImagePath,
                m_CardLayoutId = __instance.m_CardLayoutId,
                m_CardNumber = __instance.m_CardNumber,
                m_CardRarity = __instance.m_CardRarity,
                m_CardSubtype = __instance.m_CardSubtype,
                m_CardType = __instance.m_CardType,
                m_CounterCosts = __instance.m_CounterCosts,
                m_CurrentResourcesGranted = __instance.m_CurrentResourcesGranted,
                m_DeckbuildingOneForEach = __instance.m_DeckbuildingOneForEach,
                m_DeckbuildingRequiresMatches = __instance.m_DeckbuildingRequiresMatches,
                m_DefaultLayout = __instance.m_DefaultLayout,
                m_DesignerCardId = __instance.m_DesignerCardId,
                m_DesignerNumber = __instance.m_DesignerNumber,
                m_DesignNotes = __instance.m_DesignNotes,
                m_DiscardTarget = __instance.m_DiscardTarget,
                m_DiscardTargets = __instance.m_DiscardTargets,
                m_DisplayText = __instance.m_DisplayText,
                m_EntersPlayExhausted = __instance.m_EntersPlayExhausted,
                m_EquipmentIsDisplayOnly = __instance.m_EquipmentIsDisplayOnly,
                m_EquipmentModifiedCard = __instance.m_EquipmentModifiedCard,
                m_EquipmentSlots = __instance.m_EquipmentSlots,
                m_ExhaustTarget = __instance.m_ExhaustTarget,
                m_ExhaustTargets = __instance.m_ExhaustTargets,
                m_ExtendedLayout = __instance.m_ExtendedLayout,
                m_Faction = __instance.m_Faction,
                m_FlavorText = __instance.m_FlavorText,
                m_GameText = __instance.m_GameText,
                m_Gender = __instance.m_Gender,
                m_HasExtendedLayout = __instance.m_HasExtendedLayout,
                m_Id = __instance.m_Id,
                m_ImplNotes = __instance.m_ImplNotes,
                m_ImplStage = __instance.m_ImplStage,
                m_IneligibleForPvERandomTemplates = __instance.m_IneligibleForPvERandomTemplates,
                m_IneligibleForPvPRandomTemplates = __instance.m_IneligibleForPvPRandomTemplates,
                m_IsPvE = __instance.m_IsPvE,
                m_LifeCost = __instance.m_LifeCost,
                m_LineageId = __instance.m_LineageId,
                m_MaxResourcesGranted = __instance.m_MaxResourcesGranted,
                m_Name = __instance.m_Name,
                m_OverrideEventEffects = __instance.m_OverrideEventEffects,
                m_OverrideStateEffects = __instance.m_OverrideStateEffects,
                m_Parsable = __instance.m_Parsable,
                m_PlayCondition = __instance.m_PlayCondition,
                m_PutIntoDeckTarget = __instance.m_PutIntoDeckTarget,
                m_PutIntoDeckTarget2 = __instance.m_PutIntoDeckTarget2,
                m_PutIntoHandTarget = __instance.m_PutIntoHandTarget,
                m_RageValue = __instance.m_RageValue,
                m_ResourceCost = __instance.m_ResourceCost,
                m_ResourceSymbolImagePath = __instance.m_ResourceSymbolImagePath,
                m_RevealTarget = __instance.m_RevealTarget,
                m_SacrificeTarget = __instance.m_SacrificeTarget,
                m_SerializedTAC = __instance.m_SerializedTAC,
                m_SetIconImagePath = __instance.m_SetIconImagePath,
                m_SetId = __instance.m_SetId,
                m_ShuffleIntoDeckTarget = __instance.m_ShuffleIntoDeckTarget,
                m_SocketCount = __instance.m_SocketCount,
                m_SoundEventOnSpawn = __instance.m_SoundEventOnSpawn,
                m_TalentModifiedCard = __instance.m_TalentModifiedCard,
                m_TalentModifiers = __instance.m_TalentModifiers,
                m_Threshold = __instance.m_Threshold,
                m_Tradeable = __instance.m_Tradeable,
                m_Unique = __instance.m_Unique,
                m_UniqueTag = __instance.m_UniqueTag,
                m_Unlimited = __instance.m_Unlimited,
                m_VariableCost = __instance.m_VariableCost,
                m_VariableCostDouble = __instance.m_VariableCostDouble,
                m_VariableCostMinimum = __instance.m_VariableCostMinimum,
                m_VariableDefense = __instance.m_VariableDefense,
                m_VfxCreation = __instance.m_VfxCreation,
                m_VfxDamage = __instance.m_VfxDamage,
                m_VfxDestruction = __instance.m_VfxDestruction,
                m_VoidTarget = __instance.m_VoidTarget,
            };

            return false;
        }
    }
}
