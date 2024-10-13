using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeBow : MonoBehaviour
{
    Quest quest;
    GameManager gameManager;
    ProduceBow produceBow;

    // 화살 업그레이드 텍스트
    public TextMeshProUGUI BowCoolTxt;
    public TextMeshProUGUI BowMaxTxt;
    public TextMeshProUGUI BowLevelTxt;

    void Start()
    {
        quest = FindAnyObjectByType<Quest>();
        gameManager = FindAnyObjectByType<GameManager>();
        produceBow = FindAnyObjectByType<ProduceBow>();
    }

    // 화살 쿨타임 감소 버튼 클릭
    public void BowCoolTime()
    {
        if (int.Parse(BowCoolTxt.text) < 5 && gameManager.Gold >= 1000)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            if (quest.curQuest == 5)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }

            gameManager.Gold -= 1000;
            int txt = int.Parse(BowCoolTxt.text) + 1;
            BowCoolTxt.text = txt.ToString();

            gameManager.BowCoolTime -= 4f;
        }
        else SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
    }

    // 최대 제작 화살 증가 버튼 클릭
    public void BowMax()
    {
        if (int.Parse(BowMaxTxt.text) < 5 && gameManager.Gold >= 1000)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            if (quest.curQuest == 5)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }

            gameManager.Gold -= 1000;
            int txt = int.Parse(BowMaxTxt.text) + 1;
            BowMaxTxt.text = txt.ToString();

            gameManager.MaxBow++;
        }
        else SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
    }

    // 제작 화살 레벨 증가 버튼 클릭
    public void BowLevel()
    {
        if (int.Parse(BowLevelTxt.text) < 5 && gameManager.Gold >= 1000)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
            if (quest.curQuest == 5)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }

            gameManager.Gold -= 1000;
            int txt = int.Parse(BowLevelTxt.text) + 1;
            BowLevelTxt.text = txt.ToString();

            gameManager.curLevel++;
        }
        else SoundManager.instance.SFXPlay("Sound", SoundManager.instance.FailClick);
    }

}
