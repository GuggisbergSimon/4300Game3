using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractive : Interactive
{
	[SerializeField] private Text[] text;
	private bool isInteracting = false;
	private int currentTextIndex;
	
	public override void Interact()
	{
		base.Interact();
		currentTextIndex = 0;
		GameManager.Instance.MyUiManager.DisplayDialogue(text[currentTextIndex]);
		StartCoroutine(IsInteractingNextFrame());
	}

	private IEnumerator IsInteractingNextFrame()
	{
		yield return new WaitForEndOfFrame();
		isInteracting = true;
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
				GameManager.Instance.Player.CanPlayerMove = true;
			}
			else
			{
				currentTextIndex++;
				GameManager.Instance.MyUiManager.DisplayDialogue(text[currentTextIndex]);
				StartCoroutine(IsInteractingNextFrame());
			}
		}

		//checks for skipping dialogue
		if (isInteracting && Input.GetButtonDown("Fire1") && text[currentTextIndex].skippable && GameManager.Instance.MyUiManager.IsPrinting)
		{
			GameManager.Instance.MyUiManager.DisplayAllText(text[currentTextIndex].text);
		}
	}
}