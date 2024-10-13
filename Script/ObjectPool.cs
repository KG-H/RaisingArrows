using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public GameObject[] monsterPrefabs; // ���� ������ �迭 (4���� ����)
    public int poolSize = 10;
    private List<GameObject> pooledObjects;
    private int currentIndex = 0;  // ��ȯ�� ���� �ε���

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();

        // �� ���� �����ո��� poolSize��ŭ�� ��ü ����
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
        // ��� ������ ��ü ã�� (���������� Ȯ��)
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
        monster.SetActive(false);  // ��Ȱ��ȭ�Ͽ� Ǯ�� ��ȯ
    }
}
