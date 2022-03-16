using System.Collections;
using UnityEngine;
using CardSet = System.Collections.Generic.List<Card>;

public class FourOfAKind : IEvaluator<FourOfAKind>
{
	public override void Evaluate(int index)
	{
		base.Evaluate(index);

		if (cardSet.Count < 5 || index > cardSet.Count - 4)
			return;

		Card[] quad = { cardSet[index], cardSet[index + 1], cardSet[index + 2], cardSet[index + 3] };
		if (quad[0].Nominal == quad[3].Nominal && filter(quad[3]))
		{
			var set = new CardSet();
			set.Add(cardSet[index == 0 ? 4 : 0]);
			set.AddRange(quad);
			results.Add(new PokerWin(set, set[4], PokerWin.CombinationType.FourOfAKind));
		}
	}

	public override bool IsValid(CardSet cards, bool isSorted = false)
	{
		if (cards.Count != 5)
			return false;

		if (!isSorted)
			cards.Sort();

		return cards[0].Nominal == cards[3].Nominal
			|| cards[1].Nominal == cards[4].Nominal;
	}
}
