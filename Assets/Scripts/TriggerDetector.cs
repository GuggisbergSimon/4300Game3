using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
	private List<GameObject> others=new List<GameObject>();
	private bool isTriggered = false;

	public bool IsTriggered => isTriggered;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!others.Contains(other.gameObject))
		{
			others.Add(other.gameObject);
			isTriggered = true;
			Debug.Log(others.Count);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (others.Contains(other.gameObject))
		{
			others.Remove(other.gameObject);
			if (others.Count == 0)
			{
				isTriggered = false;
			}
			Debug.Log(others.Count);
		}
	}
}