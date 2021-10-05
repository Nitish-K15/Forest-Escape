using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Escape : Interactable
{
    public override void Interact()
    {
        SceneManager.LoadScene("Finish");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
