﻿/*

The MIT License (MIT)

Copyright (c) 2015-2017 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.EventSystems;

namespace Yarn.Unity.Example
{
	/// Displays dialogue lines to the player, and sends
	/// user choices back to the dialogue system.
	/** Note that this is just one way of presenting the
	 * dialogue to the user. The only hard requirement
	 * is that you provide the RunLine, RunOptions, RunCommand
	 * and DialogueComplete coroutines; what they do is up to you.
	 */
	public class ExampleDialogueUI : Yarn.Unity.DialogueUIBehaviour
	{
		/// The object that contains the dialogue and the options.
		/** This object will be enabled when conversation starts, and 
		 * disabled when it ends.
		 */
		[SerializeField] private GameObject dialogueContainer = null;

		[SerializeField] private GameObject buttonsContainer = null;

		/// The UI element that displays lines
		[SerializeField] private TextMeshProUGUI lineText = null;

		/// A UI element that appears after lines have finished appearing
		[SerializeField] private GameObject continuePrompt = null;

		/// A delegate (ie a function-stored-in-a-variable) that
		/// we call to tell the dialogue system about what option
		/// the user selected
		private Yarn.OptionChooser SetSelectedOption;

		/// How quickly to show the text, in seconds per character
		[SerializeField, Tooltip("How quickly to show the text, in seconds per character")]
		private float textSpeed = 0.025f;

		/// The buttons that let the user choose an option
		[CanBeNull] [SerializeField] private List<Button> optionButtons = null;

		/// Make it possible to temporarily disable the controls when
		/// dialogue is active and to restore them when dialogue ends
		[SerializeField] private RectTransform gameControlsContainer = null;

		[SerializeField] private AudioClip beepTalkingClip = null;
		[SerializeField] private GameObject blackGameObject = null;
		private Image blackImage;
		private EventSystem eventSystem;
		private bool isSkipping = false;
		private AudioSource myAudioSource;

		private void Awake()
		{
			// Start by hiding the container, line and option buttons
			if (dialogueContainer != null)
				dialogueContainer.SetActive(false);

			if (buttonsContainer != null)
				buttonsContainer.SetActive(false);

			lineText.gameObject.SetActive(false);

			foreach (var button in optionButtons)
			{
				button.gameObject.SetActive(false);
			}

			// Hide the continue prompt if it exists
			if (continuePrompt != null)
				continuePrompt.SetActive(false);

			myAudioSource = GetComponent<AudioSource>();
			blackImage = blackGameObject.GetComponent<Image>();
		}

		private void OnEnable()
		{
			eventSystem = EventSystem.current;
		}

		/// Show a line of dialogue, gradually
		public override IEnumerator RunLine(Yarn.Line line)
		{
			// Show the text
			lineText.gameObject.SetActive(true);

			isSkipping = false;
			if (textSpeed > 0.0f)
			{
				// Display the line one character at a time
				var stringBuilder = new StringBuilder();

				myAudioSource.clip = beepTalkingClip;
				foreach (char c in line.text)
				{
					stringBuilder.Append(c);
					lineText.text = stringBuilder.ToString();
					myAudioSource.Play();
					if (isSkipping)
					{
						lineText.text = line.text;
						yield return null;
						break;
					}

					yield return new WaitForSeconds(textSpeed);
				}
			}
			else
			{
				// Display the line immediately if textSpeed == 0
				lineText.text = line.text;
			}

			// Show the 'press any key' prompt when done, if we have one
			if (continuePrompt != null)
				continuePrompt.SetActive(true);

			// Wait for any user input
			while (!Input.GetButtonDown("Jump"))
			{
				yield return null;
			}

			// Hide the text and prompt
			lineText.gameObject.SetActive(false);
			isSkipping = false;

			if (continuePrompt != null)
				continuePrompt.SetActive(false);
		}

		/// Show a list of options, and wait for the player to make a selection.
		public override IEnumerator RunOptions(Yarn.Options optionsCollection,
			Yarn.OptionChooser optionChooser)
		{
			// Do a little bit of safety checking
			if (optionsCollection.options.Count > optionButtons.Count)
			{
				Debug.LogWarning("There are more options to present than there are" +
				                 "buttons to present them in. This will cause problems.");
			}

			lineText.gameObject.SetActive(true);
			buttonsContainer.SetActive(true);
			// Display each option in a button, and make it visible
			int i = 0;
			foreach (var optionString in optionsCollection.options)
			{
				optionButtons[i].gameObject.SetActive(true);
				optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = optionString;
				i++;
			}

			//todo find more elegant solution
			eventSystem.SetSelectedGameObject(optionButtons[4].gameObject);
			eventSystem.SetSelectedGameObject(optionButtons[0].gameObject);

			// Record that we're using it
			SetSelectedOption = optionChooser;

			// Wait until the chooser has been used and then removed (see SetOption below)
			while (SetSelectedOption != null)
			{
				yield return null;
			}

			lineText.gameObject.SetActive(false);
			buttonsContainer.SetActive(false);

			// Hide all the buttons
			foreach (var button in optionButtons)
			{
				button.gameObject.SetActive(false);
			}
		}

		/// Called by buttons to make a selection.
		public void SetOption(int selectedOption)
		{
			// Call the delegate to tell the dialogue system that we've
			// selected an option.
			SetSelectedOption(selectedOption);

			// Now remove the delegate so that the loop in RunOptions will exit
			SetSelectedOption = null;
		}

		/// Run an internal command.
		public override IEnumerator RunCommand(Yarn.Command command)
		{
			// "Perform" the command
			Debug.Log("Command: " + command.text);

			yield break;
		}

		/// Called when the dialogue system has started running.
		public override IEnumerator DialogueStarted()
		{
			// Enable the dialogue controls.
			if (dialogueContainer != null)
				dialogueContainer.SetActive(true);

			// Hide the game controls.
			if (gameControlsContainer != null)
			{
				gameControlsContainer.gameObject.SetActive(false);
			}

			yield break;
		}

		/// Called when the dialogue system has finished running.
		public override IEnumerator DialogueComplete()
		{
			// Hide the dialogue interface.
			if (dialogueContainer != null)
				dialogueContainer.SetActive(false);

			// Show the game controls.
			if (gameControlsContainer != null)
			{
				gameControlsContainer.gameObject.SetActive(true);
			}

			yield break;
		}

		private void Update()
		{
			if (GameManager.Instance.MyUiManager.DialogueRunner.isDialogueRunning && Input.GetButtonDown("Jump"))
			{
				isSkipping = true;
			}
		}

		[YarnCommand("FadeToBlack")]
		public void FadeToBlack(string boolean)
		{
			if (Boolean.Parse(boolean))
			{
				blackImage.color = Color.black;
			}
			else
			{
				blackImage.color = Color.clear;
			}
		}

		[YarnCommand("MovePlayer")]
		public void MovePlayer(string position)
		{
			GameManager.Instance.Player.transform.position = GameObject.Find(position).transform.position;
		}

		[YarnCommand("LoadScene")]
		public void LoadScene(string sceneName)
		{
			GameManager.Instance.LoadLevel(sceneName);
		}

		[YarnCommand("ChangePitch")]
		public void ChangePitch(string pitch)
		{
			myAudioSource.pitch = (float) Convert.ToDouble(pitch);
		}
	}
}