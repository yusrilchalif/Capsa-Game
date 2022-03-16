using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CardSet = System.Collections.Generic.List<Card>;

[RequireComponent(typeof(PlayerEmotion))]
public class PlayerController : MonoBehaviour
{
	public int id = -1;
	PlayerEmotion view;
	CardSet cards;
	bool isPass = false;

	// Possible Poker Hand Combinations
	List<PokerWin> hands = new List<PokerWin>(13);

	// Analyze Property
	bool isAnalyzing = false;
	bool analyzeAllMatch = true;
	System.Func<Card, bool> analyzeFilter = null;
	PokerWin.CombinationType analyzeCombination = PokerWin.CombinationType.Invalid;

	public bool Artificial
	{
		get { return id != 0; }
	}

	public CardSet Cards
	{
		get { return cards; }
		set
		{
			cards = value;
			cards.Sort();
			view.TotalCard = cards.Count;
			if (!Artificial)
				view.Display(cards);
		}
	}

    public PlayerEmotion playerEmotion
	{
		get { return view; }
	}

	public bool IsPass
	{
		get { return isPass; }
		set
		{
			isPass = value;
			view.indicator.SetActive(isPass);
		}
	}

	void Awake()
	{
		view = GetComponent<PlayerEmotion>();
	}

	public void OnTurnBegin()
	{
		view.OnTurnBegin();

		var LastTrick = TrickController.Instance.LastTrick;
		if (LastTrick != null)
		{
			analyzeCombination = LastTrick.Combination;
			analyzeFilter = trick => trick > LastTrick.Key;
		}
		StartCoroutine("OnTurn");
		StartCoroutine("Analyze");
	}

	IEnumerator OnTurn()
	{
		float time = 15f;
		float lastUpdateTime = Time.time;

		while (time > 0f)
		{
			yield return new WaitForSeconds(0.1f);

			time -= (Time.time - lastUpdateTime);
			lastUpdateTime = Time.time;
			view.TimeLeft = time;

			if (Artificial && !isAnalyzing)
			{
				if (hands.Count > 0)
				{
					if (!IsInvoking("AutoDeal"))
					{
						Invoke("AutoDeal", Random.Range(view.Hint.Count * 0.5f, view.Hint.Count * 2f));
					}
				}
				else
				{
					yield return new WaitForSeconds(0.5f);
					break;
				}
			}
		}
		if (!Artificial && TrickController.Instance.IsFirstTurn)
		{
			AutoDeal();
		}
		else
		{
			// Timeout
			Pass();
		}
	}

	public void OnTurnEnd()
	{
		StopCoroutine("Analyze");
		StopCoroutine("OnTurn");

		view.OnTurnEnd();
	}

	public void AutoDeal()
	{
		Deal(view.Hint);
	}

	public void Deal(CardSet dealCards)
	{
		if (dealCards.Count == 0)
			return;

		PokerWin hand = PokerWin.Make(dealCards);
		if (TrickController.Instance.Deal(hand))
		{
			view.OnDealSuccess();
		}
		else
		{
			view.OnDealFailed();
		}
	}

	public void Deal(PokerWin hand)
	{
		if (TrickController.Instance.Deal(hand))
		{
			view.OnDealSuccess();
		}
		else
		{
			view.OnDealFailed();
		}
	}

	public void Pass()
	{
		isPass = true;
		TrickController.Instance.Pass();
		view.OnPass();
	}

	IEnumerator Analyze()
	{
		isAnalyzing = true;
		hands.Clear();
		List<PokerWin> result;

		// One analysis each frame
		switch (analyzeCombination)
		{
			case PokerWin.CombinationType.Invalid:
				goto case PokerWin.CombinationType.One;
			case PokerWin.CombinationType.One:
				hands.AddRange(One.Instance.LazyEvaluator(cards, false, analyzeFilter));

				if (analyzeCombination == PokerWin.CombinationType.Invalid)
					goto case PokerWin.CombinationType.Pair;
				break;
			case PokerWin.CombinationType.Pair:
				hands.AddRange(Pair.Instance.LazyEvaluator(cards, analyzeAllMatch, analyzeFilter));

				if (analyzeCombination == PokerWin.CombinationType.Invalid)
					goto case PokerWin.CombinationType.Triple;
				break;
			case PokerWin.CombinationType.Triple:
				hands.AddRange(Triple.Instance.LazyEvaluator(cards, analyzeAllMatch, analyzeFilter));

				if (analyzeCombination == PokerWin.CombinationType.Invalid)
					goto case PokerWin.CombinationType.Straight;
				break;
			case PokerWin.CombinationType.Straight:
				result = Straight.Instance.LazyEvaluator(cards, analyzeAllMatch, analyzeFilter);
				hands.AddRange(result);
				if (result.Count > 0)
					view.Straight = result[result.Count - 1].Cards;
				if (!analyzeAllMatch && hands.Count > 0)
					break;
				yield return null;
				goto case PokerWin.CombinationType.Flush;
			case PokerWin.CombinationType.Flush:
				result = Flush.Instance.LazyEvaluator(cards, analyzeAllMatch, analyzeFilter);
				hands.AddRange(result);
				if (result.Count > 0)
					view.Flush = result[result.Count - 1].Cards;
				if (!analyzeAllMatch && hands.Count > 0)
					break;
				yield return null;
				goto case PokerWin.CombinationType.FullHouse;
			case PokerWin.CombinationType.FullHouse:
				result = FullHouse.Instance.LazyEvaluator(cards, analyzeAllMatch, analyzeFilter);
				hands.AddRange(result);
				if (result.Count > 0)
					view.FullHouse = result[result.Count - 1].Cards;
				if (!analyzeAllMatch && hands.Count > 0)
					break;
				yield return null;
				goto case PokerWin.CombinationType.FourOfAKind;
			case PokerWin.CombinationType.FourOfAKind:
				result = FourOfAKind.Instance.LazyEvaluator(cards, analyzeAllMatch, analyzeFilter);
				hands.AddRange(result);
				if (result.Count > 0)
					view.FourOfAKind = result[result.Count - 1].Cards;
				if (!analyzeAllMatch && hands.Count > 0)
					break;
				yield return null;
				goto case PokerWin.CombinationType.StraightFlush;
			case PokerWin.CombinationType.StraightFlush:
				result = StraightFlush.Instance.LazyEvaluator(cards, analyzeAllMatch, analyzeFilter);
				hands.AddRange(result);
				if (result.Count > 0)
					view.StraightFlush = result[result.Count - 1].Cards;
				if (!analyzeAllMatch && hands.Count > 0)
					break;
				yield return null;
				goto case PokerWin.CombinationType.RoyalFlush;
			case PokerWin.CombinationType.RoyalFlush:
				result = RoyalFlush.Instance.LazyEvaluator(cards, analyzeAllMatch, analyzeFilter);
				hands.AddRange(result);
				if (result.Count > 0)
					view.RoyalFlush = result[result.Count - 1].Cards;
				if (!analyzeAllMatch && hands.Count > 0)
					break;
				yield return null;
				goto case PokerWin.CombinationType.Dragon;
			case PokerWin.CombinationType.Dragon:
				result = Dragon.Instance.LazyEvaluator(cards, analyzeAllMatch, analyzeFilter);
				hands.AddRange(result);
				if (result.Count > 0)
					view.Dragon = result[result.Count - 1].Cards;
				break;
		}

		if (hands.Count > 0)
		{
			yield return null;
			var curr = hands[0];
			if (hands[hands.Count - 1].Combination == PokerWin.CombinationType.Dragon)
			{
				curr = hands[hands.Count - 1];
			}
			else
			{
				for (int i = 0; i < hands.Count; ++i)
				{
					if (TrickController.Instance.IsFirstTurn
						&& !(hands[i].Cards[0].Nominal == "3" && hands[i].Cards[0].suit == Card.Suit.Diamond))
						continue;
					if (curr.Combination != hands[i].Combination)
						curr = hands[i];
					if (curr.Combination >= PokerWin.CombinationType.Straight)
						break;
				}
			}
			view.Hint = curr.Cards;
		}
		else
		{
			view.Hint = null;
		}

		isAnalyzing = false;
		analyzeFilter = null;
		analyzeCombination = PokerWin.CombinationType.Invalid;
	}
}