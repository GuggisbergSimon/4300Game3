﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject dialoguePanel;
	[SerializeField] private TextMeshProUGUI dialogueText;
	private bool isPrinting = false;
	private bool isCurrentTextSkippable = true;
	private float currentTimeBetweenLetters = 0.0f;

	public void DisplayDialogue(Text text)
	{
		dialogueText.text = "";
		dialogueText.color = text.colorText;
		isCurrentTextSkippable = text.skippable;
		dialoguePanel.SetActive(true);
		//TODO actually display message instantly if time is 0 (did not work for closing dialogue)
		currentTimeBetweenLetters = text.timeBetweenLetters;
		StartCoroutine(DisplayLetterByLetter(text.text));

	}

	private void Update()
	{
		//checks for closing dialogue panel
		if (dialoguePanel.activeSelf && !isPrinting && Input.GetButtonDown("Fire1"))
		{
			dialoguePanel.SetActive(false);
			GameManager.Instance.Player.CanPlayerMove = true;
		}

		//checks for skipping dialogue
		if (isPrinting && Input.GetButtonDown("Fire1") && isCurrentTextSkippable)
		{
			currentTimeBetweenLetters = 0.0f;
		}
	}

	private IEnumerator DisplayLetterByLetter(string text)
	{
		isPrinting = true;
		foreach (var character in text)
		{
			dialogueText.text += character;
			yield return new WaitForSeconds(currentTimeBetweenLetters);
		}

		isPrinting = false;
	}
}