using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/Node", order = 1)]
public class DialogueNode : ScriptableObject
{
    public string nodeId;

    [TextArea(2, 5)]
    public string message;

    public MessageSender speaker;

    public List<DialogueChoice> choices;

    public float timeToWait = 1.5f;

    public DialogueNode directNextNode;

    public BubbleSize bubbleSize;

    public bool isEndOfDay = false;

    public Sprite image;
}


[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
    public Object neededObject;
}