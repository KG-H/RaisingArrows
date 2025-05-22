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

    public int isPlayer = -1; // 0�̸� �̸� Player, -1�̸� ȭ�� Slot
    public int itemCount; // ȭ�� ����
    public Image itemImg; //ȭ�� �̹���
    public TextMeshProUGUI text_Count; // ȭ�� ���� �ؽ�Ʈ
    private Canvas canvas;

    void Start()
    {
        quest = FindAnyObjectByType<Quest>();
        gameManager = FindAnyObjectByType<GameManager>();
        canvas = GetComponentInParent<Canvas>();  // �θ� ĵ������ ����
    }

    public void SetColor(float _alpha)
    {
        Color color = itemImg.color;
        color.a = _alpha;
        itemImg.color = color;
    }

    // ȭ�� ���۽� Slot �ʱⰪ ����
    public void AddItem(int _count = 1)
    {
        itemCount = _count;

        text_Count.text = itemCount.ToString();
        SetColor(1); // ���İ��� 1�� ����
    }

    // ĳ���� ����â UI Ŭ��
    public void OnPointerDown(PointerEventData eventData)
    {
        // isPlayer�� 0�̻� -> �÷��̾�
        if (isPlayer > -1)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
            gameManager.ShowPlayerInfo(isPlayer);
        }

    }

    // �巡�� ���۽�
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ������ ī��Ʈ�� 1�̻��� ���Ը� �̵�
        if (itemCount > 0 && isPlayer == -1)
        {
            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out worldPoint); // ���� ������ �ʱ���ġ ���

            DragSlot.instance.transform.position = worldPoint; // ���� ������ �ʱ���ġ

            // ���� ������ ������ ���� ���Կ� ����
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.ItemCount = itemCount;
            DragSlot.instance.text_Count.text = itemCount.ToString();
            DragSlot.instance.SetColor(1);
        }
    }

    // �巡�� ��
    public void OnDrag(PointerEventData eventData)
    {
        if (itemCount > 0 && isPlayer == -1)
        {
            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out worldPoint); // �巡�� ��ġ

            DragSlot.instance.transform.position = worldPoint; // ���� ������ �巡������ ��ġ�� ����
        }
    }

    // �巡�� Drop��
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null && isPlayer == -1)
        {
            // �� ���Կ� Drop
            if (itemCount == 0)
            {
                itemCount = DragSlot.instance.ItemCount;
                MoveSlot(); // ���� �̵�
            }
            else if (itemCount != DragSlot.instance.dragSlot.itemCount) // ȭ���� ������ �ٸ� Slot�� Drop
            {
                ChangeSlot(); // ȭ���� ������ ����
            }
            else if (itemCount == DragSlot.instance.dragSlot.itemCount) // ���� ȭ���� ������ Sloot�� Drop
            {
                SoundManager.instance.SFXPlay("Sound", SoundManager.instance.SuccesBow);
                itemCount = DragSlot.instance.ItemCount + 1; // ȭ�� ���� ����

                if (quest.curQuest == 2)
                {
                    quest.questsData[quest.curQuest].curQuestValue++;
                }

                if (quest.curQuest == 8 && itemCount == 7)
                {
                    quest.questsData[quest.curQuest].curQuestValue++;
                }

                MoveSlot(); // ���� �̵�
            }
        }
        else if (DragSlot.instance.dragSlot != null && isPlayer > -1) // �÷��̾� Slot�� Drop
        {
            if (DragSlot.instance.dragSlot.itemCount > itemCount)
            {
                UpgradeBow(); // �÷��̾� Slot�� ���� Slot���� ����
            }
        }
    }

    // DragSlot.instance.dragSlot == ���ʽ���, DragSlot.instance == ���ʽ��� ����, ���ݳ��� ������ġ
    private void MoveSlot()
    {
        text_Count.text = itemCount.ToString();
        SetColor(1);

        // ���� ���� �ʱ�ȭ
        DragSlot.instance.dragSlot.SetColor(0);
        DragSlot.instance.dragSlot.itemCount = 0;
        DragSlot.instance.dragSlot.text_Count.text = "";
    }
    private void ChangeSlot()
    {
        int Cnt = itemCount; // �ʱ� ȭ�� ���� ����

        // Drop�� Slot�� ���罽�� ���� �ֽ�ȭ
        itemCount = DragSlot.instance.ItemCount;
        text_Count.text = itemCount.ToString();
        SetColor(1);

        // �ʱ� ȭ�� ������ ���� ���Կ� ���� -> Swap
        DragSlot.instance.dragSlot.itemCount = Cnt;
        DragSlot.instance.dragSlot.text_Count.text = Cnt.ToString();
    }
    private void UpgradeBow()
    {
        if (quest.curQuest == 3)
        {
            quest.questsData[quest.curQuest].curQuestValue++;
        }

        itemCount = DragSlot.instance.ItemCount; // �÷��̾� ���� �ؽ�Ʈ�� ���� ���� �ؽ�Ʈ ����
        text_Count.text = itemCount.ToString();

        // ���� ���� �ʱ�ȭ
        DragSlot.instance.dragSlot.SetColor(0);
        DragSlot.instance.dragSlot.itemCount = 0;
        DragSlot.instance.dragSlot.text_Count.text = "";

        gameManager.PlusArrow(isPlayer, DragSlot.instance.ItemCount); // �÷��̾� ȭ�� ���� ����

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.text_Count.text = "";
        DragSlot.instance.dragSlot = null;
    }

}
