using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSFX : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        audioManager.PlaySFX(audioManager.Pickupbook);
        Debug.Log("audio");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
