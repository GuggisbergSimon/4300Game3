using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactive
{
	public string scene;

	public override void Interact()
	{
		base.Interact();
		GameManager.Instance.LoadLevel(scene);
	}
}