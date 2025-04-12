using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public enum CardType
{
    Attack,
    Heal,
    Shield,
    Jocker
}

public enum ActionType
{
    PlayerAttack,
    BossAttack,
    Heal,
    Shield
}


public enum PlayerTurnState
{
    WaitingForCardScan,
    WaitingForActionConfirm,
}


public class GameManager2s : MonoBehaviour
{
    [Header("Game Setup")]
    public int playersNum = 4;
    public int maxPlayerHP = 15;
    public int initialBossHP = 20;
    public int maxShield = 10;

    [Header("Game State")]
    public List<int> playerHPs = new List<int>();
    public List<int> playerShields = new List<int>();
    public int BossHP;
    public int currentPlayerIndex = 0;
    public bool isPlayerTurn = true;
    public PlayerTurnState currentTurnState;

    [Header("Card Action State (Pending)")]
    private CardType pendingCardType;
    private int pendingCardChance;
    private int pendingCardValue;
    private bool cardIsPending = false;

    [Header("Jocker State")]
    private CardType jockerChosenMove;
    private bool waitingForJokerChoice = false;

    [Header("Feedback")]
    public bool lastActionMissed = false;

    public UnityEvent<int> OnPlayerTurnStart;
    public UnityEvent OnBossTurnStart;
    public UnityEvent<int, int> OnPlayerHPChange;
    public UnityEvent<int, int> OnPlayerShieldChange;
    public UnityEvent<int> OnBossHPChange;
    public UnityEvent OnVictory;
    public UnityEvent OnDefeat;
    public UnityEvent<ActionType> OnMiss;
    public UnityEvent OnWaitingForCard;
    public UnityEvent<CardType, int, int> OnCardScanned;
    public UnityEvent OnWaitingForConfirmation;
    public UnityEvent<bool> OnActionApplied;


    private System.Random rand = new System.Random();

    void Start()
    {
        InitializeGame(playersNum);
    }

    void InitializeGame(int numberOfPlayers)
    {
        playersNum = numberOfPlayers;
        playerHPs.Clear();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerHPs.Add(maxPlayerHP);
            OnPlayerHPChange?.Invoke(i, maxPlayerHP);
        }
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerShields.Add(0);
            OnPlayerShieldChange?.Invoke(i, 0);
        }
        BossHP = initialBossHP;
        OnBossHPChange?.Invoke(BossHP);

        currentPlayerIndex = 0;
        isPlayerTurn = true;
        waitingForJokerChoice = false;
        StartPlayerTurn(currentPlayerIndex);
    }

    void Update()
    {

        if (isPlayerTurn && currentTurnState == PlayerTurnState.WaitingForCardScan)
        {
            ReceiveCardData(CardType.Attack, 80, 5);
        }
        
        if (isPlayerTurn && currentTurnState == PlayerTurnState.WaitingForActionConfirm)
        {
            ConfirmAction();
        }

        if (waitingForJokerChoice)
        {
            //Get Button Response 
            ConfirmJokerChoice(CardType.Attack);
        }
    }


    void StartPlayerTurn(int playerIndex)
    {
        while (playerIndex < playerHPs.Count && playerHPs[playerIndex] <= 0)
        {
            playerIndex++;
        }

        if (playerIndex >= playerHPs.Count)
        {
            CheckForGameOver();
            if (BossHP > 0)
            {
                StartBossTurn();
            }
            return;
        }

        currentPlayerIndex = playerIndex;
        isPlayerTurn = true;
        cardIsPending = false;
        waitingForJokerChoice = false;
        lastActionMissed = false;
        currentTurnState = PlayerTurnState.WaitingForCardScan;
        OnPlayerTurnStart?.Invoke(currentPlayerIndex);
        OnWaitingForCard?.Invoke();
    }

    void StartBossTurn()
    {
        isPlayerTurn = false;
        cardIsPending = false;
        waitingForJokerChoice = false;
        lastActionMissed = false;
        OnBossTurnStart?.Invoke();

        BossMove();

        if (!CheckForGameOver())
        {
            StartPlayerTurn(0);
        }
    }

    void NextTurn()
    {
        if (isPlayerTurn)
        {
            int nextPlayerIndex = currentPlayerIndex + 1;
            if (nextPlayerIndex >= playersNum)
            {
                StartBossTurn();
            }
            else
            {
                StartPlayerTurn(nextPlayerIndex);
            }
        }
    }


    public void ReceiveCardData(CardType type, int chance, int value)
    {
        if (!isPlayerTurn || currentTurnState != PlayerTurnState.WaitingForCardScan)
        {
            return;
        }

        pendingCardType = type;
        pendingCardChance = chance;
        pendingCardValue = value;
        cardIsPending = true;
        currentTurnState = PlayerTurnState.WaitingForActionConfirm;

        OnCardScanned?.Invoke(type, chance, value);
        OnWaitingForConfirmation?.Invoke();
    }


    public void ConfirmAction()
    {
        if (!isPlayerTurn || currentTurnState != PlayerTurnState.WaitingForActionConfirm || !cardIsPending)
        {
            return;
        }

        cardIsPending = false;

        ApplyCardAction(pendingCardType, pendingCardChance, pendingCardValue);

        if (!waitingForJokerChoice)
        {
            if (!CheckForGameOver())
            {
                NextTurn();
            }
        }
    }


    void ApplyCardAction(CardType type, int chance, int value)
    {
        lastActionMissed = false;
        bool success = false;

        switch (type)
        {
            case CardType.Attack:
                success = ApplyAttack(chance, value);
                break;
            case CardType.Heal:
                success = ApplyHeal(chance, value);
                break;
            case CardType.Jocker:
                waitingForJokerChoice = true;
                pendingCardChance = chance;
                pendingCardValue = value;
                ShowJokerChoiceUI();
                break;
            case CardType.Shield:
                success = ApplyShield(chance, value);
                break;
            default:
                success = false;
                break;
        }

        OnActionApplied?.Invoke(success);
        AnimateAction(success);
    }

    bool ApplyShield(int chance, int value)
    {
        int targetPlayerIndex = currentPlayerIndex;

        if (playerShields[targetPlayerIndex] <= 0 || playerHPs[targetPlayerIndex] >= maxShield)
        {
            OnActionApplied?.Invoke(true);
            AnimateAction(true);
            return true;
        }

        if (rand.Next(101) <= chance)
        {
            int potentialShield = playerShields[targetPlayerIndex] + value;
            int actualShield = Mathf.Clamp(potentialShield, 0, 10) - playerShields[targetPlayerIndex];

            if (actualShield > 0)
            {
                playerShields[targetPlayerIndex] += actualShield;
                OnPlayerShieldChange?.Invoke(targetPlayerIndex, playerShields[targetPlayerIndex]);
            }

            return true;
        }
        else
        {
            Miss(ActionType.Shield);
            return false;
        }
    }
        bool ApplyAttack(int chance, int value)
    {
        if (rand.Next(101) <= chance)
        {
            BossHP -= value;
            BossHP = Mathf.Max(0, BossHP);
            OnBossHPChange?.Invoke(BossHP);
            return true;
        }
        else
        {
            Miss(ActionType.PlayerAttack);
            return false;
        }
    }

    bool ApplyHeal(int chance, int value)
    {
        int targetPlayerIndex = currentPlayerIndex;

        if (playerHPs[targetPlayerIndex] <= 0 || playerHPs[targetPlayerIndex] >= maxPlayerHP)
        {
            OnActionApplied?.Invoke(true);
            AnimateAction(true);
            return true;
        }

        if (rand.Next(101) <= chance)
        {
            int potentialHP = playerHPs[targetPlayerIndex] + value;
            int actualHeal = Mathf.Clamp(potentialHP, 0, maxPlayerHP) - playerHPs[targetPlayerIndex];

            if (actualHeal > 0)
            {
                playerHPs[targetPlayerIndex] += actualHeal;
                OnPlayerHPChange?.Invoke(targetPlayerIndex, playerHPs[targetPlayerIndex]);
            }

            return true;
        }
        else
        {
            Miss(ActionType.Heal);
            return false;
        }
    }


    void ShowJokerChoiceUI()
    {
        Debug.Log("IMPLEMENT UI: Show Joker Choice (Attack/Heal/Shield)");
    }


    void ConfirmJokerChoice(CardType choice)
    {
        if (!waitingForJokerChoice)
        {
            return;
        }

        waitingForJokerChoice = false;


        bool success = false;
        if (choice == CardType.Attack)
        {
            int jokerAttackChance = pendingCardChance;
            int jokerAttackValue = pendingCardValue;
            success = ApplyAttack(jokerAttackChance, jokerAttackValue);
        }
        else if (choice == CardType.Heal)
        {
            int jokerHealChance = pendingCardChance;
            int jokerHealValue = pendingCardValue;
            success = ApplyHeal(jokerHealChance, jokerHealValue);
        }

        OnActionApplied?.Invoke(success);
        AnimateAction(success);

        if (!CheckForGameOver())
        {
            NextTurn();
        }
    }


    void BossMove()
    {
        int targetPlayerIndex = SelectLivingPlayerTarget();

        if (targetPlayerIndex == -1)
        {
            return;
        }


        int damage = rand.Next(3, 7);
        int hitChance = rand.Next(60, 85);

        bool strongAttack = (rand.Next(100) < 30 || playerHPs[targetPlayerIndex] <= damage + 2);

        if (strongAttack)
        {
            int strongDamage = damage + rand.Next(1, 4);
            int strongChance = hitChance + 10;
            ApplyDmgToPlayer(strongDamage, strongChance, targetPlayerIndex);
        }
        else
        {
            ApplyMassDmg(damage, hitChance);
        }
    }

    void ApplyDmgToPlayer(int dmg, int chance, int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= playerHPs.Count || playerHPs[playerIndex] <= 0)
        {
            return;
        }

        lastActionMissed = false;
        if (rand.Next(101) <= chance)
        {
                if (playerShields[playerIndex] > 0)
                {
                    if (playerShields[playerIndex] >= dmg)
                    {
                        playerShields[playerIndex] -= dmg;
                        return;
                    }
                    else
                    {
                        dmg -= playerShields[playerIndex];
                        playerShields[playerIndex] = 0;
                        playerHPs[playerIndex] -= dmg;
                        playerHPs[playerIndex] = Mathf.Max(0, playerHPs[playerIndex]);
                        OnPlayerHPChange?.Invoke(playerIndex, playerHPs[playerIndex]);
                    }

                }
                else
                {
                    playerHPs[playerIndex] -= dmg;
                    playerHPs[playerIndex] = Mathf.Max(0, playerHPs[playerIndex]);
                    OnPlayerHPChange?.Invoke(playerIndex, playerHPs[playerIndex]);
                }
                
            
        }
        else
        {
            Miss(ActionType.BossAttack);
        }
    }

    void ApplyMassDmg(int damage, int chance)
    {
        for (int i = 0; i < playerHPs.Count; i++)
        {
            ApplyDmgToPlayer(damage, chance, i);
        }
    }


    bool CheckForGameOver()
    {
        if (BossHP <= 0)
        {
            Victory();
            return true;
        }

        if (AreAllPlayersDead())
        {
            Defeat();
            return true;
        }
        return false;
    }


    bool AreAllPlayersDead()
    {
        foreach (int hp in playerHPs)
        {
            if (hp > 0)
            {
                return false;
            }
        }
        return true;
    }

    void Victory()
    {
        isPlayerTurn = false;
        OnVictory?.Invoke();
        enabled = false;
    }

    void Defeat()
    {
        isPlayerTurn = false;
        OnDefeat?.Invoke();
        enabled = false;
    }


    void Miss(ActionType action)
    {
        lastActionMissed = true;
        OnMiss?.Invoke(action);
    }

    int SelectLivingPlayerTarget()
    {
        List<int> livingPlayerIndices = new List<int>();
        for (int i = 0; i < playerHPs.Count; i++)
        {
            if (playerHPs[i] > 0)
            {
                livingPlayerIndices.Add(i);
            }
        }

        if (livingPlayerIndices.Count == 0)
        {
            return -1;
        }

        int randomIndexInList = rand.Next(livingPlayerIndices.Count);
        return livingPlayerIndices[randomIndexInList];
    }


    int SelectPlayerToHeal()
    {
        List<int> healablePlayerIndices = new List<int>();
        for (int i = 0; i < playerHPs.Count; i++)
        {
            if (playerHPs[i] > 0 && playerHPs[i] < maxPlayerHP)
            {
                healablePlayerIndices.Add(i);
            }
        }
        if (healablePlayerIndices.Count == 0) return -1;
        if (healablePlayerIndices.Contains(currentPlayerIndex)) return currentPlayerIndex;
        return healablePlayerIndices[rand.Next(healablePlayerIndices.Count)];
    }

    void AnimateAction(bool success)
    {
        if (!success || lastActionMissed)
        {

        }
        else
        {

        }
    }
}