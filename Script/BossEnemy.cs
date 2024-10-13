using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : BaseEnemy
{
    void Start()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material = newMaterial;
        }

        gameManager = FindAnyObjectByType<GameManager>();
        quest = FindAnyObjectByType<Quest>();
    }

    void Update()
    {
        HandleHP();
    }

    public override void Die()
    {
        quest.questsData[quest.curQuest].curQuestValue++;
        gameManager.isClear = true;
        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.DieEnemy);

        for (int i = 0; i < 5; i++)
        {
            CrystalBox[i] = Instantiate(Crystal, transform.position, transform.rotation);
            CrystalItem CrystalItem = CrystalBox[i].GetComponent<CrystalItem>();
            CrystalItem.goMove = true;
        }

        for (int i = 0; i < 5; i++)
        {
            GoldBox[i] = Instantiate(Gold, transform.position, transform.rotation);
            GoldItem GoldItem = GoldBox[i].GetComponent<GoldItem>();
            GoldItem.goMove = true;
        }
    }
}