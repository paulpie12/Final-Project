using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigator : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject AboutMenu;
    public GameObject CreditsMenu;
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


    public void GoToMainMenu() 
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        AboutMenu.SetActive(false);
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }

     public void GoToCreditsMenu() 
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        AboutMenu.SetActive(false);
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void GoToPauseMenu()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        AboutMenu.SetActive(false);
        PauseMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }

    public void GoToOptions()
    {
        MainMenu.SetActive(false);
        PauseMenu.SetActive(false);
        AboutMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }

    public void GoToAboutMenu()
    {
        MainMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        AboutMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Lvl_HUB");
    }
    public void MainMenuoutofMM()
    {
        SceneManager.LoadScene("Menu_Main");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("The Game Quit");
    }

    public void DoubleSpeed()
    {
       PlayerMovement.doubleSpeed = !PlayerMovement.doubleSpeed;
        Debug.Log("Doublespeed");
    }

    public void DoubleJumpHeight()
    {
        PlayerMovement.doubleJumpHeight = !PlayerMovement.doubleJumpHeight;
        Debug.Log("DoubleJumpHeight");
    }
}
