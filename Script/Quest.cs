using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Quest : MonoBehaviour
{
    public QuestData[] questsData; // ���� ����Ʈ �����͸� ���� �迭
    public int curQuest;

    void Start()
    {
        LoadQuestsFromJson(); // ���� ���� �� JSON ������ �ε�
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveQuestsToJson(); // ���� ��׶���� ��ȯ�� �� JSON ������ ����
        }
    }

    void OnApplicationQuit()
    {
        SaveQuestsToJson(); // ���� ���� �� JSON ������ ����
    }

    [ContextMenu("TO Json Data")]
    void SaveQuestsToJson()
    {
        string jsonData = JsonUtility.ToJson(new QuestDataContainer { quests = questsData, currentQuest = curQuest }, true);
        string path = Path.Combine(Application.dataPath, "QuestData.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From Json Data")]
    void LoadQuestsFromJson()
    {
        string path = Path.Combine(Application.dataPath, "QuestData.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            QuestDataContainer container = JsonUtility.FromJson<QuestDataContainer>(jsonData);
            questsData = container.quests; // JSON���� �ε��� �����ͷ� �迭 �ʱ�ȭ
            curQuest = container.currentQuest; // ���� ����Ʈ ����
        }
        else
        {
            Debug.LogError("������ ã�� �� �����ϴ�: " + path);
        }
    }
}

[System.Serializable]
public class QuestData
{
    public int QuestID;
    public string QuestName;
    public float curQuestValue;
    public float maxQuestValue;
    public bool isGold;
    public float Reward;
}

[System.Serializable]
public class QuestDataContainer
{
    public QuestData[] quests; // QuestData �迭�� ���δ� Ŭ����
    public int currentQuest; // ���� ����Ʈ ID
}
