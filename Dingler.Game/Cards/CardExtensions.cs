extern alias HexGame;
using Card = HexGame::Game.Shared.Mechanics.Card;
using CardRepresentation = HexGame::Game.Shared.CardRepresentation;
using CardUpdatedSessionEventArgs = HexGame::Game.Shared.CardUpdatedSessionEventArgs;
using Player = HexGame::Game.Shared.Player;

namespace Dingler.Game.Cards;

public static class CardExtensions
{
	public static CardUpdatedSessionEventArgs ConvertToUpdateEventForPlayer(this Card card, Player player, bool forceFaceup)
	{
		CardRepresentation representation;
		var isCardFaceup = forceFaceup || card.CanPlayerSeeCard(player);

		if (isCardFaceup)
		{
			representation = new CardRepresentation(card);
			representation.Defense = card.CurrentHealthValue;
			var context = card.GetCardContext();

			foreach (var intAttr in context.GetCurrentIntAttrs())
			{
				representation.IntAttrs.Add(intAttr.Key, intAttr.Value);
			}

			foreach (var stringAttr in context.GetCurrentStringAttrs())
			{
				representation.StringAttrs.Add(stringAttr.Key, stringAttr.Value);
			}

			representation.ThresholdList.Clear();

			foreach (var threshold in card.Thresholds)
			{
				for (int i = 0; i < threshold.m_ThresholdColorRequirement; i++)
				{
					representation.ThresholdList.Add(threshold.m_ColorFlags);
				}
			}

		}
		else
		{
			representation = CardRepresentation.BlankCard;
		}

		var update = representation.ConvertToUpdateEvent(!isCardFaceup);

		update.SessionCardId = card.m_SessionCardId;
		update.PlayerId = player.m_PlayerId;
		update.Controller = card.GetControllingPlayer()?.m_PlayerId ?? player.m_PlayerId;
		return update;
	}

	public static CardUpdatedSessionEventArgs ConvertToUpdateEvent(this CardRepresentation cardRepresentation,
		bool faceDown)
	{
		var intAttrDictionary = new Dictionary<string, int>();
		foreach (var kvp in cardRepresentation.IntAttrs.Where(i => i.Key.NotifyClient))
		{
			intAttrDictionary.Add(kvp.Key.name, kvp.Value);
		}

		var stringAttrDictionary = new Dictionary<string, string>();
		foreach (var kvp in cardRepresentation.StringAttrs)
		{
			stringAttrDictionary.Add(kvp.Key.name, kvp.Value);
		}

		return new CardUpdatedSessionEventArgs()
		{
			Abilities = cardRepresentation.Abilities,
			ActivationCostModifiers = cardRepresentation.ActivationCostModifiers,
			AffectedId = cardRepresentation.AffectedId,
			AffectingAbilities = cardRepresentation.AffectingAbilities,
			AICardStates = cardRepresentation.AICardStates,
			Armor = cardRepresentation.Armor,
			Attack = cardRepresentation.Attack,
			Attributes = cardRepresentation.Attributes,
			CurrentArmor = cardRepresentation.CurrentArmor,
			ExtendedArt = cardRepresentation.IsExtended,
			CDMult = cardRepresentation.CDMult,
			ChargePointCostModifiers = cardRepresentation.ChargePointCostModifiers,
			Collection = cardRepresentation.Collection,
			Controller = cardRepresentation.Controller,
			CooldownCounts = cardRepresentation.CooldownCounts,
			Cost = cardRepresentation.Cost,
			Escalation = cardRepresentation.Escalation,
			UsesPerGameCounts = cardRepresentation.UsesPerGameCounts,
			MinConst = cardRepresentation.MinConst,
			DamageShield = cardRepresentation.DamageShield,
			Defense = cardRepresentation.Defense,
			DMult = cardRepresentation.DMult,
			Feral = cardRepresentation.Feral,
			Gems = cardRepresentation.Gems,
			IntegerVariables = cardRepresentation.IntegerVariables,
			Lethal = cardRepresentation.Lethal,
			MinLimited = cardRepresentation.MinLimited,
			Nulling = faceDown,
			OrigTemplate = cardRepresentation.OrigTemplate,
			Rage = cardRepresentation.Rage,
			RelatedCards = cardRepresentation.RelatedCards,
			SpellPointCostModifiers = cardRepresentation.SpellPointCostModifiers,
			State = cardRepresentation.State,
			SubType = cardRepresentation.SubType,
			ThresholdList = cardRepresentation.ThresholdList,
			Tunneling = cardRepresentation.Tunneling,
			Type = cardRepresentation.CardType,
			Id = cardRepresentation.TemplateId,
			CounterTemplates = cardRepresentation.Counters.Keys.ToList(),
			CounterCounts = cardRepresentation.Counters.Values.ToList(),
			IntAttributes = intAttrDictionary,
			StringAttrs = stringAttrDictionary,
		};
	}
}