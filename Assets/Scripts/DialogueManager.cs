using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
	public AudioSource keysound;
	public Text nameText;
	public Text dialogueText;

	public Animator animator;

	private Queue<string> sentences;

	private bool printing_ = false;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
	}

	public void StartDialogue (Dialogue dialogue)
	{
		animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (printing_)
			printing_ = false;
		else
        {
			if (sentences.Count == 0)
			{
				EndDialogue();
				return;
			}

			string sentence = sentences.Dequeue();
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentence));
		}
	}

	IEnumerator TypeSentence (string sentence)
	{
		 keysound.Play();
		dialogueText.text = "";
		printing_ = true;
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			if (printing_)
				yield return new WaitForSeconds(0.1F);
		}
		printing_ = false;
		keysound.Stop();
	}

	public void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
		
	}

}
