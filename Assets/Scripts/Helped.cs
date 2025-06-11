using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Helped : MonoBehaviour
{
    void Start()
    {
        if(PlayerData.Instance != null)
        {
            GetComponent<TextMeshProUGUI>().text = PlayerData.Instance.helped + " fois";
        }
    }
}
