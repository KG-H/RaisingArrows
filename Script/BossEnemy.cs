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
            GetCrystal();
        }

        for (int i = 0; i < 5; i++)
        {
            GetGold();
        }
    }
}