using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDialogue : Interactive
{
	[SerializeField] private Text[] text=null;
	private bool isInteracting = false;
	private int currentTextIndex;

	public override void Interact()
	{
		base.Interact();
		GameManager.Instance.Player.MyState = PlayerController.PlayerState.Interacting;
		currentTextIndex = 0;
		GameManager.Instance.MyUiManager.DisplayDialogue(text[currentTextIndex]);
		StartCoroutine(InteractingNextFrame());
	}

	//TODO replace those two next for next frame methods by something more elegant
	private IEnumerator InteractingNextFrame()
	{
		yield return new WaitForEndOfFrame();
		isInteracting = true;
	}

	private IEnumerator IdleNextFrame()
	{
		yield return new WaitForEndOfFrame();
		GameManager.Instance.Player.MyState = PlayerController.PlayerState.Idle;
	}

	private void Update()
	{
		//checks for closing dialogue panel
		if (isInteracting && Input.GetButtonDown("Fire1") && !GameManager.Instance.MyUiManager.IsPrinting)
		{
			isInteracting = false;
			if (currentTextIndex == text.Length - 1)
			{
				GameManager.Instance.MyUiManager.CloseDialogue();
				StartCoroutine(IdleNextFrame());
			}
			else
			{
				currentTextIndex++;
				GameManager.Instance.MyUiManager.DisplayDialogue(text[currentTextIndex]);
				StartCoroutine(InteractingNextFrame());
			}
		}

		//checks for skipping dialogue
		if (isInteracting && Input.GetButtonDown("Fire1") && text[currentTextIndex].skippable &&
		    GameManager.Instance.MyUiManager.IsPrinting)
		{
			GameManager.Instance.MyUiManager.DisplayAllText(text[currentTextIndex].text);
		}
	}
}