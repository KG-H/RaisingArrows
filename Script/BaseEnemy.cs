using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 상태 패턴을 위한 인터페이스
public interface IMonsterState
{
    void Handle(BaseEnemy enemy);
}

public class WalkState : IMonsterState
{   
    public void Handle(BaseEnemy enemy)
    {
        enemy.anim.SetTrigger("Walk");

        if (enemy.curHP <= 0 && !enemy.isDie)
        {
            enemy.SetState(new DieState());
        }

        if (enemy.FrontWall)
        {
            enemy.SetState(new AttackState());
        }
    }
}

public class AttackState : IMonsterState
{
    public void Handle(BaseEnemy enemy)
    {
        enemy.anim.SetTrigger("Attack");

        if (enemy.curHP <= 0 && !enemy.isDie)
        {
            enemy.SetState(new DieState());
        }
    }
}

public class DieState : IMonsterState
{
    public void Handle(BaseEnemy enemy)
    {
        enemy.anim.SetTrigger("Die");
        enemy.FrontWall = true;
        enemy.isDie = true;
        enemy.Die();
    }
}


public abstract class BaseEnemy : MonoBehaviour
{
    private bool hasAnimationFinished = false;
    private IMonsterState _currentState;
    protected Quest quest;
    protected GameManager gameManager;

    public Material newMaterial; // 다이나믹 배칭을 위한 material 
    public bool FrontWall = false;
    public Animator anim;
    public Slider hpbar;
    public GameObject hudDamageText;
    public GameObject hudDamageTextRed;
    public GameObject Gold;
    public GameObject Crystal;
    public Transform hudPos;

    public bool isDie = false;
    public float maxHP; // 최대 체력
    public float curHP; // 현재 체력
    public float dmg; // 데미지

    void Start()
    {
        hpbar.value = curHP / maxHP;
    }

    // 초기 상태를 Walk상태로 설정
    public BaseEnemy()
    {
        _currentState = new WalkState();
    }

    private void FixedUpdate()
    {
        if (!FrontWall) transform.Translate(Vector2.left * Time.deltaTime * 0.3f);

        if (!isDie) _currentState.Handle(this);

        
        if (isDie)
        {
            DestroyEnemy();
        }
        
    }

    // 상태 변경시 호출
    public void SetState(IMonsterState newState)
    {
        _currentState = newState;
    }

    public void HandleHP()
    {
        hpbar.value = Mathf.Lerp(hpbar.value, curHP / maxHP, Time.deltaTime * 10f);
    }

    public void DestroyEnemy()
    {
        if (quest.curQuest == 0)
        {
            quest.questsData[0].curQuestValue++;
        }

        // 초기 상태로 초기화
        hasAnimationFinished = false;
        FrontWall = false;
        curHP = maxHP;
        isDie = false;
        
        ObjectPool.Instance.ReturnToPool(gameObject); // 풀 재사용
    }

    public void GetCrystal()
    {
        GameObject crystal = ObjectPool.Instance.GetPooledObject(7);
        crystal.transform.position = transform.position;
        crystal.SetActive(true);
        CrystalItem CrystalItem = crystal.GetComponent<CrystalItem>();
        CrystalItem.goMove = true;
    }

    public void GetGold()
    {
        GameObject gold = ObjectPool.Instance.GetPooledObject(6);
        gold.transform.position = transform.position;
        gold.SetActive(true);
        GoldItem goldItem = gold.GetComponent<GoldItem>();
        goldItem.goMove = true;
    }

    // 애니메이션 이벤트에서 발생
    void AttackWall()
    {
        if ((gameManager.curHP - dmg) < 0) gameManager.curHP = 0;
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.AttackEnemy);
            gameManager.curHP -= dmg;
        }
    }

    // 벽감지, 화살 피격시 플로팅 텍스트 출력
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && !isDie)
        {
            FrontWall = true;
        }
        else if (collision.CompareTag("Arrow"))
        {
            float Arrowdmg = collision.GetComponent<Arrow>().dmg;
            curHP -= Arrowdmg;
            GameObject hudText;

            if (collision.GetComponent<Arrow>().critical) // 크리 적중시 붉은 텍스트
            {
                hudText = Instantiate(hudDamageTextRed);
            }
            else hudText = Instantiate(hudDamageText);

            hudText.transform.position = hudPos.position;
            hudText.GetComponent<DamageText>().damage = Arrowdmg;

            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.HitEnemy);
        }
    }

    // Die클래스 오버라이딩
    public abstract void Die();
}