using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool instance;

    public GameObject projectilePrefab;
    public int poolSize = 20;

    List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetProjectile()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        GameObject obj = Instantiate(projectilePrefab);
        obj.SetActive(false);
        pool.Add(obj);

        return obj;
    }
}