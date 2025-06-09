using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Object{
    Medicine,
    Raincoat,
    Phone_charger,
    Tin_can,
    Bus_card,
    Bank_card,
    Null
}

public class PlayerData : MonoBehaviour
{
    public Dictionary<string, Object> object_map = new Dictionary<string, Object>();

    public Object obj1 = Object.Null;
    public Object obj2 = Object.Null;
    public Object obj3 = Object.Null;

    [SerializeField] private int health = 10;

    public static PlayerData Instance { get; private set; }

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

        object_map["Boîte de médicament"] = Object.Medicine;
        object_map["Tenue de pluie"] = Object.Raincoat;
        object_map["Chargeur de téléphone portable"] = Object.Phone_charger;
        object_map["Boîte de conserve"] = Object.Tin_can;
        object_map["Pass de bus"] = Object.Bus_card;
        object_map["Carte bancaire"] = Object.Bank_card;
    }
}
