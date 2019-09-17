using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Enemy : MonoBehaviour
{
	public int score;
	public GameObject effect;
	public string hitSound;

	public float moveSpeed = 6;
	public float maxJumpHeight = 5;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;

	protected Vector3 velocity;
	protected float velocityXSmoothing;
	protected float gravity;
	protected float maxJumpVelocity;
	protected float minJumpVelocity;
	protected bool facingRight = true;

	[SerializeField]
	protected bool inAir = false;

	protected Vector2 directionalInput;
	protected EnemySpawner spawner;
	protected Controller2D controller;
	protected Animator animator;

	[SerializeField]
	protected float deletingDistance = 10f;
	protected Camera cam;
	protected float camHorizontalExtend;
	protected float camVerticalExtend;

	private void Awake()
	{
		cam = Camera.main;
		controller = GetComponent<Controller2D>();
		animator = GetComponent<Animator>();
	}

	// Start is called before the first frame update
	void Start()
    {
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;
		directionalInput = Vector2.left;
	}

    // Update is called once per frame
    void Update()
    {
		//Delete monster if out of sight
		if (transform.position.x >= cam.transform.position.x + camHorizontalExtend + deletingDistance ||
			transform.position.x <= cam.transform.position.x - camHorizontalExtend - deletingDistance)
		{
			DeleteMonster();
		}

		CalculateVelocity();

		controller.Move(velocity * Time.deltaTime, directionalInput);

		if (controller.collisions.above || controller.collisions.below)
		{
			if (controller.collisions.slidingDownMaxSlope)
			{
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			}
			else
			{
				velocity.y = 0;
			}
		}

		if (!controller.collisions.below)
		{
			inAir = true;
		}
		if (controller.collisions.below && inAir)
		{
			inAir = false;
		}

		CollideWithPlayer();

		//Change
		if (controller.collisions.horizontalCollider != null && controller.collisions.horizontalCollider.tag == "Obstacle")
		{
			controller.collisions.horizontalCollider = null;
			Flip();
		}
	}

	private void CollideWithPlayer()
	{
		Collider2D targetCollider = null;
		Player playerScript = null;
		bool attack = false;
		bool beingAttacked = false;

		if (controller.collisions.below)
		{
			if (controller.collisions.verticalCollider != null && controller.collisions.verticalCollider.tag == "Player")
			{
				attack = true;
				playerScript = controller.collisions.verticalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.verticalCollider;
			}
			else if (controller.collisions.reverseVerticalCollider != null && controller.collisions.reverseVerticalCollider.tag == "Player")
			{
				attack = true;
				playerScript = controller.collisions.reverseVerticalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.reverseVerticalCollider;
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
				attack = true;
				playerScript = controller.collisions.horizontalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.horizontalCollider;
			}
			else if (controller.collisions.reverseHorizontalCollider != null && controller.collisions.reverseHorizontalCollider.tag == "Player")
			{
				attack = true;
				playerScript = controller.collisions.reverseHorizontalCollider.GetComponent<Player>();
				targetCollider = controller.collisions.reverseHorizontalCollider;
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
			beingAttacked = false;
			Instantiate(effect, transform.position, Quaternion.identity);
			playerScript.AllowBounceOffMonster();
			Kill();
		}
		else if(attack)
		{
			attack = false;
			playerScript.Hit();
		}

		playerScript.Bounce();
	}

	public void SetDirectionalInput(Vector2 input)
	{
		directionalInput = input;
	}

	public void SetEnemySpwaner(EnemySpawner spawn)
	{
		spawner = spawn;
	}

	public void OnJumpIntended()
	{
		Vector2 jumpVelocity = Vector2.zero;

		if (controller.collisions.below)
		{
			if (controller.collisions.slidingDownMaxSlope)
			{
				if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) // not jumping against max slope
				{
					jumpVelocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					jumpVelocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			}
			else
			{
				jumpVelocity.y = maxJumpVelocity;
			}
		}

		if (jumpVelocity != Vector2.zero)
		{
			Jump(jumpVelocity);
		}
	}

	public void Jump(Vector2 jumpVelocity)
	{
		velocity = jumpVelocity;
		JumpEffect();
	}

	public void JumpEffect()
	{
		//Instantiate(jumpParticle, jumpParticlePosition.position, Quaternion.identity);
		//AudioManager.instance.PlaySound("PlayerJump");
	}

	void CalculateVelocity()
	{
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}

	public void Kill()
	{
		AudioManager.instance.PlaySound(hitSound);
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

	protected void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		directionalInput.x *= -1;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
