using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAvatar : MonoBehaviour
{
    public Image P1, P2, P3, P4;

    GameManager gameManager;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMenuSceneLoad()
    {
        P1.sprite = gameManager.avatars[0].normal;
        P2.sprite = gameManager.avatars[1].normal;
        P3.sprite = gameManager.avatars[2].normal;
        P4.sprite = gameManager.avatars[3].normal;
    }

    public void OnPlayer1Selected()
    {
        gameManager.avatarPlayerId = 0;
        gameManager.Play();
    }

    public void OnPlayer2Selected()
    {
        gameManager.avatarPlayerId = 1;
        gameManager.Play();
    }

    public void OnPlayer3Selected()
    {
        gameManager.avatarPlayerId = 2;
        gameManager.Play();
    }

    public void OnPlayer4Selected()
    {
        gameManager.avatarPlayerId = 3;
        gameManager.Play();
    }

    public void OnPlayAgain()
    {
        gameManager.Play();
    }
}
