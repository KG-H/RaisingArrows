using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlayer : MonoBehaviour
{
    public Material newMaterial; // �پƴϹ� ��Ī�� ���� material

    private List<Transform> enemiesInRange = new List<Transform>(); // �� ������ ��ġ ���� ����
    Transform closestEnemy; // ���� ����� ��

    GameManager gameManager;
    Animator anim;

    public int PlyaerID; // �÷��̾� ID

    public GameObject arrowPrefab; // ȭ�� ������
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
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!gameManager.isDie) FindEnemy();
    }

    private void FindEnemy()
    {
        if (enemiesInRange.Count > 0 && Time.time >= nextFireTime)
        {
            closestEnemy = GetClosestEnemy(); // ���� ����� �� Ž��

            if (closestEnemy != null) // ���� ����� ���� ����
            {
                anim.SetBool("Attack", true);
                nextFireTime = Time.time + fireRate; // ȭ�� ������
            }
            else anim.SetBool("Attack", false);

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!enemiesInRange.Contains(other.transform))
            {
                enemiesInRange.Add(other.transform); // �� �ݶ��̴� ������ ��ġ�� �߰�
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(other.transform))
            {
                enemiesInRange.Remove(other.transform); // ���, �ٸ� �ݶ��̴��� ����
            }
        }
    }

    Transform GetClosestEnemy()
    {
        Transform closestEnemy = null; // �ʱ� ��ġ�� null�� �ʱ�ȭ
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.position); // �� ��ġ�� ���� ��ġ�� ���
                if (distance < closestDistance) // ���� ����� �� Ž��
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }
        return closestEnemy;
    }


    void FireArrow()
    {
        StartCoroutine("EndArrow");

        if (closestEnemy == null) return; // Ÿ���� null�̸� ȭ���� �߻����� ����

        Vector2 direction = (closestEnemy.position - transform.position).normalized;
        Vector3 position = new Vector3(0.5f, 0.5f, 0);
        GameObject arrow = Instantiate(arrowPrefab, transform.position + position, Quaternion.Euler(0, 0, -90f));

        arrow.GetComponent<Rigidbody2D>().velocity = direction * ArrowSpeed;
        arrow.GetComponent<Arrow>().dmg = gameManager.goDMG(PlyaerID, arrow);

        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.PlayerBow);
    }

    IEnumerator EndArrow() // �߻� �Ϸ� �� �ִϸ��̼� ����
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("Attack", false);
    }
}
