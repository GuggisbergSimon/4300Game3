using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAgitator : MonoBehaviour
{
	private Rigidbody2D myRigidBody2D;
	[SerializeField, Range(0, 10)] private float minTime;
	[SerializeField, Range(0, 10)] private float maxTime;
	[SerializeField] private GameObject leftBorder;
	[SerializeField] private GameObject rightBorder;
	
	private void Start()
	{
		myRigidBody2D = GetComponent<Rigidbody2D>();
		StartCoroutine(Agitate());
	}

	private IEnumerator Agitate()
	{
		for (;;)
		{
			transform.position = (new Vector2(
				Random.Range(leftBorder.transform.position.x, rightBorder.transform.position.x),
				Random.Range(leftBorder.transform.position.y, rightBorder.transform.position.y)));
			myRigidBody2D.velocity = Vector2.zero;
			yield return new WaitForSeconds(Random.Range(minTime, maxTime));
		}
	}
}
