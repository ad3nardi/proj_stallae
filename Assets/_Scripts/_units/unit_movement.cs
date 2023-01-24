using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Util;
using Unity.VisualScripting;

[RequireComponent(typeof(RichAI))]
[RequireComponent(typeof(Seeker))]
public class unit_movement : OptimizedBehaviour
{   
    [Header("Plugins")]
    [SerializeField] private Seeker _seeker;
    [SerializeField] private RichAI _aiCon;
    [Header("Unit Plugins")]
    [SerializeField] private unit_Manager _unitM;
	[SerializeField] private List<Transform> unitVis;

	[Header("Settings")]
	[SerializeField] private float _checkDistance;
    //[SerializeField] private float moveSpeed;

	/*
    [Header("Pathfinding")]
    [SerializeField] private Path _path;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float nextWaypointDistance = 3;
    [SerializeField] private int currentWaypoint = 0;
    [SerializeField] private bool reachedEndofPath;

	[Header("RVO Settings")]
	[SerializeField] public float repathRate = 1;
	[SerializeField] private float nextRepath = 0;

	[SerializeField] private bool canSearchAgain = true;

	[SerializeField] public float maxSpeed = 10;

	[SerializeField] Path path = null;

	[SerializeField] List<Vector3> vectorPath;
	[SerializeField] int wp;

	[SerializeField] public float moveNextDist = 1;
	[SerializeField] public float slowdownDistance = 1;
	[SerializeField] public LayerMask groundMask;
	*/
	public void Awake()
	{
		_unitM = GetComponent<unit_Manager>();
		_seeker = GetComponent<Seeker>();
        _aiCon = GetComponent<RichAI>();
		unitVis = new List<Transform>();
	}
    public void SetDefaults()
	{
		_aiCon.maxSpeed = _unitM.unit.unitMaxSpeed;
		_aiCon.acceleration = _unitM.unit.unitAcceleration;
		_aiCon.rotationSpeed = _unitM.unit.unitRotationSpeed;
		_aiCon.slowdownTime = _unitM.unit.unitSlowdownTime;
		_aiCon.wallForce = _unitM.unit.unitWallForce;
		_aiCon.wallDist = _unitM.unit.unitWallDist;
		_aiCon.endReachedDistance = _unitM.unit.unitEndReachedDistance;

		_checkDistance = _unitM.unit.unitCheckDistance;
	}
    public void Update()
	{
		CheckCollision();
	}
	public void CheckCollision()
	{
		RaycastHit hit;

		if (Physics.Raycast(CachedTransform.position, Vector3.forward, out hit))
		{
			for (int i = 0; i < unitVis.Count; i++)
			{
				//unitVis[i].position = new Vector3(unitVis[i].position.x, unitVis[i].position.y + 10f * Time.deltaTime, unitVis[i].position.x); 
				Debug.Log("rising " + unitVis[i]);
			}
		}
	}
    /*
	/// <summary>Set the point to move to</summary>
	public void SetTarget(Vector3 target)
	{
		targetPosition = target;
		RecalculatePath();
	}

	public void RecalculatePath()
	{
		canSearchAgain = false;
		nextRepath = Time.time + repathRate * (Random.value + 0.5f);
		_seeker.StartPath(CachedTransform.position, targetPosition, OnPathComplete);
	}

	public void OnPathComplete(Path _p)
	{
		ABPath p = _p as ABPath;

		canSearchAgain = true;

		if (path != null) path.Release(this);
		path = p;
		p.Claim(this);

		if (p.error)
		{
			wp = 0;
			vectorPath = null;
			return;
		}


		Vector3 p1 = p.originalStartPoint;
		Vector3 p2 = CachedTransform.position;
		p1.y = p2.y;
		float d = (p2 - p1).magnitude;
		wp = 0;

		vectorPath = p.vectorPath;
		Vector3 waypoint;

		if (moveNextDist > 0)
		{
			for (float t = 0; t <= d; t += moveNextDist * 0.6f)
			{
				wp--;
				Vector3 pos = p1 + (p2 - p1) * t;

				do
				{
					wp++;
					waypoint = vectorPath[wp];
				} while (_rvoCon.To2D(pos - waypoint).sqrMagnitude < moveNextDist * moveNextDist && wp != vectorPath.Count - 1);
			}
		}
	}


    public void CheckRepath()
    {
		if (Time.time >= nextRepath && canSearchAgain)
		{
			RecalculatePath();
		}
	}

	public void UpdatePosition()
    {
		Vector3 pos = CachedTransform.position;

		if (vectorPath != null && vectorPath.Count != 0)
		{
			while ((_rvoCon.To2D(pos - vectorPath[wp]).sqrMagnitude < moveNextDist * moveNextDist && wp != vectorPath.Count - 1) || wp == 0)
			{
				wp++;
			}

			// Current path segment goes from vectorPath[wp-1] to vectorPath[wp]
			// We want to find the point on that segment that is 'moveNextDist' from our current position.
			// This can be visualized as finding the intersection of a circle with radius 'moveNextDist'
			// centered at our current position with that segment.
			var p1 = vectorPath[wp - 1];
			var p2 = vectorPath[wp];

			// Calculate the intersection with the circle. This involves some math.
			var t = VectorMath.LineCircleIntersectionFactor(_rvoCon.To2D(transform.position), _rvoCon.To2D(p1), _rvoCon.To2D(p2), moveNextDist);
			// Clamp to a point on the segment
			t = Mathf.Clamp01(t);
			Vector3 waypoint = Vector3.Lerp(p1, p2, t);

			// Calculate distance to the end of the path
			float remainingDistance = _rvoCon.To2D(waypoint - pos).magnitude + _rvoCon.To2D(waypoint - p2).magnitude;
			for (int i = wp; i < vectorPath.Count - 1; i++) remainingDistance += _rvoCon.To2D(vectorPath[i + 1] - vectorPath[i]).magnitude;

			// Set the target to a point in the direction of the current waypoint at a distance
			// equal to the remaining distance along the path. Since the rvo agent assumes that
			// it should stop when it reaches the target point, this will produce good avoidance
			// behavior near the end of the path. When not close to the end point it will act just
			// as being commanded to move in a particular direction, not toward a particular point
			var rvoTarget = (waypoint - pos).normalized * remainingDistance + pos;
			// When within [slowdownDistance] units from the target, use a progressively lower speed
			var desiredSpeed = Mathf.Clamp01(remainingDistance / slowdownDistance) * maxSpeed;
			Debug.DrawLine(CachedTransform.position, waypoint, Color.red);
			_rvoCon.SetTarget(rvoTarget, desiredSpeed, maxSpeed);
		}
		else
		{
			// Stand still
			_rvoCon.SetTarget(pos, maxSpeed, maxSpeed);
		}

		// Get a processed movement delta from the rvo controller and move the character.
		// This is based on information from earlier frames.
		var movementDelta = _rvoCon.CalculateMovementDelta(Time.deltaTime);
		pos += movementDelta;

		// Rotate the character if the velocity is not extremely small
		if (Time.deltaTime > 0 && movementDelta.magnitude / Time.deltaTime > 0.01f)
		{
			var rot = CachedTransform.rotation;
			var targetRot = Quaternion.LookRotation(movementDelta, _rvoCon.To3D(Vector2.zero, 1));
			const float RotationSpeed = 5;
			if (_rvoCon.movementPlane == MovementPlane.XY)
			{
				targetRot = targetRot * Quaternion.Euler(-90, 180, 0);
			}
			CachedTransform.rotation = Quaternion.Slerp(rot, targetRot, Time.deltaTime * RotationSpeed);
		}

		if (_rvoCon.movementPlane == MovementPlane.XZ)
		{
			RaycastHit hit;
			if (Physics.Raycast(pos + Vector3.up, Vector3.down, out hit, 2, groundMask))
			{
				pos.y = hit.point.y;
			}
		}

		CachedTransform.position = pos;
	}
	*/
}
