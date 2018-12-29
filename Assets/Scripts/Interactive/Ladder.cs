using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactive
{

	public override void Interact()
	{
		base.Interact();
		GameManager.Instance.Player.MyState = PlayerController.PlayerState.Climbing;
	}
}