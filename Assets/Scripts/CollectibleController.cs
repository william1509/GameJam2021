using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : NPCController
{
    public GameManager.Ability ability;

    private bool isDropped_;



    void Start() { Initialize(); }



    new public void StopInteraction(PlayerController character)
    {
        base.StopInteraction(character);
        gameObject.SetActive(false);
    }



    new public virtual void SetVisibility(GameManager.State dimension)
    {
        if (isDropped_)
            base.SetVisibility(dimension);
    }
    public void Drop() { visible_ = true; isDropped_ = true; }



    new public virtual void Interact(PlayerController character)
    {
        base.Interact(character);
        character.GiveAbility(ability);
    }
}
