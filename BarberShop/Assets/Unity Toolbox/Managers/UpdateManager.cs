using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Update", menuName = "Toolbox/Managers/Update Manager")]
public class UpdateManager : ManagerBase, IExecute
{
	private List<ITick> ticks = new List<ITick>();
	private List<IFixedTick> fixedTicks = new List<IFixedTick>();
	private List<ILateTick> lateTicks = new List<ILateTick>();
	private List<IExecute> inits = new List<IExecute>();

	public void Add(object obj)
	{
		var mng = Toolbox.GetManager<UpdateManager>();

		if (obj is ITick)
		{
			mng.ticks.Add(obj as ITick);
		}

		if (obj is IFixedTick)
		{
			mng.fixedTicks.Add(obj as IFixedTick);
		}

		if (obj is ILateTick)
		{
			mng.lateTicks.Add(obj as ILateTick);
		}
	}

	public void Remove(object obj)
	{
		var mng = Toolbox.GetManager<UpdateManager>();

		if (obj is ITick)
		{
			mng.ticks.Remove(obj as ITick);
		}

		if (obj is IFixedTick)
		{
			mng.fixedTicks.Remove(obj as IFixedTick);
		}

		if (obj is ILateTick)
		{
			mng.lateTicks.Remove(obj as ILateTick);
		}
	}


	public void Tick()
	{
		for (int i = 0; i < ticks.Count; i++)
		{
			if (ticks[i].Process)
				ticks[i].OnTick();
		}
	}

	public void FixedTick()
	{
		for (int i = 0; i < fixedTicks.Count; i++)
		{
			if (fixedTicks[i].Process)
				fixedTicks[i].OnFixedTick();
		}
	}

	public void LateTick()
	{
		for (int i = 0; i < lateTicks.Count; i++)
		{
			if (lateTicks[i].Process)
				lateTicks[i].OnLateTick();
		}
	}

	public void OnExecute()
	{
		ticks.Clear();
		fixedTicks.Clear();
		lateTicks.Clear();

		UpdateManagerComponent umc = GameObject.Find("[ENTRY]").AddComponent<UpdateManagerComponent>();
		umc.enabled = true;
		umc.Setup(this);
	}

	
}
