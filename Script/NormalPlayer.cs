using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlayer : MonoBehaviour
{
    public Material newMaterial; // 다아니믹 배칭을 위한 material

    private List<Transform> enemiesInRange = new List<Transform>(); // 적 감지시 위치 정보 저장
    Transform closestEnemy; // 가장 가까운 적

    GameManager gameManager;
    Animator anim;

    public int PlyaerID; // 플레이어 ID

    public GameObject arrowPrefab; // 화살 프리팹
    public float fireRate = 1f; // 공격 속도
    private float nextFireTime;
    public float ArrowSpeed; // 화살 속도

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
            closestEnemy = GetClosestEnemy(); // 가장 가까운 적 탐색

            if (closestEnemy != null) // 가장 가까운 적을 공격
            {
                anim.SetBool("Attack", true);
                nextFireTime = Time.time + fireRate; // 화살 딜레이
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
                enemiesInRange.Add(other.transform); // 적 콜라이더 감지시 위치를 추가
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(other.transform))
            {
                enemiesInRange.Remove(other.transform); // 사망, 다른 콜라이더는 제거
            }
        }
    }

    Transform GetClosestEnemy()
    {
        Transform closestEnemy = null; // 초기 위치를 null로 초기화
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.position); // 내 위치와 적의 위치를 계산
                if (distance < closestDistance) // 가장 가까운 적 탐색
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

        if (closestEnemy == null) return; // 타겟이 null이면 화살을 발사하지 않음

        Vector2 direction = (closestEnemy.position - transform.position).normalized;
        Vector3 position = new Vector3(0.5f, 0.5f, 0);
        GameObject arrow = Instantiate(arrowPrefab, transform.position + position, Quaternion.Euler(0, 0, -90f));

        arrow.GetComponent<Rigidbody2D>().velocity = direction * ArrowSpeed;
        arrow.GetComponent<Arrow>().dmg = gameManager.goDMG(PlyaerID, arrow);

        SoundManager.instance.SFXPlay("Sound", SoundManager.instance.PlayerBow);
    }

    IEnumerator EndArrow() // 발사 완료 후 애니메이션 종료
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("Attack", false);
    }
}
