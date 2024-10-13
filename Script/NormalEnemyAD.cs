using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalEnemyAD : BaseEnemy
{
    public GameObject arrowPrefab; // ȭ�� ������
    public LayerMask layerMask; // �� ���� ���̾�
    public float beamDistance = 10f; // ���� �Ÿ�
    public float fireRate = 1f; // ���� �ӵ�
    private float nextFireTime;
    public float ArrowSpeed; // ȭ�� �ӵ�

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
        Attack();
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
            int random2 = Random.Range(1, 3);
            for (int i = 0; i < random2; i++)
            {
                GoldBox[i] = Instantiate(Gold, transform.position, transform.rotation);
                GoldItem GoldItem = GoldBox[i].GetComponent<GoldItem>();
                GoldItem.goMove = true;
            }
        }

        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.DieEnemy);

    }

    public void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.right, beamDistance, layerMask);

        if (Time.time >= nextFireTime && !isDie) // ���� ���ɽ� Ȱ��ȭ
        {
            if (hit.collider != null)
            {
                FrontWall = true;
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    public void FireArrow() // �ִϸ��̼� �̺�Ʈ�� ���� ����
    {
        Vector2 direction = (-transform.right).normalized;
        Vector3 position = new Vector3(-0.5f, 0.3f, 0);
        GameObject arrow = Instantiate(arrowPrefab, transform.position + position, Quaternion.Euler(0, 0, 90f));

        arrow.GetComponent<Rigidbody2D>().velocity = direction * ArrowSpeed;
    }

}
