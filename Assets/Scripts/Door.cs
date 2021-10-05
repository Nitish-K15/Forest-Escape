using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private Animator anim;
    public FirstPersonController player;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void Interact()
    {
        if(player.hasKey)
        {
            anim.Play("DoorSliding");
        }
    }

}
