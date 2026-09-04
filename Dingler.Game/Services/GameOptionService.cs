extern alias HexGame;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Mechanics;
using HexGame::Game.Shared.Mechanics.Abilities;
using HexGame::Reckoning.Game;

namespace Dingler.Game.Services;

public sealed class GameOptionService
{
	private readonly ResourceId _attackingTroopsResourceId = new("00052130-32aa-c4f5-d01e-8522345bdb1c");
	private readonly AuthoritativeSessionBase _session;

	public GameOptionService(AuthoritativeSessionBase session)
	{
		_session = session;
	}

	public PlayerOptionListSessionEventArgs CreateOptionListForPlayer(Player player)
	{
		var cardOptions = new Dictionary<Card, PlayerOptionSessionEventArgs>();
		List<Card> possibleInteractableCards = player.m_Hand.Union(player.m_Warzone).Union(player.m_Discard)
			.Union(player.m_Void).Union(player.m_Champions).ToList();

		if (player.m_ChampionCard.GetBool(IntAttrs.CanSeeTopOfDeck))
			possibleInteractableCards.Add(player.m_Deck.GetCardAtLocation(ECardLocations.Top));

		if (player.m_ChampionCard.GetBool(IntAttrs.CanSeeOpponentsTopOfDeck))
		{
			var opponent = _session.GetOpponentOfPlayer(player);
			possibleInteractableCards.Add(opponent.m_Deck.GetCardAtLocation(ECardLocations.Top));
		}

		GetAllPlayableCardsForPlayer(player, possibleInteractableCards, cardOptions);
		GetAllActivatableCardsForPlayer(player, possibleInteractableCards, cardOptions);
		GetAllAttacksForPlayer(player, cardOptions);
		GetAllBlocksForPlayer(player, cardOptions);
		GetAutoAbilitiesForOptionsForPlayer(player, cardOptions);

		var optionList = new PlayerOptionListSessionEventArgs()
		{
			PlayerId = player.m_PlayerId,
			Options = new()
		};

		foreach (var (_, option) in cardOptions)
		{
			optionList.Options.Add(option);
		}

		return optionList;
	}

	public PlayerOptionListSessionEventArgs CreateOptionListForPlayer(Player player, Card sourceCard,
		AbilityTemplate abilityTemplate, AbilityInstance abilityInstance)
	{
		var optionList = new PlayerOptionListSessionEventArgs()
		{
			PlayerId = player.m_PlayerId,
			SessionId = _session.m_SessionId,
			Options = new List<SessionEventArgs>()
		};

		optionList.Options.Add(CreateOptionForPlayer(player, sourceCard, abilityTemplate, abilityInstance));

		return optionList;
	}

	public PlayerOptionListSessionEventArgs CreateOptionListForPlayer(Player player, IList<Card> sourceCards,
		IList<AbilityTemplate> abilityTemplates, IList<AbilityInstance> abilityInstances)
	{
		var optionList = new PlayerOptionListSessionEventArgs()
		{
			PlayerId = player.m_PlayerId,
			SessionId = _session.m_SessionId,
			Options = new List<SessionEventArgs>()
		};

		for (int i = 0; i < sourceCards.Count; i++)
		{
			optionList.Options.Add(CreateOptionForPlayer(player, sourceCards[i], abilityTemplates[i],
				abilityInstances[i]));
		}

		return optionList;
	}

	private void GetAllPlayableCardsForPlayer(Player player, List<Card> cards,
		Dictionary<Card, PlayerOptionSessionEventArgs> cardOptions)
	{
		foreach (var card in cards)
		{
			if (!_session.CanPlayCard(card, player, card.GetCardContext().GetBool(IntAttrs.OwnerCanPlayForFree)))
				continue;

			cardOptions[card] = new PlayerOptionSessionEventArgs()
			{
				Card = card.m_SessionCardId,
				SessionId = _session.m_SessionId,
				State = ECardUsage.Play,
				Instances = new List<SessionEventArgs>()
			};
		}
	}

	private void GetAllActivatableCardsForPlayer(Player player, List<Card> cards,
		Dictionary<Card, PlayerOptionSessionEventArgs> cardOptions)
	{
		foreach (var card in cards)
		{
			foreach (var abilityId in card.CurrentAbilities)
			{
				if (!_session.CanActivateAbility(card, player, abilityId))
					continue;

				var ability = TemplateManager.Instance.Abilities[abilityId];

				if (!ability.IsManual)
					continue;

				if (!cardOptions.TryGetValue(card, out var option))
				{
					option = new PlayerOptionSessionEventArgs()
					{
						Card = card.m_SessionCardId,
						Instances = new List<SessionEventArgs>(),
						SessionId = _session.m_SessionId,
					};

					cardOptions[card] = option;
				}

				option.State |= ECardUsage.Activate;

				var abilityInstanceId =
					_session.AbilityManager.LookupAbilityInstanceId(card.m_SessionCardId, abilityId);
				
				AbilityInstance? abilityInstance = null;
				if (abilityInstanceId != 0)
					_session.AbilityManager.TryGetAbilityInstance(abilityInstanceId, out abilityInstance);
				
				var optionInstance = CreateOptionInstanceForPlayer(player, card, ability, abilityInstance);

				option.Instances.Add(optionInstance);
			}
		}
	}
	
	private void GetAllBlocksForPlayer(Player player, Dictionary<Card, PlayerOptionSessionEventArgs> cardOptions)
	{
		if (_session.CurrentTurnPhase != ETurnPhases.DeclareDefense || _session.GetActivePlayer().Equals(player))
			return;

		var attackers = _session.GetOpponentOfPlayer(player).GetAllAttackers();

		var warZoneCards = player.m_Warzone.m_Cards;

		foreach (var card in warZoneCards)
		{
			var potentialBlocks = attackers.Where(attacker => card.CanBlock(attacker) == ECombatantStatus.Ok)
				.Select(block => block.m_SessionCardId).ToList();

			if (potentialBlocks.Count == 0)
				continue;

			if (!cardOptions.TryGetValue(card, out var option))
			{
				option = new PlayerOptionSessionEventArgs()
				{
					Card = card.m_SessionCardId,
					Instances = new List<SessionEventArgs>(),
					SessionId = _session.m_SessionId
				};

				cardOptions[card] = option;
			}

			option.State |= ECardUsage.Defend;

			var optionInstance = new OptionInstanceSessionEventArgs()
			{
				MaxTargetCounts = [1],
				MinTargetCounts = [0],
				Id = ResourceId.Blocking,
				SessionId = _session.m_SessionId,
				TargetIds = [_attackingTroopsResourceId],
				TargetInstances = new List<SessionEventArgs>(),
			};

			var targetInstance = new TargetInstanceSessionEventArgs()
			{
				TargetId = ResourceId.Blocking,
				SessionId = _session.m_SessionId,
				TargetIndex = 0,
				Targets = potentialBlocks,
				AdditionalTargets = new List<SessionCardId>()
			};

			optionInstance.TargetInstances.Add(targetInstance);

			option.Instances.Add(optionInstance);
		}
	}

	private void GetAllAttacksForPlayer(Player player, Dictionary<Card, PlayerOptionSessionEventArgs> cardOptions)
	{
		if (_session.GetCurrentTurnPhase() != ETurnPhases.DeclareAttack ||
		    !_session.GetActivePlayer().Equals(player))
			return;

		var potentialAttacks = player.m_Warzone.m_Cards;

		foreach (var card in potentialAttacks)
		{
			if (card.CanAttack() != ECombatantStatus.Ok)
				continue;

			if (!cardOptions.TryGetValue(card, out var option))
			{
				option = new PlayerOptionSessionEventArgs()
				{
					Card = card.m_SessionCardId,
					Instances = new List<SessionEventArgs>(),
					SessionId = _session.m_SessionId,
				};

				cardOptions[card] = option;
			}


			if (card.GetCardContext().GetBool(IntAttrs.MustAttack))
			{
				option.State |= ECardUsage.ForcedAttack;
			}
			else
			{
				option.State |= ECardUsage.Attack;
			}
		}
	}

	private void GetAutoAbilitiesForOptionsForPlayer(Player player,
		Dictionary<Card, PlayerOptionSessionEventArgs> cardOptions)
	{
		foreach (var (card, option) in cardOptions)
		{
			foreach (var abilityId in card.CurrentAbilities)
			{
				if (!_session.CanPayAdditionalCost(card, abilityId))
					continue;
				
				var ability = TemplateManager.Instance.Abilities[abilityId];

				if (ability.IsManual)
					continue;
				
				var abilityInstanceId =
					_session.AbilityManager.LookupAbilityInstanceId(card.m_SessionCardId, abilityId);
				
				AbilityInstance? abilityInstance = null;
				if (abilityInstanceId != 0)
					_session.AbilityManager.TryGetAbilityInstance(abilityInstanceId, out abilityInstance);
				var optionInstance = CreateOptionInstanceForPlayer(player, card, ability, abilityInstance);

				option.Instances.Add(optionInstance);
			}
		}
	}

	private OptionInstanceSessionEventArgs CreateOptionInstanceForPlayer(Player player, Card sourceCard,
		AbilityTemplate abilityTemplate, AbilityInstance abilityInstance)
	{
		var maxTargets = _session.GetMaximumTargetCountsForAbility(sourceCard, player,
			abilityTemplate.AbilityTemplateId, abilityInstance);
		var minTargets = _session.GetMinimumTargetCountsForAbility(sourceCard, player,
			abilityTemplate.AbilityTemplateId, abilityInstance);

		var optionInstance = new OptionInstanceSessionEventArgs()
		{
			SessionId = _session.m_SessionId,
			MaxTargetCounts = maxTargets,
			MinTargetCounts = minTargets,
			Id = abilityTemplate.AbilityTemplateId,
			TargetIds = abilityTemplate.AbilityTargetTemplateIds,
			TargetInstances = new List<SessionEventArgs>(),
		};

		var targetsForAbility = _session.GetPotentialTargetsForAbility(sourceCard, player,
			abilityTemplate.AbilityTemplateId, abilityInstance);

		foreach (var kvp in targetsForAbility)
		{
			foreach (var target in kvp.Value)
			{
				optionInstance.TargetInstances.Add(new TargetInstanceSessionEventArgs()
				{
					SessionId = _session.m_SessionId,
					TargetId = target.Key,
					TargetIndex = kvp.Key,
					Targets = target.Value,
					AdditionalTargets = new List<SessionCardId>()
				});
			}
		}

		var costs =
			_session.GetPotentialCostsForAbility(sourceCard, player, abilityTemplate.AbilityTemplateId);

		foreach (var kvp in costs)
		{
			foreach (var costInfo in kvp.Value)
			{
				optionInstance.TargetInstances.Add(new CostInstanceSessionEventArgs()
				{
					SessionId = _session.m_SessionId,
					CostType = kvp.Key,
					Max = costInfo.Max,
					Min = costInfo.Min,
					Targets = costInfo.Targets,
					TargetTemplateId = costInfo.TargetTemplateId
				});
			}
		}

		return optionInstance;
	}
	
	private PlayerOptionSessionEventArgs CreateOptionForPlayer(Player player, Card sourceCard,
		AbilityTemplate abilityTemplate, AbilityInstance abilityInstance)
	{
		var option = new PlayerOptionSessionEventArgs()
		{
			Card = sourceCard.m_SessionCardId,
			State = ECardUsage.Activate,
			SessionId = _session.m_SessionId,
			Instances = new List<SessionEventArgs>()
		};

		option.Instances.Add(CreateOptionInstanceForPlayer(player, sourceCard, abilityTemplate, abilityInstance));

		return option;
	}
}