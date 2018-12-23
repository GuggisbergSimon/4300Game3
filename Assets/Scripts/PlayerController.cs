using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float fallMultiplier = 2.5f;
	[SerializeField] [Range(1, 10)] private float jumpSpeed = 5.0f;
	[SerializeField] private float lowJumpMultiplier = 2.0f;
	[SerializeField] [Range(1, 10)] private float speed = 5.0f;
	private bool hasPressedJump;
	private bool isAirborne;
	private bool canPlayerMove = true;
	private Rigidbody2D myRigidbody2D;
	private List<GameObject> interactives = new List<GameObject>();

	public bool CanPlayerMove
	{
		get => canPlayerMove;
		set => canPlayerMove = value;
	}
	
	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if (canPlayerMove)
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
	}

	private void Update()
	{
		if (canPlayerMove)
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

			if ((interactives.Count > 0 && Input.GetButtonDown("Fire1") || Input.GetAxis("Vertical") > 0) &&
			    !isAirborne)
			{
				GameObject closestToPlayer = interactives[0];
				foreach (var item in interactives)
				{
					if ((closestToPlayer.transform.position - transform.position).magnitude >
					    (item.transform.position - transform.position).magnitude)
					{
						closestToPlayer = item;
					}
				}

				//TODO add check if player is in state where he can interact (i.e. not in a menu nor while reading already a dialogue, idk)
				closestToPlayer.GetComponent<Interactive>().Interact();
				canPlayerMove = false;
				myRigidbody2D.velocity = Vector2.zero;
			}
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Interactive") && !interactives.Contains(other.gameObject))
		{
			interactives.Add(other.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Interactive") && interactives.Contains(other.gameObject))
		{
			interactives.Remove(other.gameObject);
		}
	}
}