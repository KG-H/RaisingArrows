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

    public bool isDie; // ü�� 0����
    public Image DieUI; // ����� UI
    public Image clearUI; // ���� ����� UI
    public Image[] Map; // �� �̹���
    public Sprite[] PlayerImg; // ĳ���� ����â �̹��� Sprite
    public GameObject PlayerInfo; // ĳ���� ����â
    public Image Playerimg; // ĳ���� ����â �̹���


    // ���� ü��
    public Slider hpbar;
    public float maxHP = 100f;
    public float curHP = 100f;

    // �ؽ�Ʈ
    public TextMeshProUGUI curHealthTxt;
    public TextMeshProUGUI GoldTxt;
    public TextMeshProUGUI CrystalTxt;
    public TextMeshProUGUI RankTxt;
    public TextMeshProUGUI DmgTxt;
    public TextMeshProUGUI CriRateTxt;
    public TextMeshProUGUI CriDmgTxt;
    public TextMeshProUGUI CurBowTxt;

    // �÷��̾�
    [System.Serializable]
    public struct PlayerGroup
    {
        public GameObject[] players;
    }
    
    public PlayerGroup[] playerGroup;

    public float Gold;
    public float Crystal;

    public int curBow; // ���� ���۰����� ȭ�찹��
    public int MaxBow; //���� ���� ȭ�� �ִ밹��
    public float BowCoolTime; // ���� ��Ÿ�� ����
    public float BowCoolSpeed;
    private float coolDown = 10f, coolDownCounter; // ���� ��Ÿ��

    public int curLevel = 1; // ���� ȭ���� ����

    public Transform[] spawnLocations; // ���Ͱ� ��Ÿ�� ��ġ
    public float spawnInterval; // ���� ���� ����
    public int stage = 1; // �������� ���� (1, 2, 3 ��������)

    public bool isClear = false; // ���� ���� ����

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
        SetText(); // �ؽ�Ʈ �ʱ�ȭ
        HandleHP(); // ���� ü�� �ʱ�ȭ
        SetMap(); // �� �ʱ�ȭ

        if (isClear) // ���� Ŭ���� �� ��������
        {
            isClear = false;
            clearUI.gameObject.SetActive(true);
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.ClearBg);
            SoundManager.instance.EndPlay();
        }

        if (curHP == 0) // ���� ü�� 0����
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

        // �迭�� �ִ� �� ���� ������Ʈ�� ��Ȱ��ȭ
        foreach (GameObject monster in monsters)
        {
            if (monster.activeInHierarchy) // Ȱ��ȭ�� ��츸 ��Ȱ��ȭ
            {
                monster.GetComponent<BaseEnemy>().DestroyEnemy();
            }
        }
    }

    public IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // ���� ���� ���

            int randomMonsterIndex = -1;
            int randomLocationIndex = Random.Range(0, spawnLocations.Length); // ���� ��ġ ����

            // ���������� ���� ���� ���� ����
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
                randomMonsterIndex = 3; // �������� 3��

                GameObject monster = ObjectPool.Instance.GetPooledObject(randomMonsterIndex);
                monster.transform.position = spawnLocations[1].position; // ���� ��ġ�� ����            
                monster.SetActive(true);
                monster.GetComponent<BaseEnemy>().SetState(new WalkState());
                break;
            }

            // ���� �ε����� ��ȿ�� ��
            if (randomMonsterIndex >= 0 && stage != 3)
            {
                GameObject monster = ObjectPool.Instance.GetPooledObject(randomMonsterIndex);
                if (monster != null) // ��ȿ�� ��ü Ȯ��
                {
                    monster.transform.position = spawnLocations[randomLocationIndex].position; // ���� ��ġ�� ����                
                    monster.SetActive(true); // ���� Ȱ��ȭ
                    monster.GetComponent<BaseEnemy>().SetState(new WalkState());
                }
            }
        }
    }


    public void ShowPlayerInfo(int ID)
    {
        PlayerInfo.gameObject.SetActive(true);

        // ��޿� ���� �ؽ�Ʈ ���� ����
        if (ID == 6)
        {
            RankTxt.text = "<color=yellow> ���� ��� </color>";
        }
        else if (ID == 1 || ID == 2)
        {
            RankTxt.text = "<color=blue> ��� ��� </color>";
        }
        else if (ID == 0)
        {
            RankTxt.text = "�Ϲ� ���";
        }
        else RankTxt.text = "<color=purple> ���� ��� </color>";

        Playerimg.sprite = PlayerImg[ID];
        DmgTxt.text = "�⺻ ���ݷ� : " + playerStat.playersData[ID].PlayerDmg + " + <color=red>" + playerStat.dmgStack + "</color>";
        CriRateTxt.text = "ġ��Ÿ Ȯ�� : " + playerStat.playersData[ID].CritlcalRate + " + <color=red>" + playerStat.criRateStack + "</color>" + " %";
        CriDmgTxt.text = "ġ��Ÿ ������ : " + playerStat.playersData[ID].CriticalDmg * 100f + " + <color=red>" + playerStat.criDmgStack * 100f + "</color>" + " %";
        CurBowTxt.text = "ȭ�� ���� : " + playerStat.playersData[ID].curArrow + "Lv";
    }

    public void PlusArrow(int ID, int Lv) // �÷��̾� ȭ�� ���� ����
    {
        playerStat.playersData[ID].curArrow = Lv;
    }

    public float goDMG(int ID, GameObject arrow) // ȭ�� ������ ����
    {
        int random = Random.Range(1, 101);
        if (random <= (playerStat.playersData[ID].CritlcalRate + playerStat.criRateStack)) // ũ�� ����
        {
            playerStat.playersData[ID].ArrowDmg = (((playerStat.playersData[ID].PlayerDmg + playerStat.dmgStack) + playerStat.playersData[ID].curArrow * 10f))
                * (playerStat.playersData[ID].CriticalDmg + playerStat.criDmgStack);
            arrow.GetComponent<Arrow>().critical = true;
        }
        else playerStat.playersData[ID].ArrowDmg = (playerStat.playersData[ID].PlayerDmg + playerStat.dmgStack) + (playerStat.playersData[ID].curArrow * 10f);

        return playerStat.playersData[ID].ArrowDmg; // ȭ���� ���� �������� ����� ����
    }

    public void setQuest() // ����� �������� ����Ʈ�� 0���� �ʱ�ȭ
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
