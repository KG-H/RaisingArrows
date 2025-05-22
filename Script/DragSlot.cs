using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DragSlot : MonoBehaviour
{
    // 복사 슬롯 싱글톤 패턴
    static public DragSlot instance;

    public Slot dragSlot;

    // 아이템 이미지
    [SerializeField]
    private Image imageItem;

    [SerializeField]
    private Image imageBackItem;

    public TextMeshProUGUI text_Count;
    public int ItemCount;

    void Start()
    {
        instance = this;
    }

    // 알파값 조정
    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;

        Color color2 = imageBackItem.color;
        color2.a = _alpha;
        imageBackItem.color = color2;
    }
}