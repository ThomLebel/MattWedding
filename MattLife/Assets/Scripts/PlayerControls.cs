using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	public Transform groundCheck;
	public float speed;
	public float jumpTakeOff;
	public LayerMask groundLayerMask;
	public LayerMask jumpThroughLayerMask;

	private float horizontal;
	private float vertical;
	private float safeSpot = 0.2f;

	[SerializeField]
	protected bool grounded = false;
	[SerializeField]
	protected bool isJumping = false;
	[SerializeField]
	protected bool isFalling = false;

	private Rigidbody2D rb2d;
	private Collider2D platformCollider;
	private CapsuleCollider2D playerCollider;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<CapsuleCollider2D>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, groundLayerMask);

		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");

		if (Input.GetButtonDown("Jump") && grounded && !isJumping && !isFalling)
		{
			isJumping = true;
			if (vertical < safeSpot * -1)
			{
				RaycastHit2D ray = Physics2D.Linecast(transform.position, groundCheck.position, jumpThroughLayerMask);
				if (ray.collider != null)
				{
					platformCollider = ray.collider;
					Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
					isFalling = true;
				}
			}
			else
			{
				rb2d.AddForce(new Vector2(0f, jumpTakeOff));
			}
			
		}

		if (Input.GetButtonUp("Jump"))
		{
			isJumping = false;
			if (!grounded && rb2d.velocity.y > 0)
			{
				rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
			}
		}

		if (isFalling)
		{
			if (transform.position.y < platformCollider.transform.position.y)
			{
				Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
				platformCollider = null;
				isFalling = false;
			}
		}
	}

	private void FixedUpdate()
	{
		rb2d.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb2d.velocity.y);
	}
}
