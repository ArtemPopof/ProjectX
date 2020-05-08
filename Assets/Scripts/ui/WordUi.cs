using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordUi : MonoBehaviour
{
    public  Animator animator;

    public  Text d, r, a, g, o, n;

   public static WordUi Instance { private set; get; }


    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerWordUi(string letter) 
    {
        switch(letter) {
            case "D":
            SetBehaviourForLetter(d);
            break;
            case "R":
            SetBehaviourForLetter(r);
            break;
            case "A":
            SetBehaviourForLetter(a);
            break;
            case "G":
            SetBehaviourForLetter(g);
            break;
            case "O":
            SetBehaviourForLetter(o);
            break;
            case "N":
            SetBehaviourForLetter(n);
            break;
        }
    }

    public void ReturnDefaultColor(List<string> letters) 
    {
        foreach(string letter in letters) {
            SetDefaultColor(letter);    
        }
    }

    private void SetBehaviourForLetter(Text letter)
    {
            letter.color = new Color32(214, 231, 30, 255);
            animator.SetTrigger("Show");
    }

    private void SetDefaultColor(string letter)
    {
         switch(letter) {
            case "D":
            d.color = new Color32(111, 96, 96, 255);
            break;
            case "R":
            r.color = new Color32(111, 96, 96, 255);
            break;
            case "A":
            a.color = new Color32(111, 96, 96, 255);
            break;
            case "G":
            g.color = new Color32(111, 96, 96, 255);
            break;
            case "O":
            o.color = new Color32(111, 96, 96, 255);
            break;
            case "N":
            n.color = new Color32(111, 96, 96, 255);
            break;
        }
    }

}
