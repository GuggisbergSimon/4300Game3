using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float fallMultiplier = 2.5f;
	[SerializeField] [Range(1, 10)] private float jumpSpeed = 5.0f;
	[SerializeField] private float lowJumpMultiplier = 2.0f;
	[SerializeField] [Range(1, 10)] private float speed = 5.0f;
	private bool hasPressedJump;
	private bool isAirborne;
	private Rigidbody2D myRigidbody2D;

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if (hasPressedJump)
		{
			myRigidbody2D.velocity = Vector2.up * jumpSpeed;
			hasPressedJump = false;
		}

		//code from "better jumping with 4 lines of code"
		if (myRigidbody2D.velocity.y < 0)
		{
			myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		else if (myRigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}

		var inputHorizontal = Input.GetAxis("Horizontal");
		myRigidbody2D.velocity = Vector2.right * speed * inputHorizontal + myRigidbody2D.velocity * Vector2.up;
	}

	private void Update()
	{
		if (Input.GetButtonDown("Jump") && !isAirborne)
		{
			hasPressedJump = true;
			isAirborne = true;
		}
		else if (Input.GetButtonUp("Jump"))
		{
			hasPressedJump = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		//TODO add condition for no wall jumping
		if (other.gameObject.CompareTag("Ground"))
		{
			isAirborne = false;
		}
	}

	//TODO not interacting properly, latency, some inputs not received :-/
	private void OnTriggerStay2D(Collider2D other)
	{
		if ((Input.GetButtonDown("Fire1") || Input.GetAxis("Vertical") > 0) &&
		    other.gameObject.CompareTag("Interactive") && !isAirborne)
		{
			other.gameObject.GetComponent<Interactive>().Interact();
		}
	}
}