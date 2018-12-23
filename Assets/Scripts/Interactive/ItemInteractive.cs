using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractive : Interactive
{
	[SerializeField] private Text text;

	public override void Interact()
	{
		base.Interact();
		GameManager.Instance.MyUiManager.DisplayDialogue(text);
	}
}