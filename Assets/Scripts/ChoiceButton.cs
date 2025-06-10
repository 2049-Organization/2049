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

    [SerializeField] private Sprite medicine;
    [SerializeField] private Sprite raincoat;
    [SerializeField] private Sprite phone_charger;
    [SerializeField] private Sprite tincan;
    [SerializeField] private Sprite bus_pass;
    [SerializeField] private Sprite money;

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;

    private Button btn;

    private void Awake()
    {
        le = GetComponent<LayoutElement>();
        btn = GetComponent<Button>();
    }

    public void Init(DialogueChoice choiceNode)
    {
        text.text = choiceNode.choiceText;

        switch (choiceNode.choiceText)
        {
            case "Boîte de médicament":
                icon.sprite = medicine;
                break;
            case "Tenue de pluie":
                icon.sprite = raincoat;
                break;
            case "Chargeur de téléphone":
                icon.sprite = phone_charger;
                break;
            case "Boîte de conserve":
                icon.sprite = tincan;
                break;
            case "Ticket de bus":
                icon.sprite = bus_pass;
                break;
            case "Liasse de billets":
                icon.sprite = money;
                break;
            default:
                break;
        }

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (PlayerData.Instance.object_map.ContainsKey(choiceNode.choiceText))
            {
                if(PlayerData.Instance.GetObj1() == Object.Null)
                {
                    PlayerData.Instance.SetObj1(PlayerData.Instance.object_map[choiceNode.choiceText]);
                    DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();
                    newNode.speaker = MessageSender.Player;
                    newNode.nodeId = "obj1";
                    newNode.timeToWait = 2f;
                    newNode.directNextNode = fourthDialogueNode;
                    newNode.message = GetObjectMessage(PlayerData.Instance.GetObj1());
                    if(newNode.message == "Tu vas sûrement avoir besoin de manger, prends des conserves !")
                    {
                        newNode.bubbleSize = BubbleSize.S;
                    }
                    else
                    {
                        newNode.bubbleSize = BubbleSize.XS;
                    }

                    MessageSystem.Instance.StartSendingMessages(newNode);
                }
                else if (PlayerData.Instance.GetObj2() == Object.Null)
                {
                    PlayerData.Instance.SetObj2(PlayerData.Instance.object_map[choiceNode.choiceText]);
                    DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();
                    newNode.speaker = MessageSender.Player;
                    newNode.nodeId = "obj2";
                    newNode.timeToWait = 2f;
                    newNode.directNextNode = fifthDialogueNode;
                    newNode.message = GetObjectMessage(PlayerData.Instance.GetObj2());
                    if (newNode.message == "Tu vas sûrement avoir besoin de manger, prends des conserves !")
                    {
                        newNode.bubbleSize = BubbleSize.S;
                    }
                    else
                    {
                        newNode.bubbleSize = BubbleSize.XS;
                    }

                    MessageSystem.Instance.StartSendingMessages(newNode);
                }
                else if (PlayerData.Instance.GetObj3() == Object.Null)
                {
                    PlayerData.Instance.SetObj3(PlayerData.Instance.object_map[choiceNode.choiceText]);
                    DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();
                    newNode.speaker = MessageSender.Player;
                    newNode.nodeId = "obj3";
                    newNode.timeToWait = 2f;
                    newNode.message = GetObjectMessage(PlayerData.Instance.GetObj3());
                    newNode.directNextNode = sixthDialogueNode;
                    if (newNode.message == "Tu vas sûrement avoir besoin de manger, prends des conserves !")
                    {
                        newNode.bubbleSize = BubbleSize.S;
                    }
                    else
                    {
                        newNode.bubbleSize = BubbleSize.XS;
                    }
                            
                    MessageSystem.Instance.StartSendingMessages(newNode);
                }
            }
            else
            {
                MessageSystem.Instance.StartSendingMessages(choiceNode.nextNode);
            }

            switch (choiceNode.choiceText)
            {
                case "Bus: -ticket de bus":
                    if (PlayerData.Instance.GetObj1() == Object.Bus_card)
                    {
                        PlayerData.Instance.SetObj1(Object.Null);
                    }
                    if (PlayerData.Instance.GetObj2() == Object.Bus_card)
                    {
                        PlayerData.Instance.SetObj2(Object.Null);
                    }
                    if (PlayerData.Instance.GetObj3() == Object.Bus_card)
                    {
                        PlayerData.Instance.SetObj3(Object.Null);
                    }
                    PlayerData.Instance.tookTheBus = true;
                    break;
                case "Voiture: -1 santé":
                    PlayerData.Instance.SetHealth(PlayerData.Instance.GetHealth() - 1);
                    PlayerData.Instance.tookTheCar = true;
                    break;
                case "Taxi: -1 argent":
                    PlayerData.Instance.SetMoney(PlayerData.Instance.GetMoney() - 1);
                    PlayerData.Instance.tookTheTaxi = true;
                    break;
                case "Dormir dehors: -1 santé":
                    PlayerData.Instance.SetHealth(PlayerData.Instance.GetHealth() - 1);
                    break;
                case "Entrer: Maladie":
                    PlayerData.Instance.illness = true;
                    break;
                case "Ne pas dormir: -2 santé":
                    PlayerData.Instance.SetHealth(PlayerData.Instance.GetHealth() - 2);
                    break;
                case "Donner à manger: -Boite de conserve":
                    if (PlayerData.Instance.GetObj1() == Object.Tin_can)
                    {
                        PlayerData.Instance.SetObj1(Object.Null);
                    }
                    if(PlayerData.Instance.GetObj2() == Object.Tin_can)
                    {
                        PlayerData.Instance.SetObj2(Object.Null);
                    }
                    if (PlayerData.Instance.GetObj3() == Object.Tin_can)
                    {
                        PlayerData.Instance.SetObj3(Object.Null);
                    }
                    break;
                case "Prendre un taxi: -2 argent":
                    PlayerData.Instance.SetMoney(PlayerData.Instance.GetMoney() - 2);
                    break;
                case "Soudoyer le contrôleur: -2 argent":
                    PlayerData.Instance.SetMoney(PlayerData.Instance.GetMoney() - 2);
                    break;
                case "Monter dans le train: -médicaments":
                    if (PlayerData.Instance.GetObj1() == Object.Medicine)
                    {
                        PlayerData.Instance.SetObj1(Object.Null);
                    }
                    if (PlayerData.Instance.GetObj2() == Object.Medicine)
                    {
                        PlayerData.Instance.SetObj2(Object.Null);
                    }
                    if (PlayerData.Instance.GetObj3() == Object.Medicine)
                    {
                        PlayerData.Instance.SetObj3(Object.Null);
                    }
                    break;
                case "Oui: -1 santé":
                    PlayerData.Instance.SetHealth(PlayerData.Instance.GetHealth() - 1);
                    break;
                case "Non: +1 santé":
                    PlayerData.Instance.SetHealth(PlayerData.Instance.GetHealth() + 1);
                    break;
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
                return "Tu vas sûrement devoir charger ton téléphone !";
            case Object.Tin_can:
                return "Tu vas sûrement avoir besoin de manger, prends des conserves !";
            case Object.Bus_card:
                return "Tu vas sûrement avoir besoin de prendre le bus !";
            case Object.Money:
                return "Tu vas sûrement devoir payer des trucs, prends de l'argent !";
        }

        return "";
    }

    private void Update()
    {
        
        t += transitionSpeed * Time.deltaTime;
        le.preferredWidth = Mathf.Lerp(le.preferredWidth, width, t);

        if (icon.sprite != null)
        {
            if (le.preferredWidth < 0.1f && text.isActiveAndEnabled && !icon.isActiveAndEnabled)
            {
                text.enabled = false;
                icon.enabled = true;
            }
            else if(le.preferredWidth > 0.1f && !text.isActiveAndEnabled && icon.isActiveAndEnabled)
            {
                text.enabled = true;
                icon.enabled = false;
            }
        }
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

    public void SetUnavailable()
    {
        btn.interactable = false;
    }
}
