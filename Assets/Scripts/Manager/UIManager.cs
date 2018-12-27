using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject dialoguePanel=null;
	[SerializeField] private TextMeshProUGUI dialogueText=null;
	private bool isPrinting = false;
	public bool IsPrinting => isPrinting;
	private IEnumerator displayCoroutine;
	
	public void DisplayDialogue(Text text)
	{
		dialogueText.text = "";
		dialogueText.color = text.colorText;
		dialoguePanel.SetActive(true);

		if (text.timeBetweenLetters.CompareTo(0) != 0)
		{
			displayCoroutine = DisplayLetterByLetter(text.text, text.timeBetweenLetters);
			StartCoroutine(displayCoroutine);
		}
		else
		{
			dialogueText.text = text.text;
		}
	}

	public void CloseDialogue()
	{
		dialoguePanel.SetActive(false);
	}
	
	public void DisplayAllText(string text)
	{
		
		StopCoroutine(displayCoroutine);
		dialogueText.text = "";
		dialogueText.text = text;
		isPrinting = false;
	}

	private IEnumerator DisplayLetterByLetter(string text, float time)
	{
		isPrinting = true;
		foreach (var character in text)
		{
			dialogueText.text += character;
			yield return new WaitForSeconds(time);
		}

		isPrinting = false;
	}
}