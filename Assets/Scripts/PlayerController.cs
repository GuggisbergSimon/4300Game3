using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float fallMultiplier = 2.5f;
	[SerializeField] [Range(1, 10)] private float jumpSpeed = 5.0f;
	[SerializeField] private float lowJumpMultiplier = 2.0f;
	[SerializeField] [Range(1, 10)] private float speed = 5.0f;
	[SerializeField] private GameObject groundDetector;
	private TriggerDetector groundDetectorTrigger;
	private bool hasPressedJump;
	private bool isAirborne;
	private bool canPlayerMove = true;
	private Rigidbody2D myRigidbody2D;
	private List<GameObject> interactives = new List<GameObject>();



    //dorian code
    [SerializeField] private float distance;
    [SerializeField] private float inputVertical;
    public LayerMask Ladder;
    private bool isClimbing;
    //plus dorian code

	public bool CanPlayerMove
	{
		get => canPlayerMove;
		set => canPlayerMove = value;
	}

	private void Start()
	{
		groundDetectorTrigger = groundDetector.GetComponent<TriggerDetector>();
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


        //dorian code 
	    RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, Ladder);

	    if (hitInfo.collider != null)
	    {
	        if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown("w"))
	        {
	            isClimbing = true;
	        }
        }
	    else
	    {
	        if (Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.RightArrow))
	        {
	            isClimbing = false;
	        }
	    }

	    if (isClimbing == true && hitInfo.collider != null)
	    {
	        inputVertical = Input.GetAxisRaw("Vertical");
            myRigidbody2D.velocity=new Vector2(myRigidbody2D.velocity.x,inputVertical*speed);
	        myRigidbody2D.gravityScale = 0;

        }
        else
        {
            myRigidbody2D.gravityScale = 1;
        }
        //plus dorian code
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

				closestToPlayer.GetComponent<Interactive>().Interact();
				canPlayerMove = false;
				myRigidbody2D.velocity = Vector2.zero;
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
		}
	}
}