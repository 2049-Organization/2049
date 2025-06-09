using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBubble : MonoBehaviour
{
    public string message = "";
    public TextMeshProUGUI messageText;

    public void Init()
    {
        messageText.text = message;
    }
}
