using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ObjectPoolingSystem
{
	public List<GameObject> objectPool = new List<GameObject>();

	public void InitPool(int poolSize, GameObject poolPrefab, Transform parent)
	{
		if (poolPrefab == null)
		{
			Debug.LogWarning("No prefab was passed.");
			return;
		}

		for (int i = 0; i < poolSize; i++) 
		{
			GameObject newObj = MonoBehaviour.Instantiate(poolPrefab);
			newObj.SetActive(false);
			newObj.transform.parent = parent;
			objectPool.Add(newObj);
		}
	}

	public GameObject GetObject()
	{
		if (objectPool.Count > 0)
		{
			GameObject obj = objectPool[0];
			obj.SetActive(true);
			objectPool.RemoveAt(0);
			return obj; 
		}


		Debug.LogWarning("All objects from the pool have been used. Returning null.");
		return null;
	}

	public void ReturnObject(GameObject obj)
	{
		if (obj == null)
		{
			Debug.LogWarning("You passed a null object.");
			return; 
		}

		objectPool.Add(obj);
	}
}
