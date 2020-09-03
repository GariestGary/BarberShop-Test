using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class Starter : MonoBehaviour
{
    [SerializeField, HideInInspector] private List<ManagerBase> managers = new List<ManagerBase>();
	[SerializeField] private int targetFrameRate = 60;

	private void Awake()
	{
		Application.targetFrameRate = targetFrameRate;

		for (int i = 0; i < managers.Count; i++)
		{
			Toolbox.AddManager(managers[i]);
		}

		var allAwakes = FindObjectsOfType(typeof(MonoBehaviour)).OfType<IAwake>();

		foreach (var awake in allAwakes)
		{
			awake.OnAwake();
		}

		Toolbox.GetManager<MessageManager>().Subscribe(ServiceShareData.SCENE_CHANGE, () => OnSceneChange());
	}

	private void OnSceneChange()
	{
		var allChanges = FindObjectsOfType(typeof(MonoBehaviour)).OfType<ISceneChange>();

		foreach (var change in allChanges)
		{
			change.OnSceneChange();
		}

		Toolbox.ClearAll();
	}
}
