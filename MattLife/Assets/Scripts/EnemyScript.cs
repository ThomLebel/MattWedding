using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	public float walkSpeed = 40;

	public bool bumpOnWall = false;
	public LayerMask wallLayerMask;

	public Transform headCheck;
	public Transform wallCheck;

	protected float speed;
	protected int direction = -1;
	protected float spriteWidth;
	protected float spriteHeight;

	protected SpriteRenderer spriteRenderer;
	protected Rigidbody2D rb2d;
	protected Animator animator;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}
	// Start is called before the first frame update
	void Start()
    {
		speed = walkSpeed;
		spriteWidth = spriteRenderer.bounds.size.x;
		spriteWidth = spriteRenderer.bounds.size.y;
	}

    // Update is called once per frame
    void Update()
    {
		bumpOnWall = Physics2D.Linecast(transform.position, wallCheck.position, wallLayerMask);
		if (bumpOnWall)
		{
			Flip();
		}

		rb2d.velocity = new Vector2(direction * speed * Time.deltaTime, rb2d.velocity.y);
	}

	public virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player")
		{
			Vector2 contact = collision.GetContact(0).point;

			if (contact.y >= headCheck.position.y)
			{
				Debug.Log("Kill this monster !");
				//Play hit animation

				//Raise a flag notifying the player that is can bounce on the monster back
				collision.transform.GetComponent<PlayerControls>().Bounce();

				//Kill();
			}else
			{
				Debug.Log("Kill the player !");
			}
		}
	}

	protected void Flip()
	{
		// Multiply the player's x local scale by -1.
		direction *= -1;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Kill()
	{
		Destroy(gameObject);
	}
}
