using System.Collections;
using UnityEngine;
using CardSet = System.Collections.Generic.List<Card>;

public class One : IEvaluator<One>
{
    public override void Evaluate(int index)
    {
        if(filter(cardSet[index]))
        {
            var set = new CardSet();
            set.Add(cardSet[index]);
            results.Add(new PokerWin(set, set[0], PokerWin.CombinationType.One));
        }
    }

    public override bool IsValid(CardSet cards, bool isSorted = false)
    {
        return cards.Count == 1;
    }
}
