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

    


    public void GoToMainMenu() 
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        AboutMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void GoToPauseMenu()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        AboutMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void GoToOptions()
    {
        MainMenu.SetActive(false);
        PauseMenu.SetActive(false);
        AboutMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void GoToAboutMenu()
    {
        MainMenu.SetActive(false);
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        AboutMenu.SetActive(true);
    }

    public void PlayGame()
    {
        MainMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("The Game Quit");
    }

    public void DoubleSpeed()
    {
       PlayerMovement.doubleSpeed = !PlayerMovement.doubleSpeed;
    }

    public void DoubleJumpHeight()
    {
        PlayerMovement.doubleJumpHeight = !PlayerMovement.doubleJumpHeight;
    }
}
