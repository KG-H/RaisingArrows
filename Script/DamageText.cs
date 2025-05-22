using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    TextMeshPro text;
    Color alpha;

    public float moveSpeed;
    public float alpaSpeed;
    public float destroyTime;
    public float damage;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        int dmg = (int)damage;
        text.text = dmg.ToString();
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alpaSpeed);
        text.color = alpha;
    }

	private void DestroyObject()
	{
        Destroy(gameObject);
	}
}
