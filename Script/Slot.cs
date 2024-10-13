using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler
{
    Quest quest;
    GameManager gameManager;

    public int isPlayer = -1; // 0이면 이면 Player, -1이면 화살 Slot
    public int itemCount; // 화살 레벨
    public Image itemImg; //화살 이미지
    public TextMeshProUGUI text_Count; // 화살 레벨 텍스트
    private Canvas canvas;

    void Start()
    {
        quest = FindAnyObjectByType<Quest>();
        gameManager = FindAnyObjectByType<GameManager>();
        canvas = GetComponentInParent<Canvas>();  // 부모 캔버스를 참조
    }

    public void SetColor(float _alpha)
    {
        Color color = itemImg.color;
        color.a = _alpha;
        itemImg.color = color;
    }

    // 화살 제작시 Slot 초기값 설정
    public void AddItem(int _count = 1)
    {
        itemCount = _count;

        text_Count.text = itemCount.ToString();
        SetColor(1); // 알파값을 1로 설정
    }

    // 캐릭터 정보창 UI 클릭
    public void OnPointerDown(PointerEventData eventData)
    {
        // isPlayer가 0이상 -> 플레이어
        if (isPlayer > -1)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
            gameManager.ShowPlayerInfo(isPlayer);
        }

    }

    // 드래그 시작시
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 아이템 카운트가 1이상인 슬롯만 이동
        if (itemCount > 0 && isPlayer == -1)
        {
            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out worldPoint); // 복사 슬롯의 초기위치 계산

            DragSlot.instance.transform.position = worldPoint; // 복사 슬롯의 초기위치

            // 현재 슬롯의 정보를 복사 슬롯에 저장
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.ItemCount = itemCount;
            DragSlot.instance.text_Count.text = itemCount.ToString();
            DragSlot.instance.SetColor(1);
        }
    }

    // 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (itemCount > 0 && isPlayer == -1)
        {
            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out worldPoint); // 드래그 위치

            DragSlot.instance.transform.position = worldPoint; // 복사 슬롯을 드래그중인 위치에 저장
        }
    }

    // 드래그 Drop시
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null && isPlayer == -1)
        {
            // 빈 슬롯에 Drop
            if (itemCount == 0)
            {
                itemCount = DragSlot.instance.ItemCount;
                MoveSlot(); // 슬롯 이동
            }
            else if (itemCount != DragSlot.instance.dragSlot.itemCount) // 화살의 레벨이 다른 Slot에 Drop
            {
                ChangeSlot(); // 화살의 정보를 변경
            }
            else if (itemCount == DragSlot.instance.dragSlot.itemCount) // 같은 화살의 레벨의 Sloot에 Drop
            {
                SoundManager.instance.SFXPlay("Sound", SoundManager.instance.SuccesBow);
                itemCount = DragSlot.instance.ItemCount + 1; // 화살 레벨 증가

                if (quest.curQuest == 2)
                {
                    quest.questsData[quest.curQuest].curQuestValue++;
                }

                if (quest.curQuest == 8 && itemCount == 7)
                {
                    quest.questsData[quest.curQuest].curQuestValue++;
                }

                MoveSlot(); // 슬롯 이동
            }
        }
        else if (DragSlot.instance.dragSlot != null && isPlayer > -1) // 플레이어 Slot에 Drop
        {
            if (DragSlot.instance.dragSlot.itemCount > itemCount)
            {
                UpgradeBow(); // 플레이어 Slot에 현재 Slot정보 저장
            }
        }
    }

    // DragSlot.instance.dragSlot == 최초슬롯, DragSlot.instance == 최초슬롯 복사, 지금나는 최종위치
    private void MoveSlot()
    {
        text_Count.text = itemCount.ToString();
        SetColor(1);

        // 복사 슬롯 초기화
        DragSlot.instance.dragSlot.SetColor(0);
        DragSlot.instance.dragSlot.itemCount = 0;
        DragSlot.instance.dragSlot.text_Count.text = "";
    }
    private void ChangeSlot()
    {
        int Cnt = itemCount; // 초기 화살 레벨 저장

        // Drop된 Slot에 복사슬롯 정보 최신화
        itemCount = DragSlot.instance.ItemCount;
        text_Count.text = itemCount.ToString();
        SetColor(1);

        // 초기 화살 레벨을 최초 슬롯에 저장 -> Swap
        DragSlot.instance.dragSlot.itemCount = Cnt;
        DragSlot.instance.dragSlot.text_Count.text = Cnt.ToString();
    }
    private void UpgradeBow()
    {
        if (quest.curQuest == 3)
        {
            quest.questsData[quest.curQuest].curQuestValue++;
        }

        itemCount = DragSlot.instance.ItemCount; // 플레이어 슬롯 텍스트에 복사 슬롯 텍스트 적용
        text_Count.text = itemCount.ToString();

        // 복사 슬롯 초기화
        DragSlot.instance.dragSlot.SetColor(0);
        DragSlot.instance.dragSlot.itemCount = 0;
        DragSlot.instance.dragSlot.text_Count.text = "";

        gameManager.PlusArrow(isPlayer, DragSlot.instance.ItemCount); // 플레이어 화살 레벨 증가

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.text_Count.text = "";
        DragSlot.instance.dragSlot = null;
    }

}
