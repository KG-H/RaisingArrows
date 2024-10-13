using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public GameObject[] monsterPrefabs; // 몬스터 프리팹 배열 (4개의 몬스터)
    public int poolSize = 10;
    private List<GameObject> pooledObjects;
    private int currentIndex = 0;  // 순환을 위한 인덱스

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();

        // 각 몬스터 프리팹마다 poolSize만큼의 객체 생성
        foreach (GameObject prefab in monsterPrefabs)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(int randomMonsterIndex)
    {
        // 사용 가능한 객체 찾기 (순차적으로 확인)
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            GameObject obj = pooledObjects[i];
            currentIndex = (currentIndex + 1) % pooledObjects.Count;
            if (!obj.activeInHierarchy && 
                obj.GetComponentInChildren<BaseEnemy>().dmg == monsterPrefabs[randomMonsterIndex].GetComponentInChildren<BaseEnemy>().dmg)
            {
                return obj;
            }
        }

        GameObject forceReuseObj = pooledObjects[currentIndex];
        return forceReuseObj;
    }

    public void ReturnToPool(GameObject monster)
    {
        monster.SetActive(false);  // 비활성화하여 풀에 반환
    }
}
