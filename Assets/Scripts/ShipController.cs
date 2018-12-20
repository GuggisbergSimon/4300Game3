using UnityEngine;

public class ShipController : MonoBehaviour
{
	private Rigidbody2D myRigidbody2D;
	[SerializeField] private float speed = 2.0f;

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		myRigidbody2D.velocity = myRigidbody2D.velocity.normalized * speed;
	}
}