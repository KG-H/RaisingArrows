using UnityEngine;
using UnityEngine.UI;

public class OnOffBtn : MonoBehaviour
{

    Quest quest;
    public Toggle toggle; // Toggle UI
    public Text buttonText; // Toggle 안에 있는 텍스트
    public Image buttonImage; // 배경 이미지

    public Color onColor = Color.green;
    public Color offColor = Color.red;

    public bool goAutoMake = false; // 자동 제작 버튼 On시 활성화

    void Start()
    {
        quest = FindAnyObjectByType<Quest>();
        UpdateToggleUI(toggle.isOn);
        toggle.onValueChanged.AddListener(delegate { UpdateToggleUI(toggle.isOn); });
    }

    void UpdateToggleUI(bool isOn)
    {
        if (isOn) // 버튼 OFF
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
            goAutoMake = false;
            buttonImage.color = onColor;
            buttonText.text = "On";
        }
        else // 버튼 ON
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            // 자동 제작 돌입
            goAutoMake = true;
            buttonImage.color = offColor;
            buttonText.text = "Off";

            if (quest.curQuest == 7)
            {
                quest.questsData[quest.curQuest].curQuestValue++;
            }
        }
    }
}
