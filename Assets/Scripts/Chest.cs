using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
 
public class Chest : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.AddChest();
        animator.SetTrigger("Collision");
        Destroy(this.gameObject, 1.5f);
    }
}
