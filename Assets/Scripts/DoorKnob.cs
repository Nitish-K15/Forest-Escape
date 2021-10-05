using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnob : Interactable
{
    public Puzzle2Solver[] solvers = new Puzzle2Solver[4];
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    { 
        if (canOpen())
            rend.material.color = Color.green;
        else
            rend.material.color = Color.red;
    }
    private bool canOpen()  //check if all buttons match the numbers
    {
        for(int i =0;i<solvers.Length;i++)
        {
            if (solvers[i].isCorrect == false)
                return false;
        }
        return true;
    }
    public override void Interact()
    {
     if(canOpen())
     GetComponentInParent<Animator>().Play("DoorOpen");
    }
}
