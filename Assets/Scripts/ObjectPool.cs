using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool 
{
    public static void FlushPool()
    {
        pools.Clear();
    }

    static Dictionary<string, MonoBehaviour> prefabs = new();
    static Dictionary<string, List<MonoBehaviour>> pools = new();
    
    /// <summary>
    /// Just disable an object when you're done with it and the pool will clean it up.
    /// Make sure everything is properly initialized when spawning.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T SpawnObject<T>(string resource) where T: MonoBehaviour, IPoolable
    {
        string key = resource;
        if (!pools.ContainsKey(key))
            pools[key] = new();

        foreach (var t in pools[key])
        {
            if (!t.gameObject.activeSelf)
            {
                (t as IPoolable).ResetPoolable();
                t.gameObject.SetActive(true);
                return t as T;
            }
        }

        if (!prefabs.ContainsKey(key))
        {
            Debug.Log(key);
            prefabs[key] = (Resources.Load(key) as GameObject).GetComponent<T>();
        }
        T newT = GameObject.Instantiate(prefabs[key]) as T;

        pools[key].Add(newT);
        return newT;
    }
}

public interface IPoolable
{
    public void ResetPoolable();
}