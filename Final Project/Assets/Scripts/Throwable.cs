using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform orientation;
    public Transform attackPoint;
    public GameObject objectToThrow;
    

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    //public KeyCode throwKey = KeyCode.E; - MOVED TO NEW INPUTCONTROLS SCRIPT
    public float throwForce;
    public float throwUpwardForce;

    //This bool checks if the distraction exists, which is used in the navmesh movement
    static public bool doesDistractionExist;
    bool readyToThrow;
    //private float timer = 3;

    private void Start()
    {
        readyToThrow = true;
        doesDistractionExist = false;
    }

    private void Update()
    {
         if(InputControls.getThrow() && readyToThrow && totalThrows > 0) 
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;

        //This instantiates and object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);
        doesDistractionExist = true;
        Destroy(projectile, 5);
        Invoke("removeDistractionObject", 5);

        //get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        //calculate direction
        Vector3 forceDirection = orientation.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        //implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void removeDistractionObject()
    {
        doesDistractionExist = false;
    }

}
