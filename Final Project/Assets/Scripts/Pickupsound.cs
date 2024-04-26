using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupsound : MonoBehaviour
{
    AudioSource PickUp;

    // Start is called before the first frame update
    void Start()
    {
        PickUp = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            PickUp.Play();
            Debug.Log("audio");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
