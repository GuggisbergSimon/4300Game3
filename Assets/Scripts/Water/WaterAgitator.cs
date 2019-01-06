using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAgitator : MonoBehaviour
{
	private Rigidbody2D myRigidBody2D;
	[SerializeField, Range(0, 10)] private float minTime=0.0f;
	[SerializeField, Range(0, 10)] private float maxTime=0.0f;
	[SerializeField] private GameObject leftBorder=null;
	[SerializeField] private GameObject rightBorder=null;
	
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
