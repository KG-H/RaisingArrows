using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public string tag;
        public GameObject prefab;
        public int poolSize;
    }

    public static ObjectPool Instance;

    public List<PoolItem> poolItems;
    private List<Queue<GameObject>> poolQueues;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolQueues = new List<Queue<GameObject>>();

        foreach (PoolItem item in poolItems)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            poolQueues.Add(objectQueue);
        }
    }

    // 인덱스로 오브젝트 꺼내기
    public GameObject GetPooledObject(int index)
    {
        if (index < 0 || index >= poolQueues.Count)
        {
            return null;
        }

        Queue<GameObject> queue = poolQueues[index];

        foreach (GameObject obj in queue)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        return null;
    }

    // 가장 가까운 활성화된 몬스터 반환
    public GameObject GetClosestActiveMonster(Vector3 position)
    {
        return poolQueues
            .SelectMany(queue => queue)
            .Where(m => m != null && m.activeInHierarchy && m.CompareTag("Enemy"))
            .OrderBy(m => Vector3.Distance(position, m.transform.position))
            .FirstOrDefault();
    }

    // 풀로 반환
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}