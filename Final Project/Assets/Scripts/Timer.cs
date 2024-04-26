using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeLeft = 60.0f;
    public Text timer;
    static bool BankWin = false;


    // Update is called once per frame
    void Update()
    {


        timeLeft -= Time.deltaTime;
        timer.text = (timeLeft).ToString("Time left: "+"0");
        if (timeLeft < 0)
       {
            SceneManager.LoadScene("Menu_Lose");
        }
    }
}
