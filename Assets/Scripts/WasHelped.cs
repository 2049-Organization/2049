using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WasHelped : MonoBehaviour
{
    void Start()
    {
        if (PlayerData.Instance != null)
        {
            GetComponent<TextMeshProUGUI>().text = PlayerData.Instance.wasHelped + " fois";
        }
    }
}
