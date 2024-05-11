using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Dictionary<string, Queue<GameObject>> object_pool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;
    private static ObjectPool instance;

    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    //对象池获取对应的游戏对象
    public GameObject GetObject(GameObject game_object)
    {
        GameObject temp_object;
        if (!object_pool.ContainsKey(game_object.name) || object_pool[game_object.name].Count == 0)
        {
            temp_object = GameObject.Instantiate(game_object);
            PushGameObject(temp_object);
            if (pool == null)
            {
                pool = new GameObject("ObjectPool");
            }
            GameObject chlid_pool = GameObject.Find(game_object.name + "Pool");
            if (!chlid_pool)
            {
                chlid_pool = new GameObject(game_object.name + "Pool");
                chlid_pool.transform.SetParent(pool.transform);
            }
            temp_object.transform.SetParent(chlid_pool.transform);
        }
        temp_object = object_pool[game_object.name].Dequeue();
        temp_object.SetActive(true);
        return temp_object;
    }

    //将用完的游戏对象放回队列池里
    public void PushGameObject(GameObject game_object)
    {
        string object_name = game_object.name.Replace("(Clone)", string.Empty);
        if (!object_pool.ContainsKey(object_name))
        {
            object_pool.Add(object_name, new Queue<GameObject>());
        }
        object_pool[object_name].Enqueue(game_object);
        game_object.SetActive(false);
    }
}
