extern alias HexGame;
using HarmonyLib;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Mechanics.GameActions;

// The more I think about this the more I want it gone. If someone has time to look and see if an older version of Hex
// runs the below combo the way its described without this patch, I'm willing to mark this down as "Hex Entertainment
// changed how this works. This is realistic to before the shutdown". If you want it back uncomment the HarnmonyPatch attribute

namespace Dingler.Game.HarmonyPatches
{
    // Usually I wouldn't want to use a Harmony patch for some logic in the game but I can't seem to get ordering to work
    // correctly. To see what I mean, comment this out and run TerrorMill. In Hex before the shutdown, TerrorMill would
    // resolve like so:
    // Mill egg -> Terrorantula spawn -> Prompts player for target -> Target choosen -> Mill 10 repeat
    // Without this it goes:
    // Mill egg -> Terrorantula spawn -> Mill 10 -> chain again if we mill an egg -> finally prompt for targets
    // This looks bad. It fucks with the chain formatting.
    
    // This happens because at some point in the processing of triggers, even though the order is maintained in the files
    // Hex will just group triggers as targeted in one bucket, triggered in another. Which means that even though
    // Terrorantula has it's first effect as a target + destroy, it goes AFTER the mill.
    
    // My current theory is CardUpdatedEventArgs are just for presentation and before the first target you've already
    // milled the entire deck but the server just hasn't shown you that but I dunno. I'm tired. This is easy.
    
    public static class TriggeredAbilityOrderingPatch
    {
        //[HarmonyPatch(typeof(AuthoritativeSessionBase), "HandleTriggeredAbilities")]
        public static class HandleTriggeredAbilitiesPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(AuthoritativeSessionBase __instance)
            {
                try
                {
                    var actionStack = Traverse.Create(__instance)
                        .Field("m_ActionStack")
                        .GetValue<GameActionStack>();

                    if (actionStack == null || actionStack.HasUntargetedTriggers())
                    {
                        return false;
                    }

                    var allPlayersInTurnOrder = __instance.GetAllPlayersInTurnOrder();

                    for (var node = allPlayersInTurnOrder.First; node != null; node = node.Next)
                    {
                        var player = node.Value;

                        var allPending = new List<long>();
                        allPending.AddRange(player.TriggeredAbilities);
                        allPending.AddRange(player.UntargetedTriggeredAbilities);
                        if (allPending.Count == 0)
                            continue;

                        allPending.Sort((a, b) =>
                        {
                            var abilityA = __instance.AbilityManager.GetAbilityInstance(a);
                            var abilityB = __instance.AbilityManager.GetAbilityInstance(b);
                            var cardA = abilityA.SourceCard;
                            var cardB = abilityB.SourceCard;

                            var cardOrder = cardA.m_SessionCardId.InstanceId.CompareTo(cardB.m_SessionCardId.InstanceId);
                            if (cardOrder != 0)
                                return cardOrder;

                            var orderA = cardA.CurrentAbilities.ToList().IndexOf(abilityA.AbilityTemplateId);
                            var orderB = cardB.CurrentAbilities.ToList().IndexOf(abilityB.AbilityTemplateId);
                            return orderA.CompareTo(orderB);
                        });

                        var first = __instance.AbilityManager.GetAbilityInstance(allPending[0]);
                        var firstIsUntargeted = first.m_AbilityTemplate.UntargetedTrigger();

                        var run = new List<long>();
                        foreach (var id in allPending)
                        {
                            var ability = __instance.AbilityManager.GetAbilityInstance(id);
                            if (ability.m_AbilityTemplate.UntargetedTrigger() != firstIsUntargeted)
                                break;

                            run.Add(id);
                            player.TriggeredAbilities.Remove(id);
                            player.UntargetedTriggeredAbilities.Remove(id);
                            __instance.m_PendingTriggers.Add(id);
                        }

                        StaticLogger.LogDebug(
                            "OrderedTriggers run={Count} firstIsUntargeted={Untargeted} ids=[{Ids}]",
                            run.Count, firstIsUntargeted, string.Join(",", run));
                        
                        if (firstIsUntargeted)
                            __instance.PushGameAction(new WaitForUntargetedTriggeredAbilitiesAction(player.m_PlayerId, run));
                        else
                            __instance.PushGameAction(new WaitForTriggeredAbilitiesAction(player.m_PlayerId, run));

                        return false;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    StaticLogger.LogError(
                        "TriggeredAbilityOrderingPatch failed, falling back to stock HandleTriggeredAbilities: {Exception}",
                        ex);
                    return true;
                }
            }
        }
    }
}
