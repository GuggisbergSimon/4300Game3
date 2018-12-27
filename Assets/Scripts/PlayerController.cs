using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float fallMultiplier = 2.5f;
	[SerializeField] private float lowJumpMultiplier = 2.0f;
	[SerializeField, Range(1, 10)] private float jumpSpeed = 5.0f;
	[SerializeField, Range(1, 10)] private float speed = 5.0f;
	[SerializeField, Range(1, 10)] private float climbingSpeed = 2.0f;
	[SerializeField] private GameObject groundDetector = null;
	private TriggerDetector groundDetectorTrigger;
	private bool hasPressedJump;
	private bool isAirborne;
	private Rigidbody2D myRigidbody2D;
	private List<GameObject> interactives = new List<GameObject>();
	private float horizontalInput;
	private float verticalInput;

	public enum PlayerState
	{
		Idle,
		Climbing,
		Interacting,
		Walking,
		Jumping,
		Falling
	}

	private PlayerState myState;

	public PlayerState MyState
	{
		get => myState;
		set
		{
			myState = value;
			switch (myState)
			{
				case PlayerState.Climbing:
				{
					myRigidbody2D.gravityScale = 0.0f;
					break;
				}

				case PlayerState.Interacting:
				{
					break;
				}

				default:
				{
					myRigidbody2D.gravityScale = 1.0f;
					break;
				}
			}
		}
	}

	private void Start()
	{
		groundDetectorTrigger = groundDetector.GetComponent<TriggerDetector>();
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		switch (MyState)
		{
			case PlayerState.Climbing:
			{
				myRigidbody2D.velocity = Vector2.up * verticalInput * climbingSpeed;
				break;
			}
			
			case PlayerState.Interacting:
			{
				break;
			}

			default:
			{
				//launch the initial jump
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
					myRigidbody2D.velocity +=
						Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
				}

				myRigidbody2D.velocity = Vector2.right * speed * horizontalInput + myRigidbody2D.velocity * Vector2.up;
				break;
			}
		}
	}

	private void Update()
	{
		switch (MyState)
		{
			case PlayerState.Climbing:
			{
				//updates horizontal and vertical input
				horizontalInput = Input.GetAxis("Horizontal");
				verticalInput = Input.GetAxis("Vertical");
				if (horizontalInput.CompareTo(0) != 0)
				{
					myState = PlayerState.Idle;
				}

				break;
			}

			case PlayerState.Interacting:
			{
				break;
			}

			default:
			{
				//updates horizontal input
				horizontalInput = Input.GetAxis("Horizontal");

				//code for checking jump input
				if (Input.GetButtonDown("Jump") && !isAirborne)
				{
					hasPressedJump = true;
					isAirborne = true;
				}
				else if (Input.GetButtonUp("Jump"))
				{
					hasPressedJump = false;
				}

				//code for interacting with interactives
				if ((interactives.Count > 0 && (Input.GetButtonDown("Fire1") || Input.GetAxis("Vertical") > 0) &&
				    !isAirborne))
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

					closestToPlayer.GetComponent<Interactive>().Interact();
					myRigidbody2D.velocity = Vector2.zero;
				}

				break;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (groundDetectorTrigger.IsTriggered)
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
			MyState = PlayerState.Idle;
		}
	}
}