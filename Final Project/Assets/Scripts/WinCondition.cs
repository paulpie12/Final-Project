using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    static bool BankWin = false;
    static bool MuseumWin = false;
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
        BankWin = true;
        Debug.Log("You have beat the level");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lvl_Hub");
    }
}