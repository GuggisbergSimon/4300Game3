using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractive : Interactive
{
	[SerializeField] private Text item;
	
	public override void Interact()
	{
		base.Interact();
		//TODO actual print that on the UI screen
		Debug.Log(item.text);
	}
}
