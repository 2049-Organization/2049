using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions;
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

    [SerializeField] private GameObject roxaneMessageImagePrefab;

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
    [SerializeField] private Sprite virusImage;
    [SerializeField] private GameObject alertPrefab;
    [SerializeField] private RectTransform alertHolder;

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
        bool useVirusImage = false;
        if (currentNode.isEndOfDay) 
        {
            day += 1;
            ShowEndOfDayMessage(day);
        }
        else
        {
            if (currentNode.message == "Internal/Choix du véhicule")
            {
                if (PlayerData.Instance.tookTheBus)
                    messageMod = "Hey ! Je suis ENFIN arrivé à Bordeaux. Le trajet était tellement long en bus… mais au moins on étouffait pas dans les fumées de l’embouteillage <sprite=1863>.";
                else if (PlayerData.Instance.tookTheCar)
                    messageMod = " Hey ! Je suis enfin arrivé à Bordeaux. Le trajet était long, surtout à cause des fumées de l’embouteillage qui m’ont clairement donné mal à la tête <sprite=1863>.";
                else if (PlayerData.Instance.tookTheTaxi)
                    messageMod = "Hey ! Je suis enfin arrivé à Bordeaux ! Tu avais raison, le taxi est passé par des petites routes et m’a fait éviter tous les embouteillages. Un super gain de temps <sprite=1985> !";
            }
            else if (currentNode.message == "Internal/Maladie?")
            {
                if (PlayerData.Instance.illness)
                {
                    messageMod = "Je suis arrivée au train, ils m’ont dit que je ne pouvais pas monter car je suis malade. J’ai dû attraper le nouveau virus dont tout le monde parle…";
                    useVirusImage = true;

                    DialogueChoice choice1 = new()
                    {
                        choiceText = "Monter dans le train: -médicaments",
                        neededObject = Object.Medicine,
                        nextNode = goIntoTheTrainNode
                    };
                    choices.Add(Instantiate(choicePrefab, choicesTr));
                    choices[^1].GetComponent<ChoiceButton>().Init(choice1);

                    if (Object.Medicine != PlayerData.Instance.GetObj1() &&
                        Object.Medicine != PlayerData.Instance.GetObj2() &&
                        Object.Medicine != PlayerData.Instance.GetObj3())
                    {
                        choices[^1].GetComponent<ChoiceButton>().SetUnavailable();
                    }

                    DialogueChoice choice2 = new()
                    {
                        choiceText = "Chercher une connaissance",
                        neededObject = Object.Null,
                        nextNode = findOldFriendNode
                    };
                    choices.Add(Instantiate(choicePrefab, choicesTr));
                    choices[^1].GetComponent<ChoiceButton>().Init(choice2);

                    DialogueChoice choice3 = new()
                    {
                        choiceText = "Soudoyer le contrôleur: -2 argent",
                        neededObject = Object.Null,
                        nextNode = corruptedControllerNode
                    };
                    choices.Add(Instantiate(choicePrefab, choicesTr));
                    choices[^1].GetComponent<ChoiceButton>().Init(choice3);

                    if (PlayerData.Instance.GetMoney() <= 0)
                    {
                        choices[^1].GetComponent<ChoiceButton>().SetUnavailable();
                    }
                }
                else
                {
                    messageMod = "Je suis arrivée à La Rochelle et je suis montée dans le train, je l'ai pris en direction de Paris. Je serai bientôt arrivée <sprite=2179>!";
                    DialogueChoice choice = new()
                    {
                        choiceText = "Je t’attend !",
                        neededObject = Object.Null,
                        nextNode = waitingForYouNode
                    };
                    choices.Add(Instantiate(choicePrefab, choicesTr));
                    choices[^1].GetComponent<ChoiceButton>().Init(choice);
                }
            }

            ShowMessage(messageMod == "" ? currentNode.message : messageMod, currentNode.speaker, currentNode.bubbleSize, useVirusImage ? virusImage : currentNode.image, currentNode.alert, currentNode.alertImage);
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
        if (currentNode.directNextNode != null) 
            NextMessage();
    }

    public void ShowMessage(string message, MessageSender sender, BubbleSize size, Sprite s, string alert, Sprite alertSprite)
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
        
        if(s != null)
        {
            Instantiate(roxaneMessageImagePrefab, content).GetComponent<MessageImage>().Init(s);
        }

        if(alert != null && alertSprite != null)
        {
            Instantiate(alertPrefab, alertHolder).GetComponent<Alert>().Init(alert, alertSprite);
            audioManager.PlaySFX(notificationSound);
        }

        Canvas.ForceUpdateCanvases();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
}
