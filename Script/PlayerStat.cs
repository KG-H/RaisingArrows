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
    public PlayerData[] playersData = new PlayerData[7]; // 7명의 플레이어
    public float dmgStack;       // 기본 데미지
    public float criRateStack;   // 크리 적중률
    public float criDmgStack;    // 크리 데미지

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
            // 누적 데이터 저장
            writer.Write(dmgStack);
            writer.Write(criRateStack);
            writer.Write(criDmgStack);

            // 플레이어 수 저장
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
            Debug.LogWarning("저장된 파일이 없습니다.");
            return;
        }

        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            // 누적 데이터 읽기
            dmgStack = reader.ReadSingle();
            criRateStack = reader.ReadSingle();
            criDmgStack = reader.ReadSingle();

            // 플레이어 수 읽기
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
