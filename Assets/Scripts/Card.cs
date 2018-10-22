using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    GameObject dbOBject;

    DatabaseManager dbManager;
    FirebaseManager fbManager;

    ActionStats[] actionSlots;

    float maxHP;
    float maxMana;

    [SerializeField]
    int cardID;

    [SerializeField]
    float hp;
    [SerializeField]
    float defense;
    [SerializeField]
    float mana;
    

    [SerializeField]
    string cardName;

    [SerializeField]
    int[] currentActions;
    [SerializeField]
    int[] defaultActions;

    [SerializeField]
    Sprite cardImage;
    [SerializeField]
    Sprite cardSprite;


    public class ActionStats
    {
        int id;
        string name;
        float healthBonus;
        float attack;
        float defense;
        float mana;
        string special;


        public ActionStats(int _id, string _name, float _healthBonus, float _attack, float _defense, float _mana, string _special)
        {
            id = _id;
            name = _name;
            healthBonus = _healthBonus;
            attack = _attack;
            defense = _defense;
            mana = _mana;
            special = _special;
        }
        public int GetID()
        {
            return id;
        }
        public string GetName()
        {
            return name;
        }
        public float GetHealth()
        {
            return healthBonus;
        }
        public float GetAttack()
        {
            return attack;
        }
        public float GetDefense()
        {
            return defense;
        }
        public float GetMana()
        {
            return mana;
        }
        public string GetSpecial()
        {
            return special;
        }
        public void SetID(int _id)
        {
            id = _id;
        }
        public void SetName(string _name)
        {
            name = _name;
        }
        public void SetHealthBonus(float _healthBonus)
        {
            healthBonus = _healthBonus;
        }
        public void SetAttack(float _attack)
        {
            attack = _attack;
        }
        public void SetDefense(float _defense)
        {
            defense = _defense;
        }
        public void SetMana(float _mana)
        {
            mana = _mana;
        }
        public void SetSpecial(string _special)
        {
            special = _special;
        }
        public string GetStats()
        {
            return id.ToString() + " " + name + " " + healthBonus.ToString() + " " + attack.ToString() + " " + defense.ToString() + " " + special;
        }

    }

    private void Start()
    {
        maxHP = hp;
        maxMana = mana;

        actionSlots = new ActionStats[4];

        dbOBject = GameObject.Find("DatabaseManager");
        dbManager = dbOBject.GetComponent<DatabaseManager>();
        fbManager = dbManager.GetComponent<FirebaseManager>();

        if (PlayerPrefs.HasKey("Slot1"))
            currentActions[0] = PlayerPrefs.GetInt("Slot1");
        else
            currentActions[0] = defaultActions[0];

        if (PlayerPrefs.HasKey("Slot2"))
            currentActions[1] = PlayerPrefs.GetInt("Slot2");
        else
            currentActions[1] = defaultActions[1];

        if (PlayerPrefs.HasKey("Slot3"))
            currentActions[2] = PlayerPrefs.GetInt("Slot3");
        else
            currentActions[2] = -1;

        if (PlayerPrefs.HasKey("Slot4"))
            currentActions[3] = PlayerPrefs.GetInt("Slot4");
        else
            currentActions[3] = -1;

        actionSlots[0] = new ActionStats(currentActions[0], "empty", 0f, 0f, 0f, 0f, "NULL");
        actionSlots[1] = new ActionStats(currentActions[1], "empty", 0f, 0f, 0f, 0f, "NULL");
        actionSlots[2] = new ActionStats(currentActions[2], "empty", 0f, 0f, 0f, 0f, "NULL");
        actionSlots[3] = new ActionStats(currentActions[3], "empty", 0f, 0f, 0f, 0f, "NULL");

        //if (actionSlots[0].GetID() != -1)
        //    dbManager.ConsultDatabase(actionSlots[0]);
        //if (actionSlots[1].GetID() != -1)
        //    dbManager.ConsultDatabase(actionSlots[1]);
        //if (actionSlots[2].GetID() != -1)
        //    dbManager.ConsultDatabase(actionSlots[2]);
        //if (actionSlots[3].GetID() != -1)
        //    dbManager.ConsultDatabase(actionSlots[3]);

        if (actionSlots[0].GetID() != -1)
            fbManager.ConsultDatabase(actionSlots[0]);
        if (actionSlots[1].GetID() != -1)
            fbManager.ConsultDatabase(actionSlots[1]);
        if (actionSlots[2].GetID() != -1)
            fbManager.ConsultDatabase(actionSlots[2]);
        if (actionSlots[3].GetID() != -1)
            fbManager.ConsultDatabase(actionSlots[3]);


    }
    public void SetHP(float _hp)
    {
        hp = _hp;
    }
    public float GetHP()
    {
        return hp;
    }
    public float GetMaxHP()
    {
        return maxHP;
    }
    public void SetDefense(float _defense)
    {
        defense = _defense;
    }
    public void SetID(int id)
    {
        cardID = id;
    }
    public void SetMana(float _mana)
    {
        mana = _mana;
    }

    public float GetMana()
    {
        return mana;
    }
    public float GetMaxMana()
    {
        return maxMana;
    }
    public float GetDefense()
    {
        return defense;
    }
    public int GetID()
    {
        return cardID;
    }
    public Sprite GetCardImage()
    {
        return cardImage;
    }
    public Sprite GetCardSprite()
    {
        return cardSprite;
    }
    public ActionStats GetAction(int index)
    {
        return actionSlots[index];
    }
}
