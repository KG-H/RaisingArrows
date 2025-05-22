using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiddleEnemy : BaseEnemy
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

        int random = Random.Range(0, 6);

        if(random == 5)
		{   
            int random2 = Random.Range(1, 4);
            for (int i = 0; i < random2; i++)
            {
                GetCrystal();
            }
        }
        else
		{
            int random2 = Random.Range(3, 6);
            for (int i = 0; i < random2; i++)
            {
                GetGold();
            }
        }

         SoundManager.instance.SFXPlay("Sound", SoundManager.instance.DieEnemy);
    }

}
