using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    public ObjectPooler()
	{
        destroyOnLoad = true;
	}

    [SerializeField] private int minObjectsCountToCreateNew;
    [SerializeField] private List<Pool> PoolsList = new List<Pool>();
	private Transform ObjectPoolParent;

    public Dictionary<string, LinkedList<GameObject>> Pools { get; private set; } = new Dictionary<string, LinkedList<GameObject>>();

	[System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject pooledObject;
        public int size;
    }

    public void Awake()
    {
        Pools = new Dictionary<string, LinkedList<GameObject>>();

        ObjectPoolParent = new GameObject().transform;
        ObjectPoolParent.name = "PoolParent";

        for (int i = 0; i < PoolsList.Count; i++)
        {
            AddPool(PoolsList[i].tag, PoolsList[i].pooledObject, PoolsList[i].size);
        }
    }

    public void AddPool(string tag, GameObject obj, int size)
    {
        if (Pools.ContainsKey(tag))
        {
            return;
        }

        LinkedList<GameObject> objectPool = new LinkedList<GameObject>();

        for (int j = 0; j < size; j++)
        {
            CreateNewPoolObject(obj, objectPool);
        }

        Pools.Add(tag, objectPool);
    }

    public GameObject GetObject(string poolTag, Vector3 position, Quaternion rotation, Transform parent = null, object data = null)
    {
        if (!Pools.ContainsKey(poolTag))
        {
            Debug.LogWarning("Object pool with tag " + poolTag + " doesn't exist");
            return null;
        }

        if (Pools[poolTag].Last.Value.activeSelf)
        {
            CreateNewPoolObject(Pools[poolTag].Last.Value, Pools[poolTag]);
        }

        GameObject obj = Pools[poolTag].Last.Value;
        Pools[poolTag].RemoveLast();

        if (obj == null)
        {
            return null;
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.SetParent(parent);
        obj.SetActive(true);

        IPooledObject[] pooled = obj.GetComponents<IPooledObject>();


		for (int i = 0; i < pooled.Length; i++)
		{
            if (pooled[i] != null)
            { 
                pooled[i].OnSpawn(data);
            }
		}
       
        Pools[poolTag].AddFirst(obj);
        return (obj);
    }

    private void CreateNewPoolObject(GameObject obj, LinkedList<GameObject> pool)
    {
        GameObject poolObj = Instantiate(obj);
        poolObj.name = obj.name;
        poolObj.transform.SetParent(ObjectPoolParent);

        IAwake[] awakes = poolObj.GetComponents<IAwake>();

		foreach (var awake in awakes)
		{
            awake.OnAwake();
		}
        
        poolObj.gameObject.SetActive(false);
        pool.AddLast(poolObj);
    }

    public IEnumerator DespawnCoroutine(GameObject o, float d = 0)
    {
        yield return new WaitForSeconds(d);
        o.SetActive(false);
        o.transform.SetParent(ObjectPoolParent);
    }

    public void Despawn(GameObject ObjectToDespawn, float delay = 0)
    {
        if (ObjectToDespawn == null)
        {
            return;
        }

        if(delay != 0)
		{
            StartCoroutine(DespawnCoroutine(ObjectToDespawn, delay));
        }
        else
		{
            ObjectToDespawn.SetActive(false);
            ObjectToDespawn.transform.SetParent(ObjectPoolParent);
        }

        
    }

}
