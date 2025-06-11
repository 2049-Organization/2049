using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bilan : MonoBehaviour
{
    [SerializeField] private RectTransform finalMessage;

    [SerializeField] private List<RectTransform> destroyOnClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (RectTransform rect in destroyOnClick)
            {
                if (rect != null)
                {
                    Destroy(rect.gameObject);
                }
            }
            if (finalMessage != null)
            {
                finalMessage.gameObject.SetActive(true);
            }
        }
    }
}
