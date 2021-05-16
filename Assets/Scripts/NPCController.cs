using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    DialogueManager dialogueManager;
    public GameObject QButton;

    public bool freezePlayerOnInteract;
    public bool canFlip;
    public bool canInteract = true;
    public Dialogue dialogue;
    private GameManager.State dimension_;

    bool interacting_ = false;
    protected bool visible_;



    void Start() { Initialize(); }

    protected void Initialize() {  dialogueManager = FindObjectOfType<DialogueManager>(); }



    public void ShowQButton(bool show) { QButton.SetActive(show); }



    public virtual void SetVisibility(GameManager.State dimension)
    {
        if (dimension_ == dimension)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
            visible_ = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            visible_ = false;
        }
    }



    public void SetDimension(GameManager.State dimension) { dimension_ = dimension; }
    public GameManager.State GetDimension() { return dimension_; }



    public virtual void StopInteraction(PlayerController character)
    {
        if (canInteract)
        {
            dialogueManager.EndDialogue();
            interacting_ = false;

            if (freezePlayerOnInteract)
                character.Freeze(false);
        }
    }



    public virtual void Interact(PlayerController character)
    {
        if (canInteract && visible_)
        {
            ShowQButton(false);

            if (freezePlayerOnInteract)
            character.Freeze(true);

            if (interacting_)
            {
                if (dialogueManager.DisplayNextSentence())
                {
                    ShowQButton(true);

                    if (GetType() == typeof(CollectibleController))
                        (this as CollectibleController).StopInteraction(character);
                    else if (GetType() == typeof(SurvivorController))
                        (this as SurvivorController).StopInteraction(character);
                    else
                        StopInteraction(character);
                }
            }
            else
            {
                dialogueManager.StartDialogue(dialogue);
                interacting_ = true;
            }
        }
    }
}
