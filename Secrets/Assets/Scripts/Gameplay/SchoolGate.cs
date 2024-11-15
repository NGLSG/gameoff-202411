using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SchoolGate : MonoBehaviour
{
    public GameObject confirmBox;
    private void OnTriggerEnter2D(Collider2D other)
    {
        confirmBox.SetActive(true);
        Debug.Log("Player Touch the Door");
    }
}
