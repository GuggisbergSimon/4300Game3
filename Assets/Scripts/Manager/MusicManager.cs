using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	[SerializeField] private AudioClip intro=null;
	[SerializeField] private AudioClip loop=null;
	[SerializeField] private AudioClip outro=null;
	private AudioSource myAudioSourceIntro;
	private AudioSource myAudioSourceLoop;
	
	private void Start()
	{
		var audioSources = GetComponents<AudioSource>();
		myAudioSourceLoop = audioSources[0];
		myAudioSourceIntro = audioSources[1];
		ChangeAudioClipNoLoop(intro);
		StartCoroutine(ChangeAudioClipWithLoop(loop));
	}

	private void ChangeAudioClipNoLoop(AudioClip clip)
	{
		myAudioSourceIntro.clip = clip;
		myAudioSourceIntro.Play();
		myAudioSourceIntro.loop = false;
	}

	public void PlayOutro()
	{
		ChangeAudioClipNoLoop(outro);
	}
	
	private IEnumerator ChangeAudioClipWithLoop(AudioClip clip)
	{
		yield return new WaitForSeconds(myAudioSourceIntro.clip.length);
		myAudioSourceLoop.clip = clip;
		myAudioSourceLoop.Play();
		myAudioSourceLoop.loop = true;
	}
}
