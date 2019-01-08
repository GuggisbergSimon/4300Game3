using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class UIManager : MonoBehaviour
{
	[SerializeField] private DialogueRunner dialogueRunner;
	public DialogueRunner DialogueRunner => dialogueRunner;

	public void Setup()
	{
		dialogueRunner = FindObjectOfType<DialogueRunner>();
	}

	public bool isDialogueRunning()
	{
		if (dialogueRunner != null)
		{
			return dialogueRunner.isDialogueRunning;
		}
		else
		{
			return false;
		}
	}


	private void Awake()
	{
		Setup();
	}
}