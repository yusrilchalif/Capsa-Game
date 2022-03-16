using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using CardSet = System.Collections.Generic.List<Card>;

public class IEvaluator<T> where T : class, new()
{
    public static T sInstance = new T();
    public static T Instance
    {
        get
        {
            return sInstance;
        }
    }

#pragma warning disable 0414
    protected CardSet cardSet = null;
    protected Func<Card, bool> filter = null;
#pragma warning restore 0414

    protected List<PokerWin> results = new List<PokerWin>();
    public List<PokerWin> Result
    {
        get
        {
            if(cardSet != null)
            {
                Debug.Log("Get Results before Evaluation Complete");
            }
            return results;
        }
    }

    public void Begin(CardSet cards, Func<Card, bool> filterFunction = null)
    {
        results.Clear();
        cardSet = cards;
        filter = filterFunction != null ? filterFunction : any => true;

        if (cardSet == null)
            Debug.LogException(new Exception("CardSet is Empty"));
        if (cardSet.Count == 0)
            Debug.LogException(new Exception("cant evaluate empty cardSet"));

        PreEvaluate();
    }

    protected virtual void PreEvaluate()
    {

    }

    public virtual void Evaluate(int index)
    {
        if(cardSet == null)
        {
            Debug.LogException(new Exception("Evaluate is Called before Begin()"));
        }
    }

    protected virtual void PostEvaluate()
    {

    }

    public void End()
    {
        PostEvaluate();

        cardSet = null;
        filter = null;
    }

    public List<PokerWin> LazyEvaluator(CardSet cards, bool all = false, Func<Card, bool> filterFunction = null)
    {
        Begin(cards, filterFunction);
        for (int i = 0; i < cards.Count; ++i)
        {
            Evaluate(i);
        }
        End();
        return results;
    }

    public virtual bool IsValid(CardSet cards, bool isSorted = false)
    {
        return cards.Count > 0;
    }
}
