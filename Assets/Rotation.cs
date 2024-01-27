using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationAmount;
    void Update()
    {
        transform.Rotate(Vector3.forward,Time.deltaTime*rotationAmount);
        rotationAmount += Time.deltaTime;
    }
}
