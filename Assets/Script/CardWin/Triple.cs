using System.Collections;
using CardSet = System.Collections.Generic.List<Card>;

public class Triple : IEvaluator<Triple>
{
	public override void Evaluate(int index)
	{
		if (index > cardSet.Count - 3)
			return;

		// Compare first and last nominal, since it's already sorted
		Card[] triple = { cardSet[index], cardSet[index + 1], cardSet[index + 2] };
		if (triple[0].Score == triple[1].Score && triple[1].Score == triple[2].Score && filter(triple[2]))
		{
			results.Add(new PokerWin(new CardSet(triple), triple[2], PokerWin.CombinationType.Triple));
		}
	}

	public override bool IsValid(CardSet cards, bool isSorted = false)
	{
		if (cards.Count != 3)
			return false;

		return cards[0].Nominal == cards[1].Nominal
			&& cards[1].Nominal == cards[2].Nominal;
	}
}