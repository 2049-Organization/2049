using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spent : MonoBehaviour
{
    void Start()
    {
        if (PlayerData.Instance != null)
        {
            GetComponent<TextMeshProUGUI>().text = PlayerData.Instance.moneyAtStart - PlayerData.Instance.GetMoney() + " liasse de billets";
        }
    }
}
