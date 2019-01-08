using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDialogue : Interactive
{
	[SerializeField] private string talkToNode = "";
	public override void Interact()
	{
		base.Interact();
		GameManager.Instance.MyUiManager.DialogueRunner.StartDialogue(talkToNode);
	}
}