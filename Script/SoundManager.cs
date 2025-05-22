using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource bgSound;
    public AudioClip[] bgList; // 배경음 3개

    public static SoundManager instance; // 효과음 싱글톤 패턴
    public AudioClip Click; // 화면 클릭
    public AudioClip FailClick; // 화면 클릭 실패
    public AudioClip MenuClick; // 메뉴 버튼
    public AudioClip SuccesBow; // 보스 토벌
    public AudioClip PlayerBow; // 플레이어 화살 발사시
    public AudioClip CloseBnt; // 닫기 버튼
    public AudioClip HitEnemy; // 몬스터 피격
    public AudioClip AttackEnemy; // 몬스터 공격
    public AudioClip DieEnemy; // 몬스터 사망
    public AudioClip GetGold; // 아이템 획득
    public AudioClip ClearBg; // 보스 토벌

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);
    }

    // 효과음 발생시
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);

    }

    // 배경음 설정
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
