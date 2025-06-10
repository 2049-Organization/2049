using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Object{
    Medicine,
    Raincoat,
    Phone_charger,
    Tin_can,
    Bus_card,
    Money,
    Null
}

public class PlayerData : MonoBehaviour
{
    public Dictionary<string, Object> object_map = new Dictionary<string, Object>();

    private Object obj1 = Object.Null;
    private Object obj2 = Object.Null;
    private Object obj3 = Object.Null;

    [SerializeField] private int health = 5;
    [SerializeField] private int money = 5;

    public bool tookTheBus = false;
    public bool tookTheCar = false;
    public bool tookTheTaxi = false;

    public bool illness = false;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider moneySlider;


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
        object_map["Chargeur de téléphone"] = Object.Phone_charger;
        object_map["Boîte de conserve"] = Object.Tin_can;
        object_map["Ticket de bus"] = Object.Bus_card;
        object_map["Liasse de billets"] = Object.Money;
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
        healthSlider.value = health;
    }
    public void SetMoney(int newMoney)
    {
        money = newMoney;
        moneySlider.value = money;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMoney()
    {
        return money;
    }

    public Object GetObj1()
    {
        return obj1;
    }

    public Object GetObj2()
    {
        return obj2;
    }

    public Object GetObj3()
    {
        return obj3;
    }

    public void SetObj1(Object obj)
    {
        obj1 = obj;
        if(obj1 == Object.Money)
        {
            SetMoney(money + 2);
        }
    }

    public void SetObj2(Object obj)
    {
        obj2 = obj;
        if (obj2 == Object.Money)
        {
            SetMoney(money + 2);
        }
    }

    public void SetObj3(Object obj)
    {
        obj3 = obj;
        if (obj3 == Object.Money)
        {
            SetMoney(money + 2);
        }
    }
}

