using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Input", menuName = "Toolbox/Managers/Input Manager")]
public class InputManager : ManagerBase, IExecute, ISceneChange
{
	[SerializeField] private bool externalInput = false;
	//Create input actions assets named "Controls"
	private Controls controls;

	public Vector2 PointerPosition { get; private set; }
	public Vector2 PointerDelta { get; private set; }
	public Vector2 MoveInput { get; private set; }

	public event Action OnClick;

	public bool Clicked { get; private set; } = false;

	public Controls GetBindings()
	{
		return controls;
	}

	public void SetInput(Vector2 input)
	{
		if (!externalInput) return;

		MoveInput = input;
	}

	private void OnEnable()
	{
		controls?.Enable();
	}

	private void OnDisable()
	{
		controls?.Disable();
	}

	private void OnDestroy()
	{
		controls?.Dispose();
	}

	private void InitializeControls()
	{
		controls = new Controls();

		controls.Player.PointerPosition.performed += ctx => PointerPosition = ctx.ReadValue<Vector2>();
		controls.Player.PointerDelta.performed += ctx => PointerDelta = ctx.ReadValue<Vector2>();

		controls.Player.Movement.performed += ctx => 
		{
			if (externalInput) return;
			MoveInput = ctx.ReadValue<Vector2>();
		};
		controls.Player.Click.performed += _ => 
		{
			Clicked = true;
			OnClick?.Invoke();
		};

		controls.Player.Click.canceled += _ =>
		{
			Clicked = false;
		};

		controls.Enable();
	}

	public void OnExecute()
	{
		controls?.Dispose();

		InitializeControls();
	}

	public void OnSceneChange()
	{
		controls?.Dispose();
	}
}
