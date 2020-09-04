using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour, IAwake
{
	public void OnAwake()
	{
		Toolbox.GetManager<GameManager>().SetHead(transform);
	}
}
