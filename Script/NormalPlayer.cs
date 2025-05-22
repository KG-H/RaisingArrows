using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class GameObjectExtensions
{
    public static bool IsValid(this GameObject obj)
    {
        return obj != null && obj.activeInHierarchy;
    }
}

public class NormalPlayer : MonoBehaviour
{
    public Material newMaterial; // �پƴϹ� ��Ī�� ���� material

    private List<Transform> enemiesInRange = new List<Transform>(); // �� ������ ��ġ ���� ����

    GameObject closestEnemy = null;
    GameManager gameManager;
    Animator anim;

    public int PlyaerID; // �÷��̾� ID

    public GameObject arrowPrefab; // ȭ�� ������
    public float fireRate = 1f; // ���� �ӵ�
    private float nextFireTime;
    public float ArrowSpeed; // ȭ�� �ӵ�
    bool attack;

    public bool Attack
    {
        get => attack;
        set
        {
            if (attack == value) return; // 값이 같으면 애니메이션 호출 생략
            attack = value;
            anim.SetBool("Attack", value); // 값이 변경될 때만 호출
        }
    }

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
        //int? result = null;
        if (!gameManager.isDie)
        {
            // closestEnemy 가 null이 아니라면 IsValid를 실행
            if (closestEnemy?.IsValid() ?? false)
            {
                Attack = true;
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                FindEnemy();
            }
        }
    }

    private void FindEnemy()
    {
        closestEnemy = ObjectPool.Instance.GetClosestActiveMonster(transform.position);
    }

    /*
    private void FindEnemy()
    {
        if (enemiesInRange.Count > 0 && Time.time >= nextFireTime)
        {
            closestEnemy = GetClosestEnemy(); 

            if (closestEnemy != null) 
            {
                anim.SetBool("Attack", true);
                nextFireTime = Time.time + fireRate; 
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
                enemiesInRange.Add(other.transform); 
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(other.transform))
            {
                enemiesInRange.Remove(other.transform);
            }
        }
    }

    Transform GetClosestEnemy()
    {
        Transform closestEnemy = null; 
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.position); /
                if (distance < closestDistance) 
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }
        return closestEnemy;
    }
    */
    

    void FireArrow()
    {
        closestEnemy = ObjectPool.Instance.GetClosestActiveMonster(transform.position);

        StartCoroutine("EndArrow");

        if (closestEnemy == null) return; // Ÿ���� null�̸� ȭ���� �߻����� ����

        Vector2 direction = (closestEnemy.transform.position - transform.position).normalized;
        Vector3 position = new Vector3(0.5f, 0.5f, 0);

        GameObject arrow = ObjectPool.Instance.GetPooledObject(4);
        arrow.transform.position = transform.position + position;
        arrow.SetActive(true);

        arrow.GetComponent<Rigidbody2D>().velocity = direction * ArrowSpeed;
        arrow.GetComponent<Arrow>().dmg = gameManager.goDMG(PlyaerID, arrow);

        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.PlayerBow);
    }

    IEnumerator EndArrow() // �߻� �Ϸ� �� �ִϸ��̼� ����
    {
        yield return new WaitForSeconds(0.3f);
        Attack = false;
    }
}