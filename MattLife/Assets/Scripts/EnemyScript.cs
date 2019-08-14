using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	public int score;
	public float walkSpeed = 40;

	public bool bumpOnWall = false;
	public LayerMask wallLayerMask;

	public Transform headCheck;
	public Transform wallCheck;

	public EnemySpawner spawner;

	protected float speed;
	protected int direction = -1;
	protected float spriteWidth;
	protected float spriteHeight;

	protected SpriteRenderer spriteRenderer;
	protected Rigidbody2D rb2d;
	protected Animator animator;

	[SerializeField]
	private float deletingDistance = 10f;
	private Camera cam;
	private float camHorizontalExtend;
	private float camVerticalExtend;

	private void Awake()
	{
		cam = Camera.main;
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
		camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;
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

		//Delete monster if out of reach
		if (transform.position.x >= cam.transform.position.x + camHorizontalExtend + deletingDistance ||
			transform.position.x <= cam.transform.position.x - camHorizontalExtend - deletingDistance)
		{
			DeleteMonster();
		}
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
				//collision.transform.GetComponent<PlayerControls>().Bounce();
				collision.transform.GetComponent<Player>().Bounce();

				Kill();
			}else
			{
				Debug.Log("Kill the player !");
				GameMaster.Instance.UpdateLife(-1);
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
		GameMaster.Instance.UpdateScore(score);
		DeleteMonster();
	}

	public void DeleteMonster()
	{
		if (spawner != null)
		{
			spawner.monsterList.Remove(gameObject);
		}
		Destroy(gameObject);
	}
}
