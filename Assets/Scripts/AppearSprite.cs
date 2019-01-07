using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AppearSprite : MonoBehaviour
{
	private SpriteRenderer myChildrenSpriteRenderer;
	
	private void Start()
	{
		myChildrenSpriteRenderer=GetComponentInChildren<SpriteRenderer>();
	}

	[YarnCommand("EnableSprite")]
	public void EnableSprite(string boolean)
	{
		if (Boolean.Parse(boolean))
		{
			myChildrenSpriteRenderer.color=Color.white;
		}
		else
		{
			myChildrenSpriteRenderer.color = Color.clear;
		}
	}
}
