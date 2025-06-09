using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    LayoutElement le;
    float width = -1f;
    float t = 0.0f;

    [SerializeField] private float transitionSpeed;

    [SerializeField] private DialogueNode fourthDialogueNode;
    [SerializeField] private DialogueNode fifthDialogueNode;
    [SerializeField] private DialogueNode sixthDialogueNode;

    private void Start()
    {
        le = GetComponent<LayoutElement>();
    }

    public void Init(DialogueChoice dialogueNode)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = dialogueNode.choiceText;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (PlayerData.Instance.object_map.ContainsKey(dialogueNode.choiceText))
            {
                if(PlayerData.Instance.obj1 == Object.Null)
                {
                    PlayerData.Instance.obj1 = PlayerData.Instance.object_map[dialogueNode.choiceText];
                    DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();
                    newNode.speaker = MessageSender.Player;
                    newNode.bubbleSize = BubbleSize.S;
                    newNode.nodeId = "obj1";
                    newNode.timeToWait = 2f;
                    newNode.directNextNode = fourthDialogueNode;
                    newNode.message = GetObjectMessage(PlayerData.Instance.obj1);

                    MessageSystem.Instance.StartSendingMessages(newNode);
                }
                else if (PlayerData.Instance.obj2 == Object.Null)
                {
                    PlayerData.Instance.obj2 = PlayerData.Instance.object_map[dialogueNode.choiceText];
                    DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();
                    newNode.speaker = MessageSender.Player;
                    newNode.bubbleSize = BubbleSize.S;
                    newNode.nodeId = "obj2";
                    newNode.timeToWait = 2f;
                    newNode.directNextNode = fifthDialogueNode;
                    newNode.message = GetObjectMessage(PlayerData.Instance.obj2);

                    MessageSystem.Instance.StartSendingMessages(newNode);
                }
                else if (PlayerData.Instance.obj3 == Object.Null)
                {
                    PlayerData.Instance.obj3 = PlayerData.Instance.object_map[dialogueNode.choiceText];
                    DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();
                    newNode.speaker = MessageSender.Player;
                    newNode.bubbleSize = BubbleSize.S;
                    newNode.nodeId = "obj3";
                    newNode.timeToWait = 2f;
                    newNode.directNextNode = sixthDialogueNode;
                    newNode.message = GetObjectMessage(PlayerData.Instance.obj3);
                            
                    MessageSystem.Instance.StartSendingMessages(newNode);
                }
            }
            else
            {
                MessageSystem.Instance.StartSendingMessages(dialogueNode.nextNode);
            }
        });
    }

    private string GetObjectMessage(Object obj)
    {
        switch (obj)
        {
            case Object.Medicine:
                return "Tu vas sûrement avoir besoin de médicaments !";
            case Object.Raincoat:
                return "Tu vas sûrement avoir besoin de vêtements imperméables !";
            case Object.Phone_charger:
                return "Tu vas sûrement avoir besoin de charger ton téléphone !";
            case Object.Tin_can:
                return "Tu vas sûrement avoir besoin de manger, prends des conserves !";
            case Object.Bus_card:
                return "Tu vas sûrement avoir besoin de prendre le bus !";
            case Object.Bank_card:
                return "Tu vas sûrement avoir besoin de payer, prends ta carte bancaire !";
        }

        return "";
    }

    private void Update()
    {
        t += transitionSpeed * Time.deltaTime;
        le.preferredWidth = Mathf.Lerp(le.preferredWidth, width, t);
    }

    public void MouseEnter(BaseEventData data)
    {
        if (le != null)
        {
            width = 200f;
        }
    }

    public void MouseExit(BaseEventData data)
    {
        if (le != null)
        {
            width = -1f;
        }
    }
}
