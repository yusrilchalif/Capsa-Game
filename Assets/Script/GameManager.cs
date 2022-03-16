using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SelectAvatar))]
public class GameManager : MonoBehaviour
{
    public int avatarPlayerId = 0;
    public PlayerEmotion.AvatarSet[] avatars;
    public Sprite[] rawAvatar;

    SelectAvatar selectAvatar;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
                if (instance == null)
                    Debug.Log("Game Manager is Missing");
            }
            return instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        selectAvatar = GetComponent<SelectAvatar>();

        if (rawAvatar.Length % 3 != 0)
            Debug.LogError("Avatar not enough");
        avatars = new PlayerEmotion.AvatarSet[rawAvatar.Length / 3];
        int id = 0;
        for (int i = 0; i < rawAvatar.Length; i += 3)
        {
            avatars[id] = new PlayerEmotion.AvatarSet();
            avatars[id].normal = rawAvatar[i += 0];
            avatars[id].happy = rawAvatar[i += 1];
            avatars[id].sad = rawAvatar[i += 2];
            ++id;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        selectAvatar.OnMenuSceneLoad();
    }

    public void Play()
    {
        Application.LoadLevel("Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
