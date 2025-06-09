using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public void Init(DialogueChoice dialogueNode)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = dialogueNode.choiceText;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            MessageSystem.Instance.StartSendingMessages(dialogueNode.nextNode);
            Destroy(gameObject);
        });
    }
}
