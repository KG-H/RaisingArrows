using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerStat : MonoBehaviour
{
    public PlayerData[] playersData; // 여러 플레이어 데이터를 위한 배열
    public float dmgStack = 0;       // 기본 데미지 수치
    public float criRateStack = 0;   // 크리 적중률 수치
    public float criDmgStack = 0;    // 크리 데미지 수치

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
            playersData = container.players; // JSON에서 로드한 데이터로 배열 초기화
            dmgStack = container.dmgStack;   // 기본 데미지 데이터 로드
            criRateStack = container.criRateStack; // 크리 적중률 데이터 로드
            criDmgStack = container.criDmgStack;   // 크리 데미지 데이터 로드
        }
        else
        {
            Debug.LogError("파일을 찾을 수 없습니다: " + path);
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
    public PlayerData[] players; // PlayerData 배열을 감싸는 클래스
    public float dmgStack;       // 기본 데미지 데이터
    public float criRateStack;   // 크리 적중률 데이터
    public float criDmgStack;    // 크리 데미지 데이터
}
