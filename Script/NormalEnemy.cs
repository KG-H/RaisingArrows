using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NormalEnemy : BaseEnemy
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

        int random = Random.Range(0, 10);

        if (random == 9)
        {
            int random2 = Random.Range(1, 3);
            for (int i = 0; i < random2; i++)
            {
                CrystalBox[i] = Instantiate(Crystal, transform.position, transform.rotation);
                CrystalItem CrystalItem = CrystalBox[i].GetComponent<CrystalItem>();
                CrystalItem.goMove = true;

            }
        }
        else
        {
            int random2 = Random.Range(1, 4);
            for (int i = 0; i < random2; i++)
            {
                GoldBox[i] = Instantiate(Gold, transform.position, transform.rotation);
                GoldItem GoldItem = GoldBox[i].GetComponent<GoldItem>();
                GoldItem.goMove = true;
            }
        }

        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.DieEnemy);
    }
}
