using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Text : ScriptableObject
{
	[TextArea(1,10)] public string text;
	public bool skippable = false;
	public float timeBetweenLetters = 0.0f;
	public Color colorText;
}