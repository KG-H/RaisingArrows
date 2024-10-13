using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public float dmg;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            if ((gameManager.curHP - dmg) < 0) gameManager.curHP = 0;
            else
            {
                SoundManager.instance.SFXPlay("Sound", SoundManager.instance.AttackEnemy);
                gameManager.curHP -= dmg;
            }

            Destroy(gameObject);
        }
    }
}
