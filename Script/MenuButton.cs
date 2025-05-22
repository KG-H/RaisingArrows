using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    // 메뉴 UI
    public GameObject ProduceUI;
    public GameObject UpgradeUI;
    public GameObject SpecialUI;
    public GameObject RandomUI;

    public GameObject mySelf;

    // 랜덤Box UI
    public GameObject Info1_1;
    public GameObject Info1_2;
    public GameObject Info2_1;
    public GameObject Info2_2;
    public GameObject Info2_3;
    public GameObject Info3_1;

    // 잠금 UI
    public GameObject Info1_1Rock;
    public GameObject Info1_2Rock;
    public GameObject Info2_1Rock;
    public GameObject Info2_2Rock;
    public GameObject Info2_3Rock;
    public GameObject Info3_1Rock;

    // 캐릭터 정보 텍스트 UI
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

    public void OnClickPro() // 제작 UI
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
        ProduceUI.gameObject.SetActive(true);
        UpgradeUI.gameObject.SetActive(false);
        SpecialUI.gameObject.SetActive(false);
        RandomUI.gameObject.SetActive(false);
    }
    public void OnClickUpg() // 강화 UI
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
        ProduceUI.gameObject.SetActive(false);
        UpgradeUI.gameObject.SetActive(true);
        SpecialUI.gameObject.SetActive(false);
        RandomUI.gameObject.SetActive(false);
    }
    public void OnClickSpeci() // 특수 UI
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
        ProduceUI.gameObject.SetActive(false);
        UpgradeUI.gameObject.SetActive(false);
        SpecialUI.gameObject.SetActive(true);
        RandomUI.gameObject.SetActive(false);
    }
    public void OnClickRand() // 뽑기 UI
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
        ProduceUI.gameObject.SetActive(false);
        UpgradeUI.gameObject.SetActive(false);
        SpecialUI.gameObject.SetActive(false);
        RandomUI.gameObject.SetActive(true);
    }

    public void OnClickRandBox() // 뽑기 버튼 클릭시
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
            if (rand < 0.1f) // 전설 기물
            {  
                Info3_1.gameObject.SetActive(true);
                gameManager.playerGroup[2].players[0].SetActive(true);

                Info3_1Rock.gameObject.SetActive(false); // 잠금 해제
                Info3_1Txt.gameObject.SetActive(true);
            }
            else if (rand < 0.3f) // 영웅 기물
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
            else // 희귀 기물
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

    public void OnClickResetGame() // 다시하기 버튼
    {
        if (gameManager.Crystal >= 100)
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
            gameManager.Crystal -= 100;

            gameManager.setFool(); // 풀 초기화
            gameManager.setQuest(); // 퀘스트 초기화
            gameManager.isDie = false; // 사망 처리 비활성화
            gameManager.curHP = gameManager.maxHP; // 담장 체력 초기화
            if (gameManager.stage == 3) gameManager.StartCor(); // 오브젝트 풀링 활성화

            gameManager.DieUI.gameObject.SetActive(false);
        }
    }

    public void OutBtn() // 닫기 버튼
    {
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.CloseBnt);
        mySelf.gameObject.SetActive(false);
    }
     
}
