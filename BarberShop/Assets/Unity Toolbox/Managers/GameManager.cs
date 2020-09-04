using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Toolbox/Managers/Game Manager", fileName = "Game")]
public class GameManager : ManagerBase, IExecute
{
	public Transform Head { get; private set; }
	public void OnExecute()
	{
		
	}

	public void SetHead(Transform head)
	{
		Head = head;
	}
}