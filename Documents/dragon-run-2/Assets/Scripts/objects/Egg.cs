using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        SoundManager.PlaySound("Special");
        animator.SetTrigger("Collision");
        GameManager.Instance.AddEgg();
        Destroy(this.gameObject, 1.5f);
    } 
}
