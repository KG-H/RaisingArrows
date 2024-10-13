using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class ProduceBow : MonoBehaviour
{
    //Dictionary<int, int> processedSlots = new Dictionary<int, int>(); 

    Quest quest;
    OnOffBtn onOffBtn;
    GameManager gameManager;

    // 화살 텍스트
    public TextMeshProUGUI text_MaxBow;
    public TextMeshProUGUI text_CurBow;

    public GameObject go_slotParent; // 부모 Slot 
    private Slot[] slots; // 화살 Slot
    public Image image; // 제작 화살 버튼 이미지
    public Button button; // 제작 화살 버튼 
    public bool isClicked = false; // 제작 화살 버튼 클릭시 true
    public float leftTime;

    bool isMake = false; // 자동 제작
    bool isSum = false; // 자동 합성

    void Start()
    {
        quest = FindAnyObjectByType<Quest>();
        onOffBtn = FindAnyObjectByType<OnOffBtn>();
        gameManager = FindAnyObjectByType<GameManager>();
        slots = go_slotParent.GetComponentsInChildren<Slot>();

        // 제작 텍스트 초기화
        text_CurBow.text = gameManager.curBow.ToString();
        text_MaxBow.text = gameManager.MaxBow.ToString();
    }

    void Update()
    {
        if (gameManager.curBow == 0) button.enabled = false; // 현재 제작 가능 화살이 없을경우 버튼 비활성화
        else button.enabled = true;

        gocoolTime(); // 제작 클릭시 쿨타임

        if ((gameManager.curBow < gameManager.MaxBow) && !isClicked) GoCool(); // 제작 가능화살이 최대 제작가능 화살과 같을때까지 쿨타임 활성화

        text_MaxBow.text = gameManager.MaxBow.ToString();

        if (onOffBtn.goAutoMake && !isMake) // 자동 제작 버튼 ON시 자동 제작
        {
            isMake = true;
            Invoke("AutoMake", 0.5f); // 0.5초 주기로 제작
        }

        if (onOffBtn.goAutoMake && !isSum) // 자동 제작 버튼 ON시 자동 합성
        {
            isSum = true;
            Invoke("AutoSum", 1f); // 1초 주기로 합성
        }
    }

    void gocoolTime()
    {
        if (isClicked)
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime * gameManager.BowCoolSpeed;

                if (leftTime < 0)
                {
                    leftTime = 0;
                    gameManager.curBow++; // 현재 제작 가능 화살 1증가
                    text_CurBow.text = gameManager.curBow.ToString();
                    isClicked = false;
                }

                float ratio = 1.0f - (leftTime / gameManager.BowCoolTime);
                if (image) image.fillAmount = ratio;
            }
    }

    void GoCool()
    {
        leftTime = gameManager.BowCoolTime;
        isClicked = true;
    }


    public void StartCoolTime()
    {
        if (gameManager.curBow >= 1)
        {
            gameManager.curBow--; // 현재 제작 화살 -1
            text_CurBow.text = gameManager.curBow.ToString();
        }
        else if (gameManager.curBow == 0) SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
    }

    // 화살 버튼 클릭시
    public void onClickProduceBow()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemCount == 0) // 빈슬롯에 화살 추가
            {
                if (quest.curQuest == 1)
                {
                    quest.questsData[quest.curQuest].curQuestValue++;
                }

                SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
                slots[i].AddItem(gameManager.curLevel);
                return;
            }
        }
    }

    // 자동 제작
    void AutoMake()
    {
        isMake = false;

        if (gameManager.curBow > 0) // 제작 가능 화살이 있을시
        {
            gameManager.curBow--;
            text_CurBow.text = gameManager.curBow.ToString();

            leftTime = gameManager.BowCoolTime;
            isClicked = true;

            if (isClicked)
                if (leftTime > 0)
                {
                    leftTime -= Time.deltaTime * gameManager.BowCoolSpeed;

                    if (leftTime < 0)
                    {
                        leftTime = 0;
                        gameManager.curBow++;
                        text_CurBow.text = gameManager.curBow.ToString();
                        isClicked = false;
                    }

                    float ratio = 1.0f - (leftTime / gameManager.BowCoolTime);
                    if (image) image.fillAmount = ratio;
                }

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemCount == 0)
                {
                    slots[i].AddItem(gameManager.curLevel);
                    return;
                }
            }
        }
    }

    // 자동 합성
    void AutoSum()
    {
        Dictionary<int, int> itemCountMap = new Dictionary<int, int>(); // 아이템 수를 추적하기 위한 HashMap
        HashSet<int> processedSlots = new HashSet<int>(); // 이미 처리된 슬롯을 추적

        // 첫 번째 통과: 아이템 수와 해당 슬롯 인덱스로 HashMap을 채움
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemCount > 0)
            {
                if (!itemCountMap.ContainsKey(slots[i].itemCount))
                {
                    itemCountMap[slots[i].itemCount] = i; // 이 카운트의 첫 번째 인덱스를 저장
                }
                else
                {
                    // 일치하는 카운트를 발견했으므로 첫 번째 슬롯 인덱스와 병합
                    int firstIndex = itemCountMap[slots[i].itemCount];
                    slots[firstIndex].itemCount++; // 첫 번째 슬롯의 아이템 수를 증가시킴
                    slots[firstIndex].text_Count.text = slots[firstIndex].itemCount.ToString();
                    slots[firstIndex].SetColor(1);

                    itemCountMap.Remove(slots[i].itemCount); // itemCount가 증가했으므로 이전 Count는 제거
                    if(!itemCountMap.ContainsKey(++slots[i].itemCount)) itemCountMap[++slots[i].itemCount] = firstIndex; // 키값이 없다면 저장


                    SoundManager.instance.SFXPlay("Sound", SoundManager.instance.SuccesBow);

                    // 두 번째 슬롯을 비움
                    slots[i].itemCount = 0;
                    slots[i].text_Count.text = ""; // 비워진 슬롯에 대해 빈 텍스트 설정
                    slots[i].SetColor(0); // 색상 재설정

                    // 퀘스트 조건 처리
                    if (quest.curQuest == 8 && slots[firstIndex].itemCount == 7)
                    {
                        quest.questsData[quest.curQuest].curQuestValue++;
                    }

                }
            }

            isSum = false;

        }
    }



}
