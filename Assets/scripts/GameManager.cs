using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  GamePlayerManager player;
    public GamePlayerManager enemy;


    [SerializeField] UIManager uIManager;
    [SerializeField] AI enemyAI;
    CardController card;

    public Transform PlayerHero;

    //手札にカードを生成
    public Transform PlayerHand,
                     EnemyHand,
                     EnemyField,
                     PlayerField;

    [SerializeField] CardController cardPrehub;
    public bool isPlayerTurn;

    public static GameManager instance;

    //時間管理
    int timeCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        uIManager.ResultPanel();
        card = GetComponent<CardController>();
        player.Init(new List<int>() { 1, 2, 3 ,4});
        enemy.Init(new List<int>() { 1, 2, 3 ,4});
        timeCount = 5;
        uIManager.UpdateTime(timeCount);
        uIManager.ShowHeroHp(player.heroHp, enemy.heroHp);
        uIManager.ShowManaCost(player.manaCost,enemy.manaCost);
        SettingHand();
        isPlayerTurn = true;
        TurnCalc();
    }
    public void ReduceManaCost(int cost,bool isPlayer)
    {
        if(isPlayer)
        {
            player.manaCost -= cost;
        }
        else
        {
            enemy.manaCost -= cost;
        }
        uIManager.ShowManaCost(player.manaCost, enemy.manaCost);
    }
    public void Retry()
    {
        //handとfieldの削除
        foreach(Transform card in PlayerHand)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in PlayerField)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in EnemyHand)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in EnemyField)
        {
            Destroy(card.gameObject);
        }
        //デッキの生成
        player.deck = new List<int>() { 3, 1, 2, 2, 3, 1 };
        enemy.deck = new List<int>() { 2, 1, 2, 1, 3, 3 };
        StartGame();
    }
    void SettingHand()
    {
        for (int i = 0; i < 3; i++)
        {
            GiveCardToHand(player.deck,PlayerHand);
            GiveCardToHand(enemy.deck,EnemyHand);
        }
    }
    void GiveCardToHand(List<int> deck, Transform hand)
    {
        if (deck.Count == 0)
        {
            return;
        }
        int cardID = deck[0];
        deck.RemoveAt(0);
        CreateCard(cardID, hand);
    }
    void CreateCard(int cardID,Transform hand)
    {
        //カードの生成とデータの受け渡し
        CardController card = Instantiate(cardPrehub, hand, false);
        if(hand.name == "PlayerHand")
        {
            card.Init(cardID,true);
        }
        else
        {
            card.Init(cardID, false);
        }
    }
    void TurnCalc()
    {
        StopAllCoroutines();
        StartCoroutine(CountDown());

        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            StartCoroutine(enemyAI.EnemyTurn());
        }
    }
    IEnumerator CountDown()
    {
        timeCount = 5;
        uIManager.UpdateTime(timeCount);

        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1); //1s待機
            timeCount--;
            uIManager.UpdateTime(timeCount);
        }
        ChangeTurn();
    }
    public void OnClickturnEnd()
    {
        if(isPlayerTurn)
        {
            ChangeTurn();
        }
    }

    public void ChangeTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        CardController[] PlayerfieldList = PlayerField.GetComponentsInChildren<CardController>();
        SettingCanAttackView(PlayerfieldList, false);

        CardController[] enemyfieldList = EnemyField.GetComponentsInChildren<CardController>();
        SettingCanAttackView(enemyfieldList, false);

        if (isPlayerTurn)
        {
            player.IncreaseManaCost();
            GiveCardToHand(player.deck, PlayerHand);
        }
        else
        {
            enemy.IncreaseManaCost();
            GiveCardToHand(enemy.deck, EnemyHand);
        }
        uIManager.ShowManaCost(player.manaCost,enemy.manaCost);
        TurnCalc();
    }
    public void SettingCanAttackView(CardController[] fieldCardList,bool canAttack)
    {
        foreach (CardController playerCard in fieldCardList)
        {
            playerCard.SetCanAttack(canAttack);
        }
    }

    void PlayerTurn()
    {
        //攻撃可能
        CardController[] PlayerfieldList = PlayerField.GetComponentsInChildren<CardController>();
        SettingCanAttackView(PlayerfieldList, true);
    }

    public CardController[] GetEnemyFieldCard(bool isPlayer)
    {
        if(isPlayer)
        {
            return EnemyField.GetComponentsInChildren<CardController>();
        }
        else
        {
            return PlayerField.GetComponentsInChildren<CardController>();
        }
    }

    public void CardBattle(CardController attacker,CardController defender)
    {
        attacker.Attack(defender);
        defender.Attack(attacker);
        attacker.CheckAlive();
        defender.CheckAlive();
    }
    public void AttackToHero(CardController attacker)
    {
        if (card.model.isPlayerCard)
        {
            enemy.heroHp -= attacker.model.at;
        }
        else
        {
            player.heroHp -= attacker.model.at;
        }
        attacker.SetCanAttack(false);
        uIManager.ShowHeroHp(player.heroHp,enemy.heroHp);
    }
    public void CheckHeroHp()
    {
        if(player.heroHp <= 0 || enemy.heroHp <= 0)
        {
            ShowResultPanel(player.heroHp);
        }
    }
    void ShowResultPanel(int HeroHp)
    {
        StopAllCoroutines();
        uIManager.ShowResultPanel(HeroHp);
    }

}
