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

    // ȭ�� �ؽ�Ʈ
    public TextMeshProUGUI text_MaxBow;
    public TextMeshProUGUI text_CurBow;

    public GameObject go_slotParent; // �θ� Slot 
    private Slot[] slots; // ȭ�� Slot
    public Image image; // ���� ȭ�� ��ư �̹���
    public Button button; // ���� ȭ�� ��ư 
    public bool isClicked = false; // ���� ȭ�� ��ư Ŭ���� true
    public float leftTime;

    bool isMake = false; // �ڵ� ����
    bool isSum = false; // �ڵ� �ռ�

    void Start()
    {
        quest = FindAnyObjectByType<Quest>();
        onOffBtn = FindAnyObjectByType<OnOffBtn>();
        gameManager = FindAnyObjectByType<GameManager>();
        slots = go_slotParent.GetComponentsInChildren<Slot>();

        // ���� �ؽ�Ʈ �ʱ�ȭ
        text_CurBow.text = gameManager.curBow.ToString();
        text_MaxBow.text = gameManager.MaxBow.ToString();
    }

    void Update()
    {
        if (gameManager.curBow == 0) button.enabled = false; // ���� ���� ���� ȭ���� ������� ��ư ��Ȱ��ȭ
        else button.enabled = true;

        gocoolTime(); // ���� Ŭ���� ��Ÿ��

        if ((gameManager.curBow < gameManager.MaxBow) && !isClicked) GoCool(); // ���� ����ȭ���� �ִ� ���۰��� ȭ��� ���������� ��Ÿ�� Ȱ��ȭ

        text_MaxBow.text = gameManager.MaxBow.ToString();

        if (onOffBtn.goAutoMake && !isMake) // �ڵ� ���� ��ư ON�� �ڵ� ����
        {
            isMake = true;
            Invoke("AutoMake", 0.5f); // 0.5�� �ֱ�� ����
        }

        if (onOffBtn.goAutoMake && !isSum) // �ڵ� ���� ��ư ON�� �ڵ� �ռ�
        {
            isSum = true;
            Invoke("AutoSum", 1f); // 1�� �ֱ�� �ռ�
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
                    gameManager.curBow++; // ���� ���� ���� ȭ�� 1����
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
            gameManager.curBow--; // ���� ���� ȭ�� -1
            text_CurBow.text = gameManager.curBow.ToString();
        }
        else if (gameManager.curBow == 0) SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
    }

    // ȭ�� ��ư Ŭ����
    public void onClickProduceBow()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemCount == 0) // �󽽷Կ� ȭ�� �߰�
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

    // �ڵ� ����
    void AutoMake()
    {
        isMake = false;

        if (gameManager.curBow > 0) // ���� ���� ȭ���� ������
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

    // �ڵ� �ռ�
    void AutoSum()
    {
        Dictionary<int, int> itemCountMap = new Dictionary<int, int>(); // ������ ���� �����ϱ� ���� HashMap
        HashSet<int> processedSlots = new HashSet<int>(); // �̹� ó���� ������ ����

        // ù ��° ���: ������ ���� �ش� ���� �ε����� HashMap�� ä��
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemCount > 0)
            {
                if (!itemCountMap.ContainsKey(slots[i].itemCount))
                {
                    itemCountMap[slots[i].itemCount] = i; // �� ī��Ʈ�� ù ��° �ε����� ����
                }
                else
                {
                    // ��ġ�ϴ� ī��Ʈ�� �߰������Ƿ� ù ��° ���� �ε����� ����
                    int firstIndex = itemCountMap[slots[i].itemCount];
                    slots[firstIndex].itemCount++; // ù ��° ������ ������ ���� ������Ŵ
                    slots[firstIndex].text_Count.text = slots[firstIndex].itemCount.ToString();
                    slots[firstIndex].SetColor(1);

                    itemCountMap.Remove(slots[i].itemCount); // itemCount�� ���������Ƿ� ���� Count�� ����
                    if(!itemCountMap.ContainsKey(++slots[i].itemCount)) itemCountMap[++slots[i].itemCount] = firstIndex; // Ű���� ���ٸ� ����


                    SoundManager.instance.SFXPlay("Sound", SoundManager.instance.SuccesBow);

                    // �� ��° ������ ���
                    slots[i].itemCount = 0;
                    slots[i].text_Count.text = ""; // ����� ���Կ� ���� �� �ؽ�Ʈ ����
                    slots[i].SetColor(0); // ���� �缳��

                    // ����Ʈ ���� ó��
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
