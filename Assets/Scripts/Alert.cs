using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image img;

    public void Init(string text, Sprite s)
    {
        this.text.text = text;
        img.sprite = s;

    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
