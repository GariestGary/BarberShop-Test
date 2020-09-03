using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Toolbox/Managers/Touch Input Manager")]
public class TouchInput : ManagerBase, ITick, IExecute
{
	public bool Process => true;

	private InputManager input => Toolbox.GetManager<InputManager>();

	public void OnExecute()
	{
		Toolbox.GetManager<UpdateManager>().Add(this);
	}

	public void OnTick()
	{
		if (!input) return;

		if (input.Clicked)
		{
			input.SetInput(input.PointerDelta);
		}
		else
		{
			input.SetInput(new Vector2(0, 0));
		}
	}
}
