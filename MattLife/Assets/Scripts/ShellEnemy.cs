using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEnemy : Enemy
{
	public float launchSpeed;
	public Sprite baseSprite;
	public Sprite stunSprite;

	[SerializeField]
	private State state;
	[SerializeField]
	private bool scoreGained = false;
	[SerializeField]
	private float stunTime = 3f;

	private IEnumerator stunCoroutine;

	protected override void Start()
	{
		base.Start();

		state = State.walking;
	}

	protected override void Update()
	{
		base.Update();

		CollideWithEnemy();
	}

	protected override void CollideWithPlayer()
	{
		Collider2D targetCollider = null;
		Player playerScript = null;
		bool attack = false;
		bool beingAttacked = false;

		if (controller.collisions.below)
		{
			if (controller.collisions.verticalCollider != null && controller.collisions.verticalCollider.tag == "Player")
			{
				playerScript = controller.collisions.verticalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.verticalCollider;
				if (state != State.stun)
				{
					attack = true;
				}
			}
			else if (controller.collisions.reverseVerticalCollider != null && controller.collisions.reverseVerticalCollider.tag == "Player")
			{
				playerScript = controller.collisions.reverseVerticalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.reverseVerticalCollider;
				if (state != State.stun)
				{
					attack = true;
				}
			}
		}
		else if (controller.collisions.above)
		{
			if (controller.collisions.verticalCollider != null && controller.collisions.verticalCollider.tag == "Player")
			{
				beingAttacked = true;
				playerScript = controller.collisions.verticalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.verticalCollider;
			}
			else if (controller.collisions.reverseVerticalCollider != null && controller.collisions.reverseVerticalCollider.tag == "Player")
			{
				beingAttacked = true;
				playerScript = controller.collisions.reverseVerticalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.reverseVerticalCollider;
			}
		}
		if (controller.collisions.left || controller.collisions.right)
		{
			if (controller.collisions.horizontalCollider != null && controller.collisions.horizontalCollider.tag == "Player")
			{
				playerScript = controller.collisions.horizontalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.horizontalCollider;
				if (state != State.stun)
				{
					attack = true;
				}
				else
				{
					beingAttacked = true;
				}
			}
			else if (controller.collisions.reverseHorizontalCollider != null && controller.collisions.reverseHorizontalCollider.tag == "Player")
			{
				playerScript = controller.collisions.reverseHorizontalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.reverseHorizontalCollider;
				if (state != State.stun)
				{
					attack = true;
				}
				else
				{
					beingAttacked = true;
				}
			}
		}

		if (playerScript == null)
		{
			return;
		}
		if (playerScript.isInvulnerable)
		{
			controller.collisions.ignoredCollider = targetCollider;
			return;
		}
		else
		{
			controller.collisions.ignoredCollider = null;
		}

		if (beingAttacked)
		{
			Debug.Log("being attacked");
			beingAttacked = false;
			playerScript.AllowBounceOffMonster();
			UnderAttack(targetCollider);
		}
		else if (attack)
		{
			attack = false;
			playerScript.Hit();
		}

		playerScript.Bounce();
	}

	private void UnderAttack(Collider2D targetCollider)
	{
		AudioManager.instance.PlaySound(hitSound);
		Instantiate(effect, transform.position, Quaternion.identity);
		switch (state)
		{
			case State.launched:
				StopMonster();
				break;
			case State.walking:
				StunMonster();
				break;
			case State.stun:
				LaunchMonster(targetCollider);
				break;
		}
	}

	private void StunMonster()
	{
		Debug.Log("monster stun");
		state = State.stun;
		animator.enabled = false;
		spriteRenderer.sprite = stunSprite;
		speed = 0f;
		stunCoroutine = StunTimer();
		StartCoroutine(stunCoroutine);
		if (!scoreGained)
		{
			scoreGained = true;
			GameMaster.Instance.UpdateScore(score);
		}
	}

	private void LaunchMonster(Collider2D targetCollider)
	{
		Debug.Log("monster launched");
		state = State.launched;
		//gameObject.layer = LayerMask.NameToLayer("MovingShells");
		controller.collisionMask |= (1 << LayerMask.NameToLayer("Enemies"));
		controller.enemiesMask |= (1 << LayerMask.NameToLayer("Enemies"));
		StopCoroutine(stunCoroutine);

		/*switch (controller.collisions.faceDir)
		{
			case 1:
				if (targetCollider.transform.position.x > transform.position.x)
				{
					Flip();
				}
				break;
			case -1:
				if (targetCollider.transform.position.x < transform.position.x)
				{
					Flip();
				}
				break;
		}*/
		speed = launchSpeed;
	}

	private void StopMonster()
	{
		Debug.Log("stop monster");
		state = State.stun;
		//gameObject.layer = LayerMask.NameToLayer("Enemies");
		controller.collisionMask &= ~(1 << LayerMask.NameToLayer("Enemies"));
		controller.enemiesMask &= ~(1 << LayerMask.NameToLayer("Enemies"));
		speed = 0f;
		stunCoroutine = StunTimer();
		StartCoroutine(stunCoroutine);
	}

	private IEnumerator StunTimer()
	{
		yield return new WaitForSeconds(stunTime);
		state = State.walking;
		spriteRenderer.sprite = baseSprite;
		animator.enabled = true;
		speed = walkSpeed;
	}

	private void CollideWithEnemy()
	{
		if (state != State.launched)
		{
			return;
		}

		Enemy enemyScript = null;

		if (controller.collisions.below || controller.collisions.above)
		{
			if (controller.collisions.verticalCollider != null && controller.collisions.verticalCollider.tag == "Enemy")
			{
				enemyScript = controller.collisions.verticalCollider.GetComponent<Enemy>();
			}
			else if (controller.collisions.reverseVerticalCollider != null && controller.collisions.reverseVerticalCollider.tag == "Enemy")
			{
				enemyScript = controller.collisions.reverseVerticalCollider.GetComponent<Enemy>();
			}
		}
		else if (controller.collisions.left || controller.collisions.right)
		{
			if (controller.collisions.horizontalCollider != null && controller.collisions.horizontalCollider.tag == "Enemy")
			{
				enemyScript = controller.collisions.horizontalCollider.GetComponent<Enemy>();
			}
			else if (controller.collisions.reverseHorizontalCollider != null && controller.collisions.reverseHorizontalCollider.tag == "Enemy")
			{
				enemyScript = controller.collisions.reverseHorizontalCollider.GetComponent<Enemy>();
			}
		}

		if (enemyScript != null)
		{
			Debug.Log("got killed from here ?");
			Instantiate(effect, transform.position, Quaternion.identity);
			//collision.transform.GetComponent<EnemyScript>().Kill();
			enemyScript.Kill();
		}
	}

	enum State
	{
		walking,
		stun,
		launched
	}
}
