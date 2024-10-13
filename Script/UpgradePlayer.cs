using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePlayer : MonoBehaviour
{
    Quest quest;
    PlayerStat playerStat;
    GameManager gameManager;

    // �÷��̾� ��ȭ �ؽ�Ʈ
    public TextMeshProUGUI dmgTxt;
    public TextMeshProUGUI CriPlusTxt;
    public TextMeshProUGUI CriDmgTxt;
    public TextMeshProUGUI MaxHPTxt;

    private void Start()
    {
        quest = FindAnyObjectByType<Quest>();
        playerStat = FindAnyObjectByType<PlayerStat>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // �⺻ ������ ���� ��ư Ŭ��
    public void OnClickDmg()
    {
        if (int.Parse(dmgTxt.text) < 5 && gameManager.Gold >= 1000)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            if (quest.curQuest == 4)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }

            gameManager.Gold -= 1000;
            int txt = int.Parse(dmgTxt.text) + 1;
            dmgTxt.text = txt.ToString();

            playerStat.dmgStack += 5; // �÷��̾� �⺻ ������ ���� ����
        }
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }

    // ũ�� ���߷� ��ư Ŭ��
    public void OnClickCriPlus()
    {
        if (int.Parse(CriPlusTxt.text) < 5 && gameManager.Gold >= 1000)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            if (quest.curQuest == 4)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }

            gameManager.Gold -= 1000;
            int txt = int.Parse(CriPlusTxt.text) + 1;
            CriPlusTxt.text = txt.ToString();

            playerStat.criRateStack += 5; // �÷��̾� ũ�� ���߷� ���� ����
        }
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }

    // ũ�� ������ ��ư Ŭ��
    public void OnClickCriDmg()
    {
        if (int.Parse(CriDmgTxt.text) < 5 && gameManager.Gold >= 1000)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            if (quest.curQuest == 4)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }

            gameManager.Gold -= 1000;
            int txt = int.Parse(CriDmgTxt.text) + 1;
            CriDmgTxt.text = txt.ToString();

            playerStat.criDmgStack += 0.1f; // �÷��̾� ũ�� ������ ���� ����
        }
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }

    // ���� �ִ� ü������ ��ư Ŭ��
    public void OnClickMaxHP()
    {
        if (int.Parse(MaxHPTxt.text) < 5 && gameManager.Gold >= 1000)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            if (quest.curQuest == 4)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }

            gameManager.Gold -= 1000;
            int txt = int.Parse(MaxHPTxt.text) + 1;
            MaxHPTxt.text = txt.ToString();

            gameManager.maxHP += 50f; // ���� �ִ� ü�� ����
        }
    
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }
    
    // ���� ���� ü������ ��ư Ŭ��
    public void OnClickHealHP()
    {
        if (gameManager.Gold >= 1000 && gameManager.curHP != gameManager.maxHP)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            if (quest.curQuest == 4)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }

            gameManager.Gold -= 1000;

            // ���� ���� ü�� ȸ��
            if (gameManager.curHP + 50f >= gameManager.maxHP) gameManager.curHP = gameManager.maxHP;
            else gameManager.curHP += 50f;


        }
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }
}

