using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeLeft = 60.0f;
    public Text timer;
    public Text losescreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        losescreen.enabled = false;
        timeLeft -= Time.deltaTime;
        timer.text = (timeLeft).ToString("Time left:"+"0");
        if (timeLeft < 0)
        {
            losescreen.enabled = true;
        }
    }
}
