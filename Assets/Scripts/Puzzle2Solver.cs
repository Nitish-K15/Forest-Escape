using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Puzzle2Solver : Interactable
{
    Color[] colour = { Color.red, Color.yellow, Color.white, Color.green };
    public bool isCorrect;
    public Text number;
    private Renderer button;
    private int i = 0;

    private void Start()
    {
        button = GetComponent<Renderer>();
    }
    public override void Interact()
    {
        if (i < colour.Length)
        {
            button.material.color = colour[i];
            i++;

        }
        if (i == 4)
        {
            i = 0;
        }
    }

    private void Update()
    {
        if(number.color == button.material.color)
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }
    }
}
