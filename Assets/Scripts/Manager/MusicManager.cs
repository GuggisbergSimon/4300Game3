using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	[SerializeField] private AudioClip intro;
	[SerializeField] private AudioClip loop;
	[SerializeField] private AudioClip outro;
	private AudioSource myAudioSource;

	private void Start()
	{
		myAudioSource = GetComponent<AudioSource>();
		ChangeAudioClipNoLoop(intro);
		StartCoroutine(ChangeAudioClipWithLoop(loop));
	}

	private void ChangeAudioClipNoLoop(AudioClip clip)
	{
		myAudioSource.clip = clip;
		myAudioSource.Play();
		myAudioSource.loop = false;
	}

	public void PlayOutro()
	{
		ChangeAudioClipNoLoop(outro);
	}
	
	private IEnumerator ChangeAudioClipWithLoop(AudioClip clip)
	{
		yield return new WaitForSeconds(myAudioSource.clip.length);
		ChangeAudioClipNoLoop(clip);
		myAudioSource.loop = true;
	}
}
