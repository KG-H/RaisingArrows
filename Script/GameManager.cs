using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    SoundManager soundManager;
    Quest quest;
    PlayerStat playerStat;

    public bool isDie; // 체력 0조건
    public Image DieUI; // 사망시 UI
    public Image clearUI; // 게임 종료시 UI
    public Image[] Map; // 맵 이미지
    public Sprite[] PlayerImg; // 캐릭터 정보창 이미지 Sprite
    public GameObject PlayerInfo; // 캐릭터 정보창
    public Image Playerimg; // 캐릭터 정보창 이미지


    // 담장 체력
    public Slider hpbar;
    public float maxHP = 100f;
    public float curHP = 100f;

    // 텍스트
    public TextMeshProUGUI curHealthTxt;
    public TextMeshProUGUI GoldTxt;
    public TextMeshProUGUI CrystalTxt;
    public TextMeshProUGUI RankTxt;
    public TextMeshProUGUI DmgTxt;
    public TextMeshProUGUI CriRateTxt;
    public TextMeshProUGUI CriDmgTxt;
    public TextMeshProUGUI CurBowTxt;

    // 플레이어
    [System.Serializable]
    public struct PlayerGroup
    {
        public GameObject[] players;
    }
    
    public PlayerGroup[] playerGroup;

    public float Gold;
    public float Crystal;

    public int curBow; // 현재 제작가능한 화살갯수
    public int MaxBow; //제작 가능 화살 최대갯수
    public float BowCoolTime; // 제작 쿨타임 조절
    public float BowCoolSpeed;
    private float coolDown = 10f, coolDownCounter; // 생성 쿨타임

    public int curLevel = 1; // 현재 화살의 레벨

    public Transform[] spawnLocations; // 몬스터가 나타날 위치
    public float spawnInterval; // 몬스터 스폰 간격
    public int stage = 1; // 스테이지 변수 (1, 2, 3 스테이지)

    public bool isClear = false; // 게임 종료 조건

    bool bgSound1 = false;
    bool bgSound2 = false;
    bool bgSound3 = false;


    void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
        quest = FindAnyObjectByType<Quest>();
        playerStat = FindAnyObjectByType<PlayerStat>();
        hpbar.value = curHP / maxHP;

        GoldTxt.text = "0";
        CrystalTxt.text = "0";

        coolDownCounter = coolDown;
        StartCoroutine(SpawnMonsters());
    }

    void Update()
    {
        SetText(); // 텍스트 초기화
        HandleHP(); // 담장 체력 초기화
        SetMap(); // 맵 초기화

        if (isClear) // 게임 클리어 시 종료조건
        {
            isClear = false;
            clearUI.gameObject.SetActive(true);
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.ClearBg);
            SoundManager.instance.EndPlay();
        }

        if (curHP == 0) // 담장 체력 0조건
        {
            isDie = true;
            DieUI.gameObject.SetActive(true);
        }
    }

    void SetText()
    {
        curHealthTxt.text = curHP.ToString();
        GoldTxt.text = Gold.ToString();
        CrystalTxt.text = Crystal.ToString();
    }

    public void HandleHP()
    {
        hpbar.value = Mathf.Lerp(hpbar.value, curHP / maxHP, Time.deltaTime * 10);
    }
    public void StartCor()
    {
        StartCoroutine(SpawnMonsters());
    }

    public void setFool()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Enemy");

        // 배열에 있는 각 몬스터 오브젝트를 비활성화
        foreach (GameObject monster in monsters)
        {
            if (monster.activeInHierarchy) // 활성화된 경우만 비활성화
            {
                monster.GetComponent<BaseEnemy>().DestroyEnemy();
            }
        }
    }

    public IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // 스폰 간격 대기

            int randomMonsterIndex = -1;
            int randomLocationIndex = Random.Range(0, spawnLocations.Length); // 랜덤 위치 선택

            // 스테이지에 따른 랜덤 몬스터 선택
            if (stage == 1)
            {
                randomMonsterIndex = Random.Range(0, 2); // 0 ~ 1

            }
            else if (stage == 2)
            {
                randomMonsterIndex = Random.Range(0, 3); // 0 ~ 2            
            }
            else if (stage == 3)
            {
                randomMonsterIndex = 3; // 고정으로 3번

                GameObject monster = ObjectPool.Instance.GetPooledObject(randomMonsterIndex);
                monster.transform.position = spawnLocations[1].position; // 고정 위치에 스폰            
                monster.SetActive(true);
                monster.GetComponent<BaseEnemy>().SetState(new WalkState());
                break;
            }

            // 랜덤 인덱스가 유효할 때
            if (randomMonsterIndex >= 0 && stage != 3)
            {
                GameObject monster = ObjectPool.Instance.GetPooledObject(randomMonsterIndex);
                if (monster != null) // 유효한 객체 확인
                {
                    monster.transform.position = spawnLocations[randomLocationIndex].position; // 랜덤 위치에 스폰                
                    monster.SetActive(true); // 몬스터 활성화
                    monster.GetComponent<BaseEnemy>().SetState(new WalkState());
                }
            }
        }
    }


    public void ShowPlayerInfo(int ID)
    {
        PlayerInfo.gameObject.SetActive(true);

        // 등급에 따라 텍스트 색을 변경
        if (ID == 6)
        {
            RankTxt.text = "<color=yellow> 전설 등급 </color>";
        }
        else if (ID == 1 || ID == 2)
        {
            RankTxt.text = "<color=blue> 희귀 등급 </color>";
        }
        else if (ID == 0)
        {
            RankTxt.text = "일반 등급";
        }
        else RankTxt.text = "<color=purple> 영웅 등급 </color>";

        Playerimg.sprite = PlayerImg[ID];
        DmgTxt.text = "기본 공격력 : " + playerStat.playersData[ID].PlayerDmg + " + <color=red>" + playerStat.dmgStack + "</color>";
        CriRateTxt.text = "치명타 확률 : " + playerStat.playersData[ID].CritlcalRate + " + <color=red>" + playerStat.criRateStack + "</color>" + " %";
        CriDmgTxt.text = "치명타 데미지 : " + playerStat.playersData[ID].CriticalDmg * 100f + " + <color=red>" + playerStat.criDmgStack * 100f + "</color>" + " %";
        CurBowTxt.text = "화살 레벨 : " + playerStat.playersData[ID].curArrow + "Lv";
    }

    public void PlusArrow(int ID, int Lv) // 플레이어 화살 레벨 증가
    {
        playerStat.playersData[ID].curArrow = Lv;
    }

    public float goDMG(int ID, GameObject arrow) // 화살 데미지 측정
    {
        int random = Random.Range(1, 101);
        if (random <= (playerStat.playersData[ID].CritlcalRate + playerStat.criRateStack)) // 크리 적중
        {
            playerStat.playersData[ID].ArrowDmg = (((playerStat.playersData[ID].PlayerDmg + playerStat.dmgStack) + playerStat.playersData[ID].curArrow * 10f))
                * (playerStat.playersData[ID].CriticalDmg + playerStat.criDmgStack);
            arrow.GetComponent<Arrow>().critical = true;
        }
        else playerStat.playersData[ID].ArrowDmg = (playerStat.playersData[ID].PlayerDmg + playerStat.dmgStack) + (playerStat.playersData[ID].curArrow * 10f);

        return playerStat.playersData[ID].ArrowDmg; // 화살의 최종 데미지를 계산후 전달
    }

    public void setQuest() // 사망시 진행중인 퀘스트를 0으로 초기화
    {
        quest.questsData[quest.curQuest].curQuestValue = 0;
    }

    void SetMap()
    {
        if (stage == 1 && !bgSound1)
        {
            bgSound1 = true;
            bgSound2 = false;
            bgSound3 = false;
            Map[0].gameObject.SetActive(true);
            Map[1].gameObject.SetActive(false);
            Map[2].gameObject.SetActive(false);

            soundManager.BgSoundPlay(stage);
        }

        if (stage == 2 && !bgSound2)
        {
            bgSound1 = false;
            bgSound2 = true;
            bgSound3 = false;

            Map[0].gameObject.SetActive(false);
            Map[1].gameObject.SetActive(true);
            Map[2].gameObject.SetActive(false);

            soundManager.BgSoundPlay(stage);
        }

        if (stage == 3 && !bgSound3)
        {
            bgSound1 = false;
            bgSound2 = false;
            bgSound3 = true;

            Map[0].gameObject.SetActive(false);
            Map[1].gameObject.SetActive(false);
            Map[2].gameObject.SetActive(true);

            soundManager.BgSoundPlay(stage);
        }
    }

}
