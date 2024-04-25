using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    //This script tracks your progression through the game, allowing you to win and lose each level, and ultimately the game
    static bool BankWin = false;
    static bool MuseumWin = false;
    static int MuseumPaintingsStolen = 0;
    static bool WhiteHouseWin = false;


    public void Update()
    {
        if (BankWin == true && MuseumWin == true && WhiteHouseWin == true)
        {
            LoadWinScene();
        }
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("Menu_Win");
    }

    public void OnTriggerEnter(Collider other)
    {
        //This set of code teleports you in the hub
        if (other.CompareTag("BankTP"))
        {
            //lets the code actually run inbbetween scenes
            DontDestroyOnLoad(this.gameObject);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lvl_Bank");
            //This also triggers the loss condition for the bank level
            Invoke("BankDeathTimer", 60);
            Debug.Log("The invoke invoked");
        }
        if (other.CompareTag("MuseumTP"))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lvl_Museum"); 
        }
        if (other.CompareTag("WhiteHouseTP"))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lvl_WhiteHouse");
        }

        //This set of code allows you to win each level
        if (other.CompareTag("BankWin"))
        {
            BankWin = true;
            Debug.Log("You have beat the Bank");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lvl_Hub");
        }
        if (other.CompareTag("WhiteHouseWin"))
        {
            WhiteHouseWin = true;
            Debug.Log("You have beat the White House");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lvl_Hub");
        }
        if (other.CompareTag("MuseumPaintingsStolen"))
        {
            MuseumPaintingsStolen = MuseumPaintingsStolen + 1;
            Debug.Log("You have stolen a painting");
            Destroy(other.gameObject);
            if (MuseumPaintingsStolen == 10)
            {
                MuseumWin = true;
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lvl_Hub");
            }
        }

        //This set of code makes you lose each level
        if (other.CompareTag("Guard"))
        {
            Debug.Log("You have lost the Museum");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu_Lose");
        }

        if (other.CompareTag("Camera"))
        {
            Debug.Log("You have lost the WhiteHouse");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu_Lose");
        }
    }
    public void BankDeathTimer()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log("The current scene is " + currentScene.name);
        if (currentScene.name == "Lvl_Bank")
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu_Lose");
        }
    }
}