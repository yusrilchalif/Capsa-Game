using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrickView : MonoBehaviour
{
	public GameObject resultPanel;
	public Image announcer;
	public Text notifier;

	public Sprite dragon;
	public Sprite royalFlush;
	public Sprite straightFlush;
	public Sprite fourOfAKind;
	public Sprite fullhouse;
	public Sprite flush;
	public Sprite straight;
	public Sprite triple;
	public Sprite pair;

	TrickController controller;

	void Awake()
	{
		controller = GetComponent<TrickController>();
	}

	public void OnDealSuccess()
	{
	}

	public void OnDealFailed()
	{
	}

	public void OnGameOver()
	{
		if (resultPanel == null)
			return;
		resultPanel.SetActive(true);

		resultPanel = resultPanel.transform.GetChild(0).gameObject;
		var p = resultPanel.transform.Find("Player 1");
		p.transform.Find("Avatar").GetComponent<Image>().sprite = controller.players[0].playerEmotion.Avatar.normal;
		p.transform.Find("Card").GetComponent<Text>().text = controller.players[0].Cards.Count.ToString();
		p.transform.Find("Points").GetComponent<Text>().text = GetPoint(0).ToString();

		p = resultPanel.transform.Find("Player 2");
		p.transform.Find("Avatar").GetComponent<Image>().sprite = controller.players[1].playerEmotion.Avatar.normal;
		p.transform.Find("Card").GetComponent<Text>().text = controller.players[1].Cards.Count.ToString();
		p.transform.Find("Points").GetComponent<Text>().text = GetPoint(1).ToString();

		p = resultPanel.transform.Find("Player 3");
		p.transform.Find("Avatar").GetComponent<Image>().sprite = controller.players[2].playerEmotion.Avatar.normal;
		p.transform.Find("Card").GetComponent<Text>().text = controller.players[2].Cards.Count.ToString();
		p.transform.Find("Points").GetComponent<Text>().text = GetPoint(2).ToString();

		p = resultPanel.transform.Find("Player 4");
		p.transform.Find("Avatar").GetComponent<Image>().sprite = controller.players[3].playerEmotion.Avatar.normal;
		p.transform.Find("Card").GetComponent<Text>().text = controller.players[3].Cards.Count.ToString();
		p.transform.Find("Points").GetComponent<Text>().text = GetPoint(3).ToString();

		resultPanel = resultPanel.transform.parent.gameObject;
	}

	int GetPoint(int playerId)
	{
		var cardLeft = controller.players[playerId].Cards.Count;
		if (cardLeft >= 13)
		{
			return cardLeft * -3;
		}
		else if (cardLeft >= 10)
		{
			return cardLeft * -2;
		}
		else if (cardLeft > 0)
		{
			return cardLeft * -1;
		}
		else
		{
			var point = 0;
			for (int i = 0; i < controller.players.Count; ++i)
			{
				if (i != playerId)
					point += GetPoint(i);
			}
			return Mathf.Abs(point);
		}
	}

	public void Announce(PokerWin.CombinationType comb)
	{
		if (comb == PokerWin.CombinationType.One)
			return;

		StopCoroutine("Announcement");
		announcer.color = Color.white;
		switch (comb)
		{
			case PokerWin.CombinationType.Pair:
				announcer.sprite = pair;
				break;
			case PokerWin.CombinationType.Triple:
				announcer.sprite = triple;
				break;
			case PokerWin.CombinationType.Straight:
				announcer.sprite = straight;
				break;
			case PokerWin.CombinationType.Flush:
				announcer.sprite = flush;
				break;
			case PokerWin.CombinationType.FullHouse:
				announcer.sprite = fullhouse;
				break;
			case PokerWin.CombinationType.StraightFlush:
				announcer.sprite = straightFlush;
				break;
			case PokerWin.CombinationType.RoyalFlush:
				announcer.sprite = royalFlush;
				break;
			case PokerWin.CombinationType.Dragon:
				announcer.sprite = dragon;
				break;
			default:
				announcer.sprite = null;
				break;
		}
		StartCoroutine("Announcement");
	}

	IEnumerator Announcement()
	{
		float time = 3f;

		while (time > 0)
		{
			time -= Time.deltaTime;
			if (time < 2f)
			{
				var color = announcer.color;
				color.a -= Time.deltaTime / 2f;
				announcer.color = color;
			}
			yield return null;
		}
		announcer.color = Color.clear;
	}

	public void NotifyMessage(string message)
	{
		notifier.text = message;
		if (IsInvoking("ClearNotification"))
			CancelInvoke("ClearNotification");
		Invoke("ClearNotification", 2f);
	}

	void ClearNotification()
	{
		notifier.text = "";
	}

	public void OnPlayAgain()
	{
		controller.PlayAgain();
	}

	public void OnQuit()
	{
		controller.Quit();
	}
}
