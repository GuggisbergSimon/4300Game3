using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		Rigidbody2D otherRigidbody2D = other.GetComponent<Rigidbody2D>();

		if (otherRigidbody2D != null)
		{
			transform.parent.GetComponent<Water>().Splash(transform.position.x,
				otherRigidbody2D.velocity.y * otherRigidbody2D.mass / 40.0f);
		}
	}
}