using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static PlayerObj;

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

public class Quest : MonoBehaviour
{
    public QuestData[] questsData; // 여러 퀘스트 데이터를 위한 배열
    public int curQuest;

    private string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/QuestSave.dat";
        SaveData();
    }

    public void SaveData()
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            // 누적 데이터 저장
            writer.Write(curQuest);

            // 플레이어 수 저장
            writer.Write(questsData.Length);

            foreach (QuestData p in questsData)
            {
                writer.Write(p.QuestID);
                writer.Write(p.QuestName);
                writer.Write(p.curQuestValue);
                writer.Write(p.maxQuestValue);
                writer.Write(p.isGold);
                writer.Write(p.Reward);
            }
        }
    }

    public void LoadData()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("저장된 파일이 없습니다.");
            return;
        }

        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            // 누적 데이터 읽기
            curQuest = reader.ReadInt32();

            // 플레이어 수 읽기
            int count = reader.ReadInt32();
            questsData = new QuestData[count];

            for (int i = 0; i < count; i++)
            {
                QuestData p = new QuestData();
                p.QuestID = reader.ReadInt32();
                p.QuestName = reader.ReadString();
                p.curQuestValue = reader.ReadSingle();
                p.maxQuestValue = reader.ReadSingle();
                p.isGold = reader.ReadBoolean();
                p.Reward = reader.ReadInt32();

                questsData[i] = p;

            }
        }
    }


}
