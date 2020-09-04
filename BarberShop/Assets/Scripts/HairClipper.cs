using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairClipper : MonoBehaviour, ITick, IAwake
{
	[Range(0, 1)]
	[SerializeField] private float sensitivity;
	[SerializeField] private LayerMask headlayer;
	[SerializeField] private float rotationDamping;
	[SerializeField] private BoxCollider bounds;
	public bool Process => gameObject.activeSelf;

	private InputManager input => Toolbox.GetManager<InputManager>();
	private GameManager game => Toolbox.GetManager<GameManager>();

	private Transform t;
	private Camera cam;
	private Rigidbody rb;
	private bool moved = false;

	public void OnAwake()
	{
		Toolbox.GetManager<UpdateManager>().Add(this);
		t = transform;
		cam = Camera.main;
		rb = GetComponent<Rigidbody>();
	}

	public void OnTick()
	{
		if (game.Head == null) return;

		Move();
		Clamp();

		if (!moved) return;

		Rotate();
		
	}

	private void Move()
	{
		if (input.Clicked)
		{
			Vector3 moveDelta = new Vector3(input.PointerDelta.x, input.PointerDelta.y, 0);
			rb.MovePosition(t.position + moveDelta * sensitivity);

			if (!moved) moved = true;
		}
		t.position = new Vector3(t.position.x, t.position.y, game.Head.position.z);
		rb.velocity = Vector3.zero;
	}

	private void Rotate()
	{
		RaycastHit hit;

		Vector3 headDirection = game.Head.position - t.position;

		Debug.DrawRay(t.position, headDirection.normalized * Vector3.Distance(game.Head.position, t.position), Color.red);

		if(Physics.Raycast(t.position, headDirection, out hit, Vector3.Distance(game.Head.position, t.position), headlayer))
		{
			Debug.DrawRay(hit.point, hit.normal, Color.green);

			t.rotation = Quaternion.Lerp(t.rotation, Quaternion.LookRotation(headDirection), Time.deltaTime * rotationDamping);
		}
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
