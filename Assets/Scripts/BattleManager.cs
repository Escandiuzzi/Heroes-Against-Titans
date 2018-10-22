using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    IEnumerator IAThinkingTime()
    {
        float randTime = Random.Range(.5f, 2f);
        float counter = 0f;
        crRunning = true;

        while (counter < randTime)
        {
            counter += Time.deltaTime;
            yield return null; 
        }

       
        int index = 0;

        float health = currentCard.GetAction(index).GetHealth();
        float atk = currentCard.GetAction(index).GetAttack();
        float defense = currentCard.GetAction(index).GetDefense();
        string special = currentCard.GetAction(index).GetSpecial();

        targetCard = DefineTargetCard();

        if (targetCard != null && playerCards.Count > 0)
        {
            float _atk = atk - targetCard.GetDefense();

            if (_atk > 0)
            {
                float _hp = targetCard.GetHP() - atk;
                targetCard.SetHP(_hp);
            }

            else
            {
                targetCard.SetDefense(targetCard.GetDefense() - atk);
            }

            float __hp = currentCard.GetHP() + health;

            if (__hp > 100)
                __hp = 100;

            currentCard.SetHP(__hp);

            if (targetCard.GetHP() <= 0)
            {
                charactersHB[targetCard.GetID()].SetActive(false);

                cardsList.Remove(targetCard);
                cAmount--;

                playerCards.Remove(targetCard);

                Destroy(charactersSlots[targetCard.GetID()]);
            }

            attack = false;
            targetCard = null;

            cIndex++;
            if (cIndex >= cAmount)
                cIndex = 0;
            currentCard = cardsList[cIndex];
            CurrentPlayerMiniature();

            if (currentCard.tag == "Player")
            {
                selectionHUD.SetActive(true);
                actionHUD.SetActive(false);
            }
        }
        crRunning = false;
    }


    int cIndex;
    int cAmount;

    List<Card> cardsList;

    [SerializeField]
    GameObject ccImage;

    GameObject player;

    PlayerController pController;

    Card currentCard;
    Card targetCard;

    bool attack;
    bool battle;
    bool crRunning;

    List<Card> playerCards;

    [SerializeField]
    List<Card> enemyCards;

    [SerializeField]
    GameObject hbHUD;
    [SerializeField]
    GameObject manaHUD;
    [SerializeField]
    GameObject selectionHUD;
    [SerializeField]
    GameObject actionHUD;
    [SerializeField]
    GameObject egPanel;

    [SerializeField]
    GameObject[] charactersSlots;
    [SerializeField]
    GameObject[] enemiesSlots;
    [SerializeField]
    GameObject[] enemiesHB;
    [SerializeField]
    GameObject[] charactersHB;
    [SerializeField]
    GameObject[] arrow;

    [SerializeField]
    Text endGameText;
    
    [SerializeField]
    Text[] actionText;

    void Start () {
        player = GameObject.Find("Player");
        pController = player.GetComponent<PlayerController>();

        //playerCards = new List<Card>();
        //enemyCards = new List<Card>();
        cardsList = new List<Card>();

        InitializePlayerDeck();
        InitializeEnemyDeck();
        InitializeGame();

        battle = true;

    }
	void Update () {

        if (battle)
        {
            UpdateHealthBar();
            UpdateManaBar();

            if (attack && targetCard != null)
            {
                selectionHUD.SetActive(false);
                actionHUD.SetActive(true);

                for (int i = 0; i < 4; i++)
                {
                    if (currentCard.GetAction(i).GetID() != -1)
                        actionText[i].text = currentCard.GetAction(i).GetName();
                }
            }
            if (currentCard.tag != "Player")
            {
                selectionHUD.SetActive(false);
                actionHUD.SetActive(false);
                IAInteraction();
            }
        }

        if (enemyCards.Count == 0)
        {
            egPanel.SetActive(true);
            endGameText.text = "You Win!";
            battle = false;
        }
        else if (playerCards.Count == 0)
        {
            egPanel.SetActive(true);
            endGameText.text = "You Lose";
            battle = false;
        }
	}

    void InitializePlayerDeck()
    {
        playerCards = pController.GetDeck();

        for (int i = 0; i < 3; i++){
            if (playerCards[i] != null)
            {
                charactersSlots[i].GetComponent<SpriteRenderer>().sprite = playerCards[i].GetCardSprite();
                playerCards[i].gameObject.tag = "Player";
                playerCards[i].SetID(i);
            }
        }
    }

    void InitializeEnemyDeck()
    {
        for (int i = 0; i < 3; i++)
        {
            if (enemyCards[i] != null)
                enemiesSlots[i].GetComponent<Image>().sprite = enemyCards[i].GetCardSprite();
        }
    }
    void InitializeGame()
    {
        cIndex = 0;
  
        for(int i = 0; i < 3; i++)
        {
            if (playerCards[i] != null)
            {
                cardsList.Add(playerCards[i]);
                cAmount++;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (enemyCards[i] != null)
            {
                cardsList.Add(enemyCards[i]);
                cAmount++;
            }
        }

        currentCard = cardsList[cIndex];
        CurrentPlayerMiniature();
    }
    void CurrentPlayerMiniature()
    {
        ccImage.GetComponent<Image>().sprite = currentCard.GetCardImage();
    }

    public void SetTarget(Card card)
    {
        targetCard = card;
    }

    public void SelectAttack()
    {
        attack = true;
    }
    public void ActionSlot(int index)
    {
        foreach (GameObject a in arrow)
            a.SetActive(false);

        float health = currentCard.GetAction(index).GetHealth();
        float atk = currentCard.GetAction(index).GetAttack();
        float defense = currentCard.GetAction(index).GetDefense();
        string special = currentCard.GetAction(index).GetSpecial();

        float _atk = atk - targetCard.GetDefense();

        if (_atk > 0)
        {
            float _hp = targetCard.GetHP() - atk;
            targetCard.SetHP(_hp);
        }

        else
        {
            targetCard.SetDefense(targetCard.GetDefense() - atk);
        }

        float __hp = currentCard.GetHP() + health;

        if (__hp > 100)
            __hp = 100;

        currentCard.SetHP(__hp);


        if (targetCard.GetHP() <= 0)
        {
            enemiesHB[targetCard.GetID()].SetActive(false);
            enemiesSlots[targetCard.GetID()].SetActive(false);
            cardsList.Remove(targetCard);
            cAmount--;

            enemyCards.Remove(targetCard);

            Destroy(targetCard.gameObject);
        }

        attack = false;
        targetCard = null;

        cIndex++;

        if (cIndex >= cardsList.Count)
            cIndex = 0;

        currentCard = cardsList[cIndex];
        CurrentPlayerMiniature();

        selectionHUD.SetActive(true);
        actionHUD.SetActive(false);
    }
 

    public void UpdateHealthBar()
    {
   
        float cHp = currentCard.GetHP();
        cHp = cHp / currentCard.GetMaxHP();
        hbHUD.GetComponent<Image>().fillAmount = cHp;

        for (int i = 0; i < playerCards.Count; i++)
        {
            if (playerCards[i] != null)
            {
                float hp = playerCards[i].GetHP();

                hp = hp / playerCards[i].GetMaxHP();

                charactersHB[playerCards[i].GetID()].GetComponent<Image>().fillAmount = hp;
            }
            else
                charactersHB[i].SetActive(false);
        }
        for (int i = 0; i < enemyCards.Count; i++)
        {

            if (enemyCards[i] != null)
            {
                float hp = enemyCards[i].GetHP();

                hp = hp / enemyCards[i].GetMaxHP(); 

                enemiesHB[enemyCards[i].GetID()].GetComponent<Image>().fillAmount = hp;
            }
            else
                enemiesHB[i].SetActive(false);

        }

    }

    public void UpdateManaBar()
    {
        float cMana = currentCard.GetMana();
        cMana = cMana / currentCard.GetMaxMana();
        manaHUD.GetComponent<Image>().fillAmount = cMana;
    }

    void IAInteraction()
    {
        if(!crRunning)
            StartCoroutine(IAThinkingTime());
    }

    Card DefineTargetCard()
    {
        Card target = null;

        if (playerCards.Count != 0)
        {
            int rand = Random.Range(0, playerCards.Count);

            if (playerCards[rand] != null && playerCards.Count > 0)
                target = playerCards[rand];
        }
        return target;
    }
}
