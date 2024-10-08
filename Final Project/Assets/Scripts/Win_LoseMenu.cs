using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win_LoseMenu : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void cliclSound()
    {
        audioManager.PlaySFX(audioManager.button);
        Debug.Log("audio");
    }
    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu_Main");
    }
    public void Restart()
    {
        SceneManager.LoadScene("Lvl_HUB");
    }
    public void MenuQuit()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
