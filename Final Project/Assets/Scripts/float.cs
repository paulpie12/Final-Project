using UnityEngine;
using System.Collections;

public class Floater : MonoBehaviour {
public float degreesPerSecond = 15.0f;
public float amplitude = 0.5f;
public float frequency = 1f;

// Position Storage Variables
Vector3 posOffset = new Vector3 ();
Vector3 tempPos = new Vector3 ();

// Use this for initialization
void Start () 
{

posOffset = transform.position;

}

void Update () 
{
tempPos = posOffset;
tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;

transform.position = tempPos;

}

}