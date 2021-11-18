using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterPad : MonoBehaviour
{
    [SerializeField]
    private float turboAmount;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        other.GetComponent<Rigidbody>().AddForce(transform.forward * turboAmount);

    }
}