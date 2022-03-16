using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CardSet = System.Collections.Generic.List<Card>;

public class PlayerEmotion : MonoBehaviour
{

    [System.Serializable]
    public class AvatarSet
    {
        public Sprite normal, happy, sad;
    }

    public Image avaImage;
    public GameObject panelInfo;
    public GameObject indicator;
    public Text labelTimer;
    public Text labelCount;
    public QuickBar quickBar;
    AvatarSet avatarSet;

    PlayerController gameManager;

    CardSet straight;
    CardSet flush;
    CardSet fullHouse;
    CardSet fourOfAKind;
    CardSet straightFlush;
    CardSet royalFlush;
    CardSet dragon;
    CardSet hint;

    PokerWin.CombinationType lastMarkCombination = PokerWin.CombinationType.Invalid;
    CardSet markedCards = new CardSet();

    void Awake()
    {
        gameManager = GetComponent<PlayerController>();
    }

	public void OnCardMarked(Card card)
	{
		markedCards.Add(card);
		lastMarkCombination = PokerWin.CombinationType.Invalid;
	}

	public void OnCardUnmarked(Card card)
	{
		markedCards.Remove(card);
		lastMarkCombination = PokerWin.CombinationType.Invalid;
	}

	public void OnTurnBegin()
	{
		if (panelInfo)
			panelInfo.SetActive(true);
		if (indicator)
			indicator.SetActive(false);
		avaImage.sprite = avatarSet.normal;
	}

	public void OnTurnEnd()
	{
		if (panelInfo)
			panelInfo.SetActive(false);
		if (quickBar)
			quickBar.Interactable = false;
		if (indicator)
			indicator.SetActive(false);
	}

	public void OnDeal()
	{
		gameManager.Deal(markedCards);
	}

	public void OnDealSuccess()
	{
		markedCards.Clear();
		this.TotalCard = gameManager.Cards.Count;
		avaImage.sprite = avatarSet.happy;
	}

	public void OnDealFailed()
	{
	}

	public void OnPass()
	{
		indicator.SetActive(gameManager.IsPass);
		avaImage.sprite = avatarSet.sad;
	}

	// Helper events for interface
	public void OnSelectStraight()
	{
		MarkAll(straight, lastMarkCombination != PokerWin.CombinationType.Straight);
		lastMarkCombination = PokerWin.CombinationType.Straight;
	}

	public void OnSelectFlush()
	{
		MarkAll(flush, lastMarkCombination != PokerWin.CombinationType.Flush);
		lastMarkCombination = PokerWin.CombinationType.Flush;
	}

	public void OnSelectFullHouse()
	{
		MarkAll(fullHouse, lastMarkCombination != PokerWin.CombinationType.FullHouse);
		lastMarkCombination = PokerWin.CombinationType.FullHouse;
	}

	public void OnSelectFourOfAKind()
	{
		MarkAll(fourOfAKind, lastMarkCombination != PokerWin.CombinationType.FourOfAKind);
		lastMarkCombination = PokerWin.CombinationType.FourOfAKind;
	}

	public void OnSelectStraightFlush()
	{
		MarkAll(straightFlush, lastMarkCombination != PokerWin.CombinationType.StraightFlush);
		lastMarkCombination = PokerWin.CombinationType.StraightFlush;
	}

	public void OnSelectRoyalFlush()
	{
		MarkAll(royalFlush, lastMarkCombination != PokerWin.CombinationType.RoyalFlush);
		lastMarkCombination = PokerWin.CombinationType.RoyalFlush;
	}

	public void OnSelectDragon()
	{
		MarkAll(dragon, lastMarkCombination != PokerWin.CombinationType.Dragon);
		lastMarkCombination = PokerWin.CombinationType.Dragon;
	}

	public void OnSelectHint()
	{
		MarkAll(hint, true);
		lastMarkCombination = PokerWin.CombinationType.Invalid;
	}

	void MarkAll(CardSet set, bool reset)
	{
		if (set == null)
			return;

		// Deselect all
		if (reset)
		{
			for (int i = 0; i < gameManager.Cards.Count; ++i)
			{
				gameManager.Cards[i].Select(false);
			}
		}

		// Select new card
		for (int i = 0; i < set.Count; ++i)
		{
			set[i].ToggleSelect();
		}
	}

	public void Display(CardSet set)
	{
		for (int i = 0; i < set.Count; ++i)
		{
			set[i].transform.SetParent(transform, false);
		}
	}

	// Property for Card Collection
	public CardSet MarkedCards
	{
		get { return markedCards; }
	}

	public int TotalCard
	{
		set { if (labelCount) labelCount.text = "" + value; }
	}

	public float TimeLeft
	{
		set { if (labelTimer) labelTimer.text = "" + Mathf.CeilToInt(value); }
	}

	public CardSet Straight
	{
		get { return straight; }
		set
		{
			straight = value;
			if (quickBar) quickBar.straight.interactable = true;
		}
	}

	public CardSet Flush
	{
		get { return flush; }
		set
		{
			flush = value;
			if (quickBar) quickBar.flush.interactable = true;
		}
	}

	public CardSet FullHouse
	{
		get { return fullHouse; }
		set
		{
			fullHouse = value;
			if (quickBar) quickBar.fullHouse.interactable = true;
		}
	}

	public CardSet FourOfAKind
	{
		get { return flush; }
		set
		{
			fourOfAKind = value;
			if (quickBar) quickBar.fourOfAKind.interactable = true;
		}
	}

	public CardSet StraightFlush
	{
		get { return straightFlush; }
		set
		{
			straightFlush = value;
			if (quickBar) quickBar.straightFlush.interactable = true;
		}
	}

	public CardSet RoyalFlush
	{
		get { return royalFlush; }
		set
		{
			royalFlush = value;
			if (quickBar) quickBar.royalFlush.interactable = true;
		}
	}

	public CardSet Dragon
	{
		get { return dragon; }
		set
		{
			dragon = value;
			if (quickBar) quickBar.dragon.interactable = true;
		}
	}

	public CardSet Hint
	{
		get { return hint; }
		set { hint = value; }
	}

	public AvatarSet Avatar
	{
		get { return avatarSet; }
		set
		{
			avatarSet = value;
			avaImage.sprite = avatarSet.normal;
		}
	}
}
