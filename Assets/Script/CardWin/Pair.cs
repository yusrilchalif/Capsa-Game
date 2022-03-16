using System.Collections;
using UnityEngine;
using CardSet = System.Collections.Generic.List<Card>;

public class Pair : IEvaluator<Pair>
{
    public override void Evaluate(int index)
    {
        base.Evaluate(index);

        if (index > cardSet.Count - 2)
            return;

        Card[] pair = { cardSet[index], cardSet[index + 1] };
        if(pair[0].Score == pair[1].Score && filter(pair[1]))
        {
            results.Add(new PokerWin(new CardSet(pair), pair[1], PokerWin.CombinationType.Pair));
        }
    }

    public override bool IsValid(CardSet cards, bool isSorted = false)
    {
        if (cards.Count != 2)
            return false;

        return cards[0].Score == cards[1].Score;
    }
}
