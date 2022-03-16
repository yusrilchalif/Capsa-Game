using System.Collections;
using System.Collections.Generic;
using CardSet = System.Collections.Generic.List<Card>;

public class FullHouse : IEvaluator<FullHouse>
{
	public List<PokerWin> triples = null;
	public List<PokerWin> pairs = null;

	Triple tripleEvaluator = new Triple();
	Pair pairEvaluator = new Pair();

	protected override void PreEvaluate()
	{
		if (triples != null || pairs != null)
			return;

		tripleEvaluator.Begin(cardSet, filter);
		pairEvaluator.Begin(cardSet, filter);
	}

	public override void Evaluate(int index)
	{
		base.Evaluate(index);

		if (triples != null || pairs != null)
			return;

		tripleEvaluator.Evaluate(index);
		pairEvaluator.Evaluate(index);
	}

	protected override void PostEvaluate()
	{
		if (triples == null && pairs == null)
		{
			tripleEvaluator.End();
			pairEvaluator.End();

			triples = tripleEvaluator.Result;
			pairs = pairEvaluator.Result;
		}

		CardSet fullHouse = new CardSet(5);
		for (int tIndex = 0; tIndex < triples.Count; ++tIndex)
		{
			fullHouse.Clear();
			for (int pIndex = 0; pIndex < pairs.Count; ++pIndex)
			{
				if (triples[tIndex].Cards[0].Nominal != pairs[pIndex].Cards[0].Nominal)
				{
					fullHouse.AddRange(pairs[pIndex].Cards);
					fullHouse.AddRange(triples[tIndex].Cards);
					results.Add(new PokerWin(fullHouse, triples[tIndex].Cards[2], PokerWin.CombinationType.FullHouse));
					break;
				}
			}
		}

		triples = null;
		pairs = null;
	}

	public override bool IsValid(CardSet cards, bool isSorted = false)
	{
		if (cards.Count != 5)
			return false;

		if (!isSorted)
			cards.Sort();

		return cards[0].Nominal == cards[1].Nominal && cards[1].Nominal == cards[2].Nominal && cards[3].Nominal == cards[4].Nominal
			|| cards[0].Nominal == cards[1].Nominal && cards[2].Nominal == cards[3].Nominal && cards[3].Nominal == cards[4].Nominal;
	}
}