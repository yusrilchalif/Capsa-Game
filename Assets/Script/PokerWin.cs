using System;
using System.Collections;
using UnityEngine;
using CardSet = System.Collections.Generic.List<Card>;

public class PokerWin : IComparable<PokerWin>
{
   public enum CombinationType
    {
        Invalid, One, Pair, Triple, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush, Dragon
    }

    private Card key;
    private CardSet cards;
    private CombinationType combination;

    public PokerWin(CardSet cards, Card key, CombinationType combination)
    {
        this.cards = cards;
        this.key = key;
        this.combination = combination;
    }

    public static PokerWin Make(CardSet cards)
    {
        cards.Sort();
        var key = cards[cards.Count - 1];
        var combination = PokerWin.CombinationType.Invalid;

		if (cards.Count > 5)
		{
			if (Dragon.Instance.IsValid(cards, true))
				combination = CombinationType.Dragon;
		}
		else if (cards.Count == 5)
		{
			if (RoyalFlush.Instance.IsValid(cards, true))
				combination = CombinationType.RoyalFlush;
			else if (StraightFlush.Instance.IsValid(cards, true))
				combination = CombinationType.StraightFlush;
			else if (FourOfAKind.Instance.IsValid(cards, true))
				combination = CombinationType.FourOfAKind;
			else if (FullHouse.Instance.IsValid(cards, true))
				combination = CombinationType.FullHouse;
			else if (Flush.Instance.IsValid(cards, true))
				combination = CombinationType.Flush;
			else if (Straight.Instance.IsValid(cards, true))
				combination = CombinationType.Straight;
			else
				combination = CombinationType.Invalid;
		}
		else
		{
			if (Triple.Instance.IsValid(cards, true))
				combination = CombinationType.Triple;
			else if (Pair.Instance.IsValid(cards, true))
				combination = CombinationType.Pair;
			else if (One.Instance.IsValid(cards, true))
				combination = CombinationType.One;
			else
				combination = CombinationType.Invalid;
		}

		return new PokerWin(new CardSet(cards.ToArray()), key, combination);
	}

    int IComparable<PokerWin>.CompareTo(PokerWin other)
    {
        throw new NotImplementedException();
    }

    public Card Key
    {
		get { return key; }
    }

	public CardSet Cards
    {
		get { return cards; }
    }

	public CombinationType Combination
	{
		get { return combination; }
	}

	public static bool operator >(PokerWin lhs, PokerWin rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	public static bool operator <(PokerWin lhs, PokerWin rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	public int CompareTo(PokerWin other)
	{
		if (this.combination == other.combination)
			return this.key.CompareTo(other.key);
		else
			return (int)this.combination - (int)other.combination;
	}
}
