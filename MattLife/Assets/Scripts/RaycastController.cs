﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
	public LayerMask collisionMask;

	[SerializeField]
	protected const float skinWidth = 0.015f;
	[SerializeField]
	protected const float distBetweenRays = 0.25f;

	protected int horizontalRayCount = 4;
	protected int verticalRayCount = 4;

	protected float horizontalRaySpacing;
	protected float verticalRaySpacing;

	[HideInInspector]
	public BoxCollider2D boxCollider;
	protected RaycastOrigins raycastOrigins;

	public virtual void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
	}

	public virtual void Start()
	{
		CalculateRaySpacing();
	}

	public void UpdateRaycastOrigins()
	{
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	public void CalculateRaySpacing()
	{
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;

		horizontalRayCount = Mathf.RoundToInt(boundsHeight / distBetweenRays);
		verticalRayCount = Mathf.RoundToInt(boundsWidth / distBetweenRays);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
