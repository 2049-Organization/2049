using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageImage : MonoBehaviour
{
    [SerializeField] private Image img;

    public void Init(Sprite s)
    {
        img.sprite = s;
    }
}
