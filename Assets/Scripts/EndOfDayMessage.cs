using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndOfDayMessage : MonoBehaviour
{
    public void SetDay(int day)
    {
        GetComponent<TextMeshProUGUI>().text = "Fin du jour " + day;
    }
}
