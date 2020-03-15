using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.AddScore();
        animator.SetTrigger("Collision");
        Destroy(this.gameObject, 1.5f);
    } 
}
