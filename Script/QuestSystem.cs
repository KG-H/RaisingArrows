using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour, IPointerDownHandler
{
    public GameObject[] GoldBox = new GameObject[5];
    public GameObject[] CrystalBox = new GameObject[5];
    public GameObject Gold; // 보상을 위한 아이템
    public GameObject Crystal; // 보상을 위한 아이템

    public Quest quest;
    GameManager gameManager;

    // 퀘스트 UI
    public Slider Questbar; // 진행 퀘스트 Slidler
    public TextMeshProUGUI QuestTxt; // 퀘스트 텍스트
    public TextMeshProUGUI curValueTxt; // 현재 퀘스트 진행 수치
    public TextMeshProUGUI MaxValueTxt; // 현제 퀘스트 최대 수치
    public TextMeshProUGUI RewardTxt; // 퀘스트 보상 수치
    public TextMeshProUGUI StageTxt; // state 텍스트
    public Image ReWardImg; // 보상 이미지
    public Sprite GoldImg; // 보상이미지를 위한 Sprite
    public Sprite CryImg; // 보상이미지를 위한 Sprite

    public int curQuest; // 현재 진행 중인 퀘스트

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        curQuest = quest.curQuest;
        SetCurQuest(); // 현재 퀘스트 UI 초기화
    }

    void Update()
    {
        HandleHP();
        GocurQuest(curQuest); // 퀘스트 변경시 초기화
        ChangeStage(); // Stage 변경시 초기화
    }

    void ChangeStage()
    {
        if (quest.curQuest < 4) StageTxt.text = "Stage " + gameManager.stage.ToString();

        if (quest.curQuest >= 4 && quest.curQuest <= 8)
        {
            StageTxt.text = "Stage " + gameManager.stage.ToString();
            gameManager.stage = 2;
            gameManager.spawnInterval = 1f; // 
        }

        if (quest.curQuest == 9)
        {
            gameManager.stage = 3;
            StageTxt.text = "Stage " + gameManager.stage.ToString();
        }
    }


    void SetCurQuest()
    {
        // quest 정보들을 받아와 출력
        QuestTxt.text = quest.questsData[curQuest].QuestName;
        curValueTxt.text = quest.questsData[curQuest].curQuestValue.ToString();
        MaxValueTxt.text = quest.questsData[curQuest].maxQuestValue.ToString();
        RewardTxt.text = " X " + quest.questsData[curQuest].Reward.ToString();

        if (quest.questsData[curQuest].isGold)
        {
            ReWardImg.sprite = GoldImg;
        }
        else ReWardImg.sprite = CryImg;
    }

    void GocurQuest(int curQuest)
    {
        Questbar.value = quest.questsData[curQuest].curQuestValue / quest.questsData[curQuest].maxQuestValue;
        curValueTxt.text = quest.questsData[curQuest].curQuestValue.ToString();
    }

    public void HandleHP()
    {
        Questbar.value = Mathf.Lerp(Questbar.value, quest.questsData[curQuest].curQuestValue / quest.questsData[curQuest].maxQuestValue, Time.deltaTime * 10);
    }

    // 퀘스트 완료시 화면 클릭을 통해 실행
    public void OnPointerDown(PointerEventData eventData)
    {
        if (quest.questsData[curQuest].curQuestValue >= quest.questsData[curQuest].maxQuestValue) // 현재 퀘스트 완료 조건이 충족됐을시 
        {
            if (quest.questsData[curQuest].isGold) // 골드 보상
            {
                gameManager.Gold += quest.questsData[curQuest].Reward;
                gameManager.GoldTxt.text = gameManager.Gold.ToString();

                for (int i = 0; i < 5; i++)
                {
                    GoldBox[i] = Instantiate(Gold, transform.position, transform.rotation);
                    GoldItem GoldItem = GoldBox[i].GetComponent<GoldItem>();
                    GoldItem.isReward = true;
                }
            }
            else if (!quest.questsData[curQuest].isGold) // 크리스탈 보상
            {
                gameManager.Crystal += quest.questsData[curQuest].Reward;
                gameManager.CrystalTxt.text = gameManager.Crystal.ToString();

                for (int i = 0; i < 5; i++)
                {
                    CrystalBox[i] = Instantiate(Crystal, transform.position, transform.rotation);
                    CrystalItem CrystalItem = CrystalBox[i].GetComponent<CrystalItem>();
                    CrystalItem.isReward = true;
                }
            }

            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
            quest.curQuest++; // 퀘스트 수치 증가 -> 다움 퀘스트
            curQuest = quest.curQuest;

            SetCurQuest(); // 다움 퀘스트 UI 초기화
        }
    }
}