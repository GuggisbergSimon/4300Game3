using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractive : Interactive
{
	[SerializeField] private Text text;
	private bool isInteracting = false;
	
	public override void Interact()
	{
		base.Interact();
		GameManager.Instance.MyUiManager.DisplayDialogue(text);
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
			GameManager.Instance.MyUiManager.CloseDialogue();
			GameManager.Instance.Player.CanPlayerMove = true;
			isInteracting = false;
		}

		//checks for skipping dialogue
		if (isInteracting && Input.GetButtonDown("Fire1") && text.skippable)
		{
			GameManager.Instance.MyUiManager.DisplayAllText(text.text);
		}
	}
}