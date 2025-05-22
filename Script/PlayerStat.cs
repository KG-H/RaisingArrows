using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public int PlayerID;
    public float ArrowDmg;
    public float PlayerDmg;
    public float CritlcalRate;
    public float CriticalDmg;
    public int curArrow;
}

public class PlayerStat : MonoBehaviour
{ 
    public PlayerData[] playersData = new PlayerData[7]; // 7���� �÷��̾�
    public float dmgStack;       // �⺻ ������
    public float criRateStack;   // ũ�� ���߷�
    public float criDmgStack;    // ũ�� ������

    private string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/playerSave.dat";
        SaveData();
    }

    public void SaveData()
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            // ���� ������ ����
            writer.Write(dmgStack);
            writer.Write(criRateStack);
            writer.Write(criDmgStack);

            // �÷��̾� �� ����
            writer.Write(playersData.Length);

            foreach (PlayerData p in playersData)
            {
                writer.Write(p.PlayerID);
                writer.Write(p.ArrowDmg);
                writer.Write(p.PlayerDmg);
                writer.Write(p.CritlcalRate);
                writer.Write(p.CriticalDmg);
                writer.Write(p.curArrow);
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
            dmgStack = reader.ReadSingle();
            criRateStack = reader.ReadSingle();
            criDmgStack = reader.ReadSingle();

            // �÷��̾� �� �б�
            int count = reader.ReadInt32();
            playersData = new PlayerData[count];

            for (int i = 0; i < count; i++)
            {
                PlayerData p = new PlayerData();
                p.PlayerID = reader.ReadInt32();
                p.ArrowDmg = reader.ReadSingle();
                p.PlayerDmg = reader.ReadSingle();
                p.CritlcalRate = reader.ReadSingle();
                p.CriticalDmg = reader.ReadSingle();
                p.curArrow = reader.ReadInt32();

                playersData[i] = p;

            }
        }
    }
}
