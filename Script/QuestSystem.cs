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
    public GameObject Gold; // ������ ���� ������
    public GameObject Crystal; // ������ ���� ������

    public Quest quest;
    GameManager gameManager;

    // ����Ʈ UI
    public Slider Questbar; // ���� ����Ʈ Slidler
    public TextMeshProUGUI QuestTxt; // ����Ʈ �ؽ�Ʈ
    public TextMeshProUGUI curValueTxt; // ���� ����Ʈ ���� ��ġ
    public TextMeshProUGUI MaxValueTxt; // ���� ����Ʈ �ִ� ��ġ
    public TextMeshProUGUI RewardTxt; // ����Ʈ ���� ��ġ
    public TextMeshProUGUI StageTxt; // state �ؽ�Ʈ
    public Image ReWardImg; // ���� �̹���
    public Sprite GoldImg; // �����̹����� ���� Sprite
    public Sprite CryImg; // �����̹����� ���� Sprite

    public int curQuest; // ���� ���� ���� ����Ʈ

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        curQuest = quest.curQuest;
        SetCurQuest(); // ���� ����Ʈ UI �ʱ�ȭ
    }

    void Update()
    {
        HandleHP();
        GocurQuest(curQuest); // ����Ʈ ����� �ʱ�ȭ
        ChangeStage(); // Stage ����� �ʱ�ȭ
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
        // quest �������� �޾ƿ� ���
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

    // ����Ʈ �Ϸ�� ȭ�� Ŭ���� ���� ����
    public void OnPointerDown(PointerEventData eventData)
    {
        if (quest.questsData[curQuest].curQuestValue >= quest.questsData[curQuest].maxQuestValue) // ���� ����Ʈ �Ϸ� ������ ���������� 
        {
            if (quest.questsData[curQuest].isGold) // ��� ����
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
            else if (!quest.questsData[curQuest].isGold) // ũ����Ż ����
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
            quest.curQuest++; // ����Ʈ ��ġ ���� -> �ٿ� ����Ʈ
            curQuest = quest.curQuest;

            SetCurQuest(); // �ٿ� ����Ʈ UI �ʱ�ȭ
        }
    }
}