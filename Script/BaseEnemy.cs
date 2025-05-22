using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// ���� ������ ���� �������̽�
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

    public Material newMaterial; // ���̳��� ��Ī�� ���� material 
    public bool FrontWall = false;
    public Animator anim;
    public Slider hpbar;
    public GameObject hudDamageText;
    public GameObject hudDamageTextRed;
    public GameObject Gold;
    public GameObject Crystal;
    public Transform hudPos;

    public bool isDie = false;
    public float maxHP; // �ִ� ü��
    public float curHP; // ���� ü��
    public float dmg; // ������

    void Start()
    {
        hpbar.value = curHP / maxHP;
    }

    // �ʱ� ���¸� Walk���·� ����
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

    // ���� ����� ȣ��
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

        // �ʱ� ���·� �ʱ�ȭ
        hasAnimationFinished = false;
        FrontWall = false;
        curHP = maxHP;
        isDie = false;
        
        ObjectPool.Instance.ReturnToPool(gameObject); // Ǯ ����
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

    // �ִϸ��̼� �̺�Ʈ���� �߻�
    void AttackWall()
    {
        if ((gameManager.curHP - dmg) < 0) gameManager.curHP = 0;
        else
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.AttackEnemy);
            gameManager.curHP -= dmg;
        }
    }

    // ������, ȭ�� �ǰݽ� �÷��� �ؽ�Ʈ ���
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

            if (collision.GetComponent<Arrow>().critical) // ũ�� ���߽� ���� �ؽ�Ʈ
            {
                hudText = Instantiate(hudDamageTextRed);
            }
            else hudText = Instantiate(hudDamageText);

            hudText.transform.position = hudPos.position;
            hudText.GetComponent<DamageText>().damage = Arrowdmg;

            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.HitEnemy);
        }
    }

    // DieŬ���� �������̵�
    public abstract void Die();
}