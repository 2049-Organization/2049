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
    XXS,
    XS,
    S,
    L,
    XL
}

public class MessageSystem : MonoBehaviour
{
    [Header("Bubbles prefabs")]

    [SerializeField] private GameObject playerMessagePrefabXXS;
    [SerializeField] private GameObject playerMessagePrefabXS;
    [SerializeField] private GameObject playerMessagePrefabS;
    [SerializeField] private GameObject playerMessagePrefabL;
    [SerializeField] private GameObject playerMessagePrefabXL;
    [SerializeField] private GameObject roxaneMessagePrefabXXS;
    [SerializeField] private GameObject roxaneMessagePrefabXS;
    [SerializeField] private GameObject roxaneMessagePrefabS;
    [SerializeField] private GameObject roxaneMessagePrefabL;
    [SerializeField] private GameObject roxaneMessagePrefabXL;

    [Header("Choice system")]
    [SerializeField] private Transform choicesTr;
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private List<GameObject> choices = new List<GameObject>();

    [Header("Audio System")]
    [SerializeField] private AudioClip recieveSound;
    [SerializeField] private AudioClip sendSound;
    [SerializeField] private AudioClip notificationSound;
    [SerializeField] private AudioManager audioManager;

    [Header("Miscellaneous")]
    [SerializeField] private Transform content;
    private ScrollRect scrollRect;
    private DialogueNode currentNode;
    [SerializeField] private DialogueNode startNode;
    [SerializeField] private int day = 0;
    [SerializeField] private GameObject endOfDayPrefab;
    [SerializeField] private Animator animEyes;
    [SerializeField] private DialogueNode waitingForYouNode;
    [SerializeField] private DialogueNode corruptedControllerNode;
    [SerializeField] private DialogueNode findOldFriendNode;
    [SerializeField] private DialogueNode goIntoTheTrainNode;
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
        choices.Clear();
        currentNode = start;
        StartCoroutine(SendMessage());
    }

   private IEnumerator SendMessage()
   {
        string messageMod = "";

        if (currentNode.isEndOfDay) 
        {
            day += 1;
            ShowEndOfDayMessage(day);
        }
        else
        {
            if (currentNode.message == "Internal/Choix du v�hicule")
            {
                if (PlayerData.Instance.tookTheBus)
                    messageMod = "Hey ! Je suis ENFIN arriv� � Bordeaux. Le trajet �tait tellement long en bus� mais au moins on �touffait pas dans les fum�es de l�embouteillage.";
                else if (PlayerData.Instance.tookTheCar)
                    messageMod = " Hey ! Je suis enfin arriv� � Bordeaux. Le trajet �tait long, surtout � cause des fum�es de l�embouteillage qui m�ont clairement donn� mal � la t�te.\r\n";
                else if (PlayerData.Instance.tookTheTaxi)
                    messageMod = "Hey ! Je suis enfin arriv� � Bordeaux ! Tu avais raison, le taxi est pass� par des petites routes et m�a fait �viter tous les embouteillages. Un super gain de temps !\r\n";
            }
            else if (currentNode.message == "Internal/Maladie?")
            {
                if (PlayerData.Instance.illness)
                {
                    messageMod = "Je suis arriv�e au train, ils m�ont dit que je ne pouvais pas monter car je suis malade. J�ai d� attraper le nouveau virus dont tout le monde parle�";

                    DialogueChoice choice1 = new DialogueChoice();
                    choice1.choiceText = "Monter dans le train: -m�dicaments";
                    choice1.neededObject = Object.Medicine;
                    choice1.nextNode = goIntoTheTrainNode;
                    choices.Add(Instantiate(choicePrefab, choicesTr));
                    choices[^1].GetComponent<ChoiceButton>().Init(choice1);

                    DialogueChoice choice2 = new DialogueChoice();
                    choice2.choiceText = "Chercher une connaissance";
                    choice2.neededObject = Object.Null;
                    choice2.nextNode = findOldFriendNode;
                    choices.Add(Instantiate(choicePrefab, choicesTr));
                    choices[^1].GetComponent<ChoiceButton>().Init(choice2);

                    DialogueChoice choice3 = new DialogueChoice();
                    choice3.choiceText = "Soudoyer le contr�leur: -2 argent";
                    choice3.neededObject = Object.Null;
                    choice3.nextNode = corruptedControllerNode;
                    choices.Add(Instantiate(choicePrefab, choicesTr));
                    choices[^1].GetComponent<ChoiceButton>().Init(choice3);
                }
                else
                {
                    messageMod = "Je suis arriv�e � La Rochelle et je suis mont�e dans le train, je l'ai pris en direction de Paris. Je serai bient�t arriv�e !";
                    DialogueChoice choice = new DialogueChoice();
                    choice.choiceText = "Je t�attend !";
                    choice.neededObject = Object.Null;
                    choice.nextNode = waitingForYouNode;
                    choices.Add(Instantiate(choicePrefab, choicesTr));
                    choices[^1].GetComponent<ChoiceButton>().Init(choice);
                }
            }

            ShowMessage(messageMod == "" ? currentNode.message : messageMod, currentNode.speaker, currentNode.bubbleSize);
            if (currentNode.timeToWait > 0 && !Input.GetKey(KeyCode.Mouse1))
                yield return new WaitForSeconds(currentNode.timeToWait);
            
            if (currentNode.choices != null && currentNode.choices.Count > 0 && choices.Count == 0)
            {
                foreach (var choice in currentNode.choices)
                {

                    if (PlayerData.Instance.object_map.ContainsKey(choice.choiceText))
                    {
                        if (PlayerData.Instance.object_map[choice.choiceText] == PlayerData.Instance.GetObj1() ||
                            PlayerData.Instance.object_map[choice.choiceText] == PlayerData.Instance.GetObj2() ||
                            PlayerData.Instance.object_map[choice.choiceText] == PlayerData.Instance.GetObj3())
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

                    if (choice.neededObject != Object.Null &&
                    PlayerData.Instance.GetObj1() != choice.neededObject &&
                    PlayerData.Instance.GetObj2() != choice.neededObject &&
                    PlayerData.Instance.GetObj3() != choice.neededObject)
                    {
                        choices[^1].GetComponent<ChoiceButton>().SetUnavailable();
                    }
                }

                Canvas.ForceUpdateCanvases();
                scrollRect.normalizedPosition = new Vector2(0, 0);

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
        scrollRect.normalizedPosition = new Vector2(0, 0);
        animEyes.Play("Sleep");
        StartCoroutine(WaitForNextDay());
    }

    private IEnumerator WaitForNextDay()
    {
        yield return new WaitForSeconds(3f);
        NextMessage();
    }

    public void ShowMessage(string message, MessageSender sender, BubbleSize size)
    {
        switch (sender)
        {
            case MessageSender.Player:
                audioManager.PlaySFX(sendSound);
                GameObject go_pl;
                switch (size)
                {
                    case BubbleSize.XXS:
                        go_pl = Instantiate(playerMessagePrefabXXS, content);
                        go_pl.GetComponent<MessageBubble>().message = message;
                        go_pl.GetComponent<MessageBubble>().Init();
                        break;
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
                audioManager.PlaySFX(recieveSound);
                GameObject go_rx;
                switch (size)
                {
                    case BubbleSize.XXS:
                        go_rx = Instantiate(roxaneMessagePrefabXXS, content);
                        go_rx.GetComponent<MessageBubble>().message = message;
                        go_rx.GetComponent<MessageBubble>().Init();
                        break;
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
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
}
