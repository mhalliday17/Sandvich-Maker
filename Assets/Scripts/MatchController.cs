using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    private int playerScore;
    public int PlayerScore
    {
        get
        {
            return playerScore;
        }

        set
        {
            playerScore = value;
            PlayerScoreOnChange?.Invoke(value);
        }
    }
    [SerializeField] int scoreIncreaseAmount = 15;
    [SerializeField] int scoreDecreaseAmount = 10;

    private int sandwichesMadeTotalAmount;
    public int SandwichesMadeTotalAmount
    {
        get
        {
            return sandwichesMadeTotalAmount;
        }

        set
        {
            sandwichesMadeTotalAmount = value;
            SandwichMadeAmountOnChange?.Invoke(value);
        }
    }

    private bool matchOnCourse;
    public bool MatchOnCourse
    {
        get
        {
            return matchOnCourse;
        }

        set
        {
            matchOnCourse = value;
            MatchOnCourseOnChange?.Invoke(value);
        }
    }

    private bool matchCountdownOnCourse;
    public bool MatchCountdownOnCourse
    {
        get
        {
            return matchCountdownOnCourse;
        }

        set
        {
            matchCountdownOnCourse = value;
            MatchCountdownOnCourseOnChange?.Invoke(value);
        }
    }

    [SerializeField] private float matchDurationTime = 120f;
    [SerializeField] private float matchStartCountdownTime = 10f;
    [SerializeField] private float matchStartCountdownTimeInterval = 1f;

    public event Action<int> PlayerScoreOnChange;
    public event Action<int> SandwichMadeAmountOnChange;

    public event Action MatchStarted;
    public event Action<int> MatchEnded;
    public event Action<bool> MatchOnCourseOnChange;
    public event Action<float> MatchTimerOnChange;
    public event Action MatchTimerOnFinish;

    public event Action MatchCountdownStarted;
    public event Action MatchCountdownEnded;
    public event Action<bool> MatchCountdownOnCourseOnChange;
    public event Action<float> MatchTimerCountdownOnChange;
    public event Action MatchTimerCountdownOnFinish;

    Coroutine matchControllerTimerCountdown;

    private void OnEnable()
    {
        GameManager.Instance.uIMainMenuController.MatchStartDelayEnded += MatchInitialize;
        GameManager.Instance.uIMatchEndMenu.MatchRestartingDelayEnded += MatchInitialize;
        MatchTimerCountdownOnFinish += MatchStart;
        MatchTimerOnFinish += MatchEnd;
        GameManager.Instance.sandwichBuilderController.SandwichFinished += UpdatePlayerScore;
    }

    private void OnDisable()
    {
        MatchTimerCountdownOnFinish -= MatchStart;
        MatchTimerOnFinish -= MatchEnd;
        if (GameManager.Instance == null)
            return;
        GameManager.Instance.uIMainMenuController.MatchStartDelayEnded -= MatchInitialize;
        GameManager.Instance.uIMatchEndMenu.MatchRestartingDelayEnded -= MatchInitialize;
        GameManager.Instance.sandwichBuilderController.SandwichFinished -= UpdatePlayerScore;
    }

    public void MatchInitialize()
    {
        if (MatchOnCourse || MatchCountdownOnCourse) return;
        MatchCountdownStarted?.Invoke();
        MatchCountdownOnCourse = true;
        StartMatchControllerTimer(true);
    }

    public void MatchStart()
    {
        MatchStarted?.Invoke();
        MatchCountdownEnded?.Invoke();
        MatchCountdownOnCourse = false;
        MatchOnCourse = true;
        StartMatchControllerTimer(false);
        PlayerScore = 0;
        SandwichesMadeTotalAmount = 0;
    }

    private void MatchEnd()
    {
        MatchOnCourse = false;
        MatchEnded?.Invoke(playerScore);
    }

    private void StartMatchControllerTimer(bool isCountDown)
    {
        if(matchControllerTimerCountdown != null)
        {
            StopCoroutine(matchControllerTimerCountdown);
        }

        matchControllerTimerCountdown = StartCoroutine(MatchControllerTimer(isCountDown));
    }

    private IEnumerator MatchControllerTimer(bool isCountDown)
    {
        float currentCountTime = isCountDown ? matchStartCountdownTime : matchDurationTime;

        if (isCountDown)
        {
            while (currentCountTime > 0f)
            {
                MatchTimerCountdownOnChange.Invoke(currentCountTime);
                yield return new WaitForSeconds(matchStartCountdownTimeInterval);
                currentCountTime -= 1f;
                MatchTimerCountdownOnChange.Invoke(currentCountTime);
            }
            MatchTimerCountdownOnFinish?.Invoke();
        }
        else
        {
            while (currentCountTime > 0f)
            {
                MatchTimerOnChange?.Invoke(currentCountTime);
                yield return new WaitForSeconds(1f);
                currentCountTime -= 1f;
                MatchTimerOnChange?.Invoke(currentCountTime);
            }
            MatchTimerOnFinish?.Invoke();
        }
    }

    private void UpdatePlayerScore(bool isIncrease)
    {
        PlayerScore += isIncrease ? scoreIncreaseAmount : -scoreDecreaseAmount;
        if (isIncrease) IncreaseSandwichMadeTotalAmount();
    }

    private void IncreaseSandwichMadeTotalAmount()
    {
        SandwichesMadeTotalAmount++;
    }
}
