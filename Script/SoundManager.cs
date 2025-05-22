using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource bgSound;
    public AudioClip[] bgList; // ����� 3��

    public static SoundManager instance; // ȿ���� �̱��� ����
    public AudioClip Click; // ȭ�� Ŭ��
    public AudioClip FailClick; // ȭ�� Ŭ�� ����
    public AudioClip MenuClick; // �޴� ��ư
    public AudioClip SuccesBow; // ���� ���
    public AudioClip PlayerBow; // �÷��̾� ȭ�� �߻��
    public AudioClip CloseBnt; // �ݱ� ��ư
    public AudioClip HitEnemy; // ���� �ǰ�
    public AudioClip AttackEnemy; // ���� ����
    public AudioClip DieEnemy; // ���� ���
    public AudioClip GetGold; // ������ ȹ��
    public AudioClip ClearBg; // ���� ���

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);
    }

    // ȿ���� �߻���
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);

    }

    // ����� ����
    public void BgSoundPlay(int stage)
    {
        bgSound.clip = bgList[stage - 1];
        bgSound.loop = true;
        bgSound.volume = 1f;
        bgSound.Play();
    }

    public void EndPlay()
    {
        bgSound.volume = 0f;
    }
}
