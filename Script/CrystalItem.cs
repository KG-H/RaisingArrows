using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalItem : MonoBehaviour
{
    GameManager gameManager;
    Vector3 StartPos;
    Vector3 EndPos;

    public float Speed;
    public float rewardSpeed;
    public bool goMove;
    public bool isReward;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        EndPos = new Vector3(0.6f, 0.68f, 0);
    }

    void Update()
    {
        if (goMove) Invoke("MoveGold", 1f);
        if (isReward) MoveReward();
    }

    public void MoveReward()
    {
        StartPos = transform.position;
        transform.position = Vector3.MoveTowards(StartPos, EndPos, rewardSpeed * Time.deltaTime);
    }

    public void MoveGold()
    {
        StartPos = transform.position;
        transform.position = Vector3.MoveTowards(StartPos, EndPos, Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GoldCryBox"))
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.GetGold);
            gameManager.Crystal += 50;
            gameObject.SetActive(false);
        }
    }
}
