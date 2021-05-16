using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    DialogueManager dialogueManager;
    public Dialogue dialogue;

    bool interacting_ = false;



    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void StopInteraction()
    {
        dialogueManager.EndDialogue();
        interacting_ = false;
    }



    public void Interact()
    {
        if (interacting_)
            dialogueManager.DisplayNextSentence();
        else
        {
            dialogueManager.StartDialogue(dialogue);
            interacting_ = true;
        }
    }
}
