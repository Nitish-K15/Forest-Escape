using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Puzzle2 : MonoBehaviour
{
    private Text text;
    Color[] colour = { Color.red, Color.yellow, Color.white, Color.green};
    void Start()
    {
        text = GetComponent<Text>();
        int num = Random.Range(0, 3);
        text.color = colour[num];
    }

  
}
