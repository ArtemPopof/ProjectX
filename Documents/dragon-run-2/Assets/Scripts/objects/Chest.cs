using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
 
public class Chest : MonoBehaviour
{
    public Animator animator;
    public GameObject closed;
    public GameObject open;
    public GameObject fx;
    public Boolean canBeCollected = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeCollected) return;
        GameManager.Instance.AddChest();
        animator.SetTrigger("Collision");
        Destroy(this.gameObject, 1.5f);
    }

    public void OpenChest()
    {
        closed.SetActive(false);
        open.SetActive(true);
        fx.SetActive(true);
    }
}
