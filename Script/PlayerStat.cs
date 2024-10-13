using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerStat : MonoBehaviour
{
    public PlayerData[] playersData; // ���� �÷��̾� �����͸� ���� �迭
    public float dmgStack = 0;       // �⺻ ������ ��ġ
    public float criRateStack = 0;   // ũ�� ���߷� ��ġ
    public float criDmgStack = 0;    // ũ�� ������ ��ġ

    [ContextMenu("TO Json Data")]
    void SavePlayersToJson()
    {
        string jsonData = JsonUtility.ToJson(new PlayerDataContainer
        {
            players = playersData,
            dmgStack = dmgStack,
            criRateStack = criRateStack,
            criDmgStack = criDmgStack
        }, true);
        string path = Path.Combine(Application.dataPath, "playerData.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From Json Data")]
    void LoadPlayersFromJson()
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            PlayerDataContainer container = JsonUtility.FromJson<PlayerDataContainer>(jsonData);
            playersData = container.players; // JSON���� �ε��� �����ͷ� �迭 �ʱ�ȭ
            dmgStack = container.dmgStack;   // �⺻ ������ ������ �ε�
            criRateStack = container.criRateStack; // ũ�� ���߷� ������ �ε�
            criDmgStack = container.criDmgStack;   // ũ�� ������ ������ �ε�
        }
        else
        {
            Debug.LogError("������ ã�� �� �����ϴ�: " + path);
        }
    }
}

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

[System.Serializable]
public class PlayerDataContainer
{
    public PlayerData[] players; // PlayerData �迭�� ���δ� Ŭ����
    public float dmgStack;       // �⺻ ������ ������
    public float criRateStack;   // ũ�� ���߷� ������
    public float criDmgStack;    // ũ�� ������ ������
}
