using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairClipper : MonoBehaviour, ITick, IAwake
{
	[Range(0, 1)]
	[SerializeField] private float sensitivity;
	[Space]
	[SerializeField] private float depthOffset;
	[SerializeField] private float depthCheckOffset;
	[Range(1, 50)]
	[SerializeField] private float rotationDamping;
	[Range(1, 50)]
	[SerializeField] private float depthMoveDamping;
	[Space]
	[SerializeField] private LayerMask headLayer;
	[SerializeField] private BoxCollider bounds;
	[SerializeField] private List<Vector3> pointsToCheck;

	public bool Process => gameObject.activeSelf;

	private InputManager input => Toolbox.GetManager<InputManager>();
	private GameManager game => Toolbox.GetManager<GameManager>();

	private Transform t;			//self Transform
	private Rigidbody rb;			//RigidBody
	private bool moved = false;     //Is hair clipper moved flag
	private bool overHead = false;  //Is hair clipper moving over head

	public void OnAwake()
	{
		Toolbox.GetManager<UpdateManager>().Add(this);
		t = transform;
		rb = GetComponent<Rigidbody>();
	}

	public void OnTick()
	{
		if (game.Head == null) return;

		if (input.Clicked)
		{
			Move();
			Clamp();
		}

		DepthMove();

		if (!moved) return;

		Rotate();
		
	}

	private void Move()
	{
		t.position += new Vector3(input.PointerDelta.x, input.PointerDelta.y, 0) * sensitivity;

		if (!moved) moved = true;
	}

	private void DepthMove()
	{
		float maxDepth = 0;

		overHead = false;

		foreach (var point in pointsToCheck)
		{
			if(Physics.Raycast(t.position + point + (Vector3.back * depthCheckOffset), Vector3.forward, out RaycastHit hit, float.PositiveInfinity, headLayer))
			{
				overHead = true;

				if (hit.point.z < maxDepth) maxDepth = hit.point.z;
			}
		}

		float targetDepth;

		if(overHead)
		{
			targetDepth = maxDepth + depthOffset;
		}
		else
		{
			targetDepth = game.Head.position.z;
		}

		t.position = new Vector3(t.position.x, t.position.y, Mathf.Lerp(t.position.z, targetDepth, Time.deltaTime * depthMoveDamping));
	}

	private void Rotate()
	{
		Vector3 headDirection = game.Head.position - t.position;

		Quaternion targetRotation = Quaternion.LookRotation(headDirection);

		t.rotation = Quaternion.Lerp(t.rotation, targetRotation, Time.deltaTime * rotationDamping);
	}

	private void Clamp()
	{
		if (bounds == null) return;

		float xPos = Mathf.Clamp(t.position.x, -bounds.bounds.extents.x, bounds.bounds.extents.x);
		float yPos = Mathf.Clamp(t.position.y, -bounds.bounds.extents.y, bounds.bounds.extents.y);
		float zPos = Mathf.Clamp(t.position.z, -bounds.bounds.extents.z, bounds.bounds.extents.z);

		t.position = new Vector3(xPos, yPos, zPos);
	}
}
