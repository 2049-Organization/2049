using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum MessageSender
{
    Player,
    Roxane
}
public class MessageSystem : MonoBehaviour
{
    public float characterPerLine = 10f;

    public GameObject playerMessagePrefabXS;
    public GameObject playerMessagePrefabS;
    public GameObject playerMessagePrefabL;
    public GameObject playerMessagePrefabXL;
    public GameObject roxaneMessagePrefabXS;
    public GameObject roxaneMessagePrefabS;
    public GameObject roxaneMessagePrefabL;
    public GameObject roxaneMessagePrefabXL;

    public Transform content;
    public Transform choices;

    public GameObject choicePrefab;

    private ScrollRect scrollRect;

    private DialogueNode currentNode;
    [SerializeField] private DialogueNode startNode;

    public static MessageSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        StartSendingMessages(startNode);
    }

    public void StartSendingMessages(DialogueNode start)
    {
        currentNode = start;
        StartCoroutine(SendMessage());
    }

   private IEnumerator SendMessage()
   {
        ShowMessage(currentNode.message, currentNode.speaker);
        if (currentNode.timeToWait > 0)
            yield return new WaitForSeconds(currentNode.timeToWait);
        if (currentNode.choices != null && currentNode.choices.Count > 0)
        {
            foreach (var choice in currentNode.choices)
            {
                GameObject go = Instantiate(choicePrefab, choices);
                go.GetComponent<ChoiceButton>().Init(choice);
            }
        }
        else if(currentNode.directNextNode != null)
        {
            NextMessage();
        }
        else
        {
            Debug.Log("Dialogue has ended");
        }
    }

    private void NextMessage()
    {
        currentNode = currentNode.directNextNode;
        StartCoroutine(SendMessage());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowMessage("This is a test message to see how the message system works. It should be able to handle multiple lines and different senders.", MessageSender.Player);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowMessage("This is a test message to see how the message system works. It should be able to handle multiple lines and different senders.", MessageSender.Roxane);
        }
    }

    public void ShowMessage(string message, MessageSender sender)
    {
        int nbLines = Mathf.CeilToInt(message.Length / characterPerLine);
        switch (sender)
        {
            case MessageSender.Player:
                GameObject go_pl;
                switch (nbLines)
                {
                    case 1:
                        go_pl = Instantiate(playerMessagePrefabXS, content);
                        go_pl.GetComponent<MessageBubble>().message = message;
                        go_pl.GetComponent<MessageBubble>().Init();
                        break;
                    case 2:
                        go_pl = Instantiate(playerMessagePrefabS, content);
                        go_pl.GetComponent<MessageBubble>().message = message;
                        go_pl.GetComponent<MessageBubble>().Init();
                        break;
                    case 3:
                        go_pl = Instantiate(playerMessagePrefabL, content);
                        go_pl.GetComponent<MessageBubble>().message = message;
                        go_pl.GetComponent<MessageBubble>().Init();
                        break;
                    case 4:
                        go_pl = Instantiate(playerMessagePrefabXL, content);
                        go_pl.GetComponent<MessageBubble>().message = message;
                        go_pl.GetComponent<MessageBubble>().Init();
                        break;
                    default:
                        break;
                }
                
                break;
            case MessageSender.Roxane:
                GameObject go_rx;
                switch (nbLines)
                {
                    case 1:
                        go_rx = Instantiate(roxaneMessagePrefabXS, content);
                        go_rx.GetComponent<MessageBubble>().message = message;
                        go_rx.GetComponent<MessageBubble>().Init();
                        break;
                    case 2:
                        go_rx = Instantiate(roxaneMessagePrefabS, content);
                        go_rx.GetComponent<MessageBubble>().message = message;
                        go_rx.GetComponent<MessageBubble>().Init();
                        break;
                    case 3:
                        go_rx = Instantiate(roxaneMessagePrefabL, content);
                        go_rx.GetComponent<MessageBubble>().message = message;
                        go_rx.GetComponent<MessageBubble>().Init();
                        break;
                    case 4:
                        go_rx = Instantiate(roxaneMessagePrefabXL, content);
                        go_rx.GetComponent<MessageBubble>().message = message;
                        go_rx.GetComponent<MessageBubble>().Init();
                        break;
                    default:
                        break;
                }

                break;
        }
        Canvas.ForceUpdateCanvases();
        scrollRect.normalizedPosition = new Vector2(0.0f, 0);
    }
}
