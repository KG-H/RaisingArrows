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

    // 플레이어 강화 텍스트
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

    // 기본 데미지 증가 버튼 클릭
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

            playerStat.dmgStack += 5; // 플레이어 기본 데미지 스텟 증가
        }
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }

    // 크리 적중률 버튼 클릭
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

            playerStat.criRateStack += 5; // 플레이어 크리 적중률 스텟 증가
        }
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }

    // 크리 데미지 버튼 클릭
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

            playerStat.criDmgStack += 0.1f; // 플레이어 크리 데미지 스텟 증가
        }
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }

    // 담장 최대 체력증가 버튼 클릭
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

            gameManager.maxHP += 50f; // 담장 최대 체력 증가
        }
    
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }
    
    // 현재 담장 체력증가 버튼 클릭
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

            // 현재 담장 체력 회복
            if (gameManager.curHP + 50f >= gameManager.maxHP) gameManager.curHP = gameManager.maxHP;
            else gameManager.curHP += 50f;


        }
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
        }
    }
}

