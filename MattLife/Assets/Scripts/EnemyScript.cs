using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	public float speed;

	private int direction = -1;
	public bool bumpOnWall = false;
	public LayerMask wallLayerMask;

	public Transform headCheck;
	public Transform wallCheck;

	private Rigidbody2D rb2d;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		bumpOnWall = Physics2D.Linecast(transform.position, wallCheck.position, wallLayerMask);
		if (bumpOnWall)
		{
			direction *= -1;
			Flip();
		}

		rb2d.velocity = new Vector2(direction * speed * Time.deltaTime, rb2d.velocity.y);
	}

	//// Player jump on his head
	//private void OnTriggerEnter2D(Collider2D collision)
	//{
	//	if (collision.tag == "Player")
	//	{
	//		Debug.Log("Kill this monster !");
	//		//Play hit animation

	//		//Raise a flag notifying the player that is can bounce on the monster back
	//		collision.GetComponent<PlayerControls>().Bounce();

	//		//Kill();
	//	}
	//}

	private void OnCollisionEnter2D(Collision2D collision)
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

	private void Flip()
	{
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void Kill()
	{
		Destroy(gameObject);
	}
}
