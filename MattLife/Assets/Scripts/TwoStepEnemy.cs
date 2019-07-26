using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoStepEnemy : EnemyScript
{
	public float launchSpeed;
	public Sprite baseSprite;
	public Sprite stunSprite;

	[SerializeField]
	private bool isStun = false;
	[SerializeField]
	private bool isLaunch = false;
	[SerializeField]
	private float stunTime = 3f;

	private IEnumerator stunCoroutine;


	public override void OnCollisionEnter2D(Collision2D collision)
	{
		//base.OnCollisionEnter2D(collision);
		if (collision.transform.tag == "Player")
		{
			Vector2 contact = collision.GetContact(0).point;

			if (contact.y >= transform.position.y)
			{
				//On stun le monstre
				if (!isStun)
				{
					StunMonster();
				}
				//On lance le monstre sous forme de coquille
				else if (isStun && !isLaunch)
				{
					LaunchMonster(contact);
				}
				//On stop le monstre
				else if (isStun && isLaunch)
				{
					StopMonster();
				}

				//We bounce off the monster head
				collision.transform.GetComponent<PlayerControls>().Bounce();
			}
			else
			{
				if (isStun && !isLaunch)
				{
					LaunchMonster(contact);
				}
				else
				{
					Debug.Log("Kill the player !");
				}
				
			}
		}
		else if (collision.transform.tag == "Enemy")
		{
			Debug.Log("Kill this enemy !");
			//collision.transform.GetComponent<EnemyScript>().Kill();
		}
	}

	private void StunMonster()
	{
		isStun = true;
		Debug.Log("Stun this monster !");
		animator.enabled = false;
		spriteRenderer.sprite = stunSprite;
		speed = 0f;
		stunCoroutine = StunTimer();
		StartCoroutine(stunCoroutine);
	}

	private void LaunchMonster(Vector2 contact)
	{
		isLaunch = true;
		Debug.Log("Launch this monster from :"+ contact.x);
		Debug.Log("center is at : "+ transform.position.x + headCheck.position.x);
		StopCoroutine(stunCoroutine);
		switch (direction)
		{
			case 1:
				if (contact.x > transform.position.x)
				{
					Debug.Log("going right, we flip to go left");
					Flip();
				}
				break;
			case -1:
				if (contact.x < transform.position.x)
				{
					Debug.Log("going left, we flip to go right");
					Flip();
				}
				break;
		}
		/*if (contact.x > transform.position.x + headCheck.position.x)
		{
			Flip();
		}*/
		speed = launchSpeed;
	}

	private void StopMonster()
	{
		isLaunch = false;
		speed = 0f;
		stunCoroutine = StunTimer();
		StartCoroutine(stunCoroutine);
	}

	private IEnumerator StunTimer()
	{
		yield return new WaitForSeconds(stunTime);
		isStun = false;
		spriteRenderer.sprite = baseSprite;
		animator.enabled = true;
		speed = walkSpeed;
	}
}
