using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public enum MessageSender
{
    Player,
    Roxane
}

public enum BubbleSize
{
    XS,
    S,
    L,
    XL
}

public class MessageSystem : MonoBehaviour
{
    [Header("Bubbles prefabs")]

    [SerializeField] private GameObject playerMessagePrefabXS;
    [SerializeField] private GameObject playerMessagePrefabS;
    [SerializeField] private GameObject playerMessagePrefabL;
    [SerializeField] private GameObject playerMessagePrefabXL;
    [SerializeField] private GameObject roxaneMessagePrefabXS;
    [SerializeField] private GameObject roxaneMessagePrefabS;
    [SerializeField] private GameObject roxaneMessagePrefabL;
    [SerializeField] private GameObject roxaneMessagePrefabXL;


    [Header("Choice system")]
    [SerializeField] private Transform choicesTr;
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private List<GameObject> choices = new List<GameObject>();

    [Header("Miscellaneous")]

    [SerializeField] private Transform content;
    private ScrollRect scrollRect;
    private DialogueNode currentNode;
    [SerializeField] private DialogueNode startNode;
    [SerializeField] private int day = 0;
    [SerializeField] private GameObject endOfDayPrefab;

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
        foreach(GameObject choice in choices)
        {
            Destroy(choice);
        }
        currentNode = start;
        StartCoroutine(SendMessage());
    }

   private IEnumerator SendMessage()
   {
        if (currentNode.isEndOfDay) 
        {
            day += 1;
            ShowEndOfDayMessage(day);
        }
        else
        {
            ShowMessage(currentNode.message, currentNode.speaker, currentNode.bubbleSize);
            if (currentNode.timeToWait > 0)
                yield return new WaitForSeconds(currentNode.timeToWait);
            if (currentNode.choices != null && currentNode.choices.Count > 0)
            {
                foreach (var choice in currentNode.choices)
                {
                    if (PlayerData.Instance.object_map.ContainsKey(choice.choiceText))
                    {
                        if (PlayerData.Instance.object_map[choice.choiceText] == PlayerData.Instance.obj1 ||
                            PlayerData.Instance.object_map[choice.choiceText] == PlayerData.Instance.obj2 ||
                            PlayerData.Instance.object_map[choice.choiceText] == PlayerData.Instance.obj3 )
                        {
                            continue;
                        }
                        else
                        {
                            choices.Add(Instantiate(choicePrefab, choicesTr));
                            choices[^1].GetComponent<ChoiceButton>().Init(choice);
                        }
                    }
                    else
                    {
                        choices.Add(Instantiate(choicePrefab, choicesTr));
                        choices[^1].GetComponent<ChoiceButton>().Init(choice);
                    }
                }

                Canvas.ForceUpdateCanvases();
                scrollRect.normalizedPosition = new Vector2(0.0f, 0);
            }
            else if(currentNode.directNextNode != null)
            {
                NextMessage();
            }
        }
    }

    private void NextMessage()
    {
        currentNode = currentNode.directNextNode;
        StartCoroutine(SendMessage());
    }

    public void ShowEndOfDayMessage(int day)
    {
        Instantiate(endOfDayPrefab, content).GetComponent<EndOfDayMessage>().SetDay(day);
        Canvas.ForceUpdateCanvases();
        scrollRect.normalizedPosition = new Vector2(0.0f, 0);
    }

    public void ShowMessage(string message, MessageSender sender, BubbleSize size)
    {
        switch (sender)
        {
            case MessageSender.Player:
                GameObject go_pl;
                switch (size)
                {
                    case BubbleSize.XS:
                        go_pl = Instantiate(playerMessagePrefabXS, content);
                        go_pl.GetComponent<MessageBubble>().message = message;
                        go_pl.GetComponent<MessageBubble>().Init();
                        break;
                    case BubbleSize.S:
                        go_pl = Instantiate(playerMessagePrefabS, content);
                        go_pl.GetComponent<MessageBubble>().message = message;
                        go_pl.GetComponent<MessageBubble>().Init();
                        break;
                    case BubbleSize.L:
                        go_pl = Instantiate(playerMessagePrefabL, content);
                        go_pl.GetComponent<MessageBubble>().message = message;
                        go_pl.GetComponent<MessageBubble>().Init();
                        break;
                    case BubbleSize.XL:
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
                switch (size)
                {
                    case BubbleSize.XS:
                        go_rx = Instantiate(roxaneMessagePrefabXS, content);
                        go_rx.GetComponent<MessageBubble>().message = message;
                        go_rx.GetComponent<MessageBubble>().Init();
                        break;
                    case BubbleSize.S:
                        go_rx = Instantiate(roxaneMessagePrefabS, content);
                        go_rx.GetComponent<MessageBubble>().message = message;
                        go_rx.GetComponent<MessageBubble>().Init();
                        break;
                    case BubbleSize.L:
                        go_rx = Instantiate(roxaneMessagePrefabL, content);
                        go_rx.GetComponent<MessageBubble>().message = message;
                        go_rx.GetComponent<MessageBubble>().Init();
                        break;
                    case BubbleSize.XL:
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
