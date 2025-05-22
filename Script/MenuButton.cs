using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    // �޴� UI
    public GameObject ProduceUI;
    public GameObject UpgradeUI;
    public GameObject SpecialUI;
    public GameObject RandomUI;

    public GameObject mySelf;

    // ����Box UI
    public GameObject Info1_1;
    public GameObject Info1_2;
    public GameObject Info2_1;
    public GameObject Info2_2;
    public GameObject Info2_3;
    public GameObject Info3_1;

    // ��� UI
    public GameObject Info1_1Rock;
    public GameObject Info1_2Rock;
    public GameObject Info2_1Rock;
    public GameObject Info2_2Rock;
    public GameObject Info2_3Rock;
    public GameObject Info3_1Rock;

    // ĳ���� ���� �ؽ�Ʈ UI
    public GameObject Info1_1Txt;
    public GameObject Info1_2Txt;
    public GameObject Info2_1Txt;
    public GameObject Info2_2Txt;
    public GameObject Info2_3Txt;
    public GameObject Info3_1Txt;

    GameManager gameManager;
    Quest quest;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        quest = FindAnyObjectByType<Quest>();
    }

    public void OnClickPro() // ���� UI
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
        ProduceUI.gameObject.SetActive(true);
        UpgradeUI.gameObject.SetActive(false);
        SpecialUI.gameObject.SetActive(false);
        RandomUI.gameObject.SetActive(false);
    }
    public void OnClickUpg() // ��ȭ UI
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
        ProduceUI.gameObject.SetActive(false);
        UpgradeUI.gameObject.SetActive(true);
        SpecialUI.gameObject.SetActive(false);
        RandomUI.gameObject.SetActive(false);
    }
    public void OnClickSpeci() // Ư�� UI
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
        ProduceUI.gameObject.SetActive(false);
        UpgradeUI.gameObject.SetActive(false);
        SpecialUI.gameObject.SetActive(true);
        RandomUI.gameObject.SetActive(false);
    }
    public void OnClickRand() // �̱� UI
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
        ProduceUI.gameObject.SetActive(false);
        UpgradeUI.gameObject.SetActive(false);
        SpecialUI.gameObject.SetActive(false);
        RandomUI.gameObject.SetActive(true);
    }

    public void OnClickRandBox() // �̱� ��ư Ŭ����
    {
        float rand = Random.value;

        if (gameManager.Crystal >= 1000)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            if (quest.curQuest == 6)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }

            gameManager.Crystal -= 1000;
            if (rand < 0.1f) // ���� �⹰
            {  
                Info3_1.gameObject.SetActive(true);
                gameManager.playerGroup[2].players[0].SetActive(true);

                Info3_1Rock.gameObject.SetActive(false); // ��� ����
                Info3_1Txt.gameObject.SetActive(true);
            }
            else if (rand < 0.3f) // ���� �⹰
            {
                int rand2 = Random.Range(0, 3);   

                if (rand2 == 0)
                {
                    Info2_1.gameObject.SetActive(true);

                    gameManager.playerGroup[1].players[rand2].SetActive(true);

                    Info2_1Rock.gameObject.SetActive(false);
                    Info2_1Txt.gameObject.SetActive(true);
                }
                else if (rand2 == 1)
                {
                    Info2_2.gameObject.SetActive(true);

                    gameManager.playerGroup[1].players[rand2].SetActive(true);

                    Info2_2Rock.gameObject.SetActive(false);
                    Info2_2Txt.gameObject.SetActive(true);
                }
                else
                {
                    Info2_3.gameObject.SetActive(true);

                    gameManager.playerGroup[1].players[rand2].SetActive(true);

                    Info2_3Rock.gameObject.SetActive(false);
                    Info2_3Txt.gameObject.SetActive(true);
                }
            }
            else // ��� �⹰
            {
                int rand2 = Random.Range(0, 2);

                if (rand2 == 0)
                {
                    Info1_1.gameObject.SetActive(true);

                    gameManager.playerGroup[0].players[rand2].SetActive(true);

                    Info1_1Rock.gameObject.SetActive(false);
                    Info1_1Txt.gameObject.SetActive(true);
                }
                else
                {
                    Info1_2.gameObject.SetActive(true);

                    gameManager.playerGroup[0].players[rand2].SetActive(true);
                    Info1_2Rock.gameObject.SetActive(false);
                    Info1_2Txt.gameObject.SetActive(true);
                }
            }
            
        }
        else
        {

        }
    }

    public void OnClickResetGame() // �ٽ��ϱ� ��ư
    {
        if (gameManager.Crystal >= 100)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
            gameManager.Crystal -= 100;

            gameManager.setFool(); // Ǯ �ʱ�ȭ
            gameManager.setQuest(); // ����Ʈ �ʱ�ȭ
            gameManager.isDie = false; // ��� ó�� ��Ȱ��ȭ
            gameManager.curHP = gameManager.maxHP; // ���� ü�� �ʱ�ȭ
            if (gameManager.stage == 3) gameManager.StartCor(); // ������Ʈ Ǯ�� Ȱ��ȭ

            gameManager.DieUI.gameObject.SetActive(false);
        }
    }

    public void OutBtn() // �ݱ� ��ư
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.CloseBnt);
        mySelf.gameObject.SetActive(false);
    }
     
}
