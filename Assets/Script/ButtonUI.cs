using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public GameObject UIPanel;

    // Start is called before the first frame update
    void Start()
    {
        DisablePanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Change Scene to " + sceneName);
    }

    public void ActivePanel()
    {
        UIPanel.SetActive(true);
    }

    public void DisablePanel()
    {
        UIPanel.SetActive(false);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Quit App");
    }
}
