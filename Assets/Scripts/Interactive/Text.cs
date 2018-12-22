using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Text : ScriptableObject
{
	public string text;
	public bool skippable = false;
	public float timeBetweenLetters = 0.0f;
	public Color colorText;
}
