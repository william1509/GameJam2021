using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorController : NPCController
{
    public CollectibleController droppedItem;



    void Start() { Initialize(); }



    new public void StopInteraction(PlayerController character)
    {
        base.StopInteraction(character);
        canInteract = false;

        GetComponent<Collider2D>().enabled = false;
        Animate("survivor_dying");
        droppedItem.gameObject.SetActive(true);
        droppedItem.Drop();

        ShowQButton(false);
    }



    new public virtual void Interact(PlayerController character) { base.Interact(character); }



    private void Animate(string clipName)
    {
        GetComponent<Animator>().Play(clipName);
    }
}
