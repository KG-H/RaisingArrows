using UnityEngine;
using UnityEngine.UI;

public class OnOffBtn : MonoBehaviour
{

    Quest quest;
    public Toggle toggle; // Toggle UI
    public Text buttonText; // Toggle �ȿ� �ִ� �ؽ�Ʈ
    public Image buttonImage; // ��� �̹���

    public Color onColor = Color.green;
    public Color offColor = Color.red;

    public bool goAutoMake = false; // �ڵ� ���� ��ư On�� Ȱ��ȭ

    void Start()
    {
        quest = FindAnyObjectByType<Quest>();
        UpdateToggleUI(toggle.isOn);
        toggle.onValueChanged.AddListener(delegate { UpdateToggleUI(toggle.isOn); });
    }

    void UpdateToggleUI(bool isOn)
    {
        if (isOn) // ��ư OFF
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);
            goAutoMake = false;
            buttonImage.color = onColor;
            buttonText.text = "On";
        }
        else // ��ư ON
        {
            SoundManager.instance.SFXPlay("Sound", SoundManager.instance.Click);

            // �ڵ� ���� ����
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
