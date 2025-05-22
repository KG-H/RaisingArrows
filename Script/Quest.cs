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
    public QuestData[] questsData; // ���� ����Ʈ �����͸� ���� �迭
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
            // ���� ������ ����
            writer.Write(curQuest);

            // �÷��̾� �� ����
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
            Debug.LogWarning("����� ������ �����ϴ�.");
            return;
        }

        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            // ���� ������ �б�
            curQuest = reader.ReadInt32();

            // �÷��̾� �� �б�
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
