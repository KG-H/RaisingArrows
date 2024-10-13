using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Quest : MonoBehaviour
{
    public QuestData[] questsData; // 여러 퀘스트 데이터를 위한 배열
    public int curQuest;

    void Start()
    {
        LoadQuestsFromJson(); // 게임 시작 시 JSON 데이터 로드
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveQuestsToJson(); // 앱이 백그라운드로 전환될 때 JSON 데이터 저장
        }
    }

    void OnApplicationQuit()
    {
        SaveQuestsToJson(); // 게임 종료 시 JSON 데이터 저장
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
            questsData = container.quests; // JSON에서 로드한 데이터로 배열 초기화
            curQuest = container.currentQuest; // 현재 퀘스트 설정
        }
        else
        {
            Debug.LogError("파일을 찾을 수 없습니다: " + path);
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
    public QuestData[] quests; // QuestData 배열을 감싸는 클래스
    public int currentQuest; // 현재 퀘스트 ID
}
