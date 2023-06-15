using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using System;
using UnityEngine.Timeline;

public class UIMatchEndMenu : MonoBehaviour
{
    [SerializeField] private GameObject content;

    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI sandwichesMadeText;
    [SerializeField] private Button restartMatchButton;

    [SerializeField] private PlayableDirector animDirector;
    [SerializeField] private TimelineAsset endMatchTimeline;
    [SerializeField] private TimelineAsset restartMatchTimeline;

    public event Action MatchEndingDelayEnded;
    public event Action MatchRestartingDelayEnded;

    Coroutine matchEndingDelay;
    Coroutine restartMatchDelay;

    private void Start()
    {
        content.SetActive(false);
    }

    private void OnEnable()
    {
        if (content != null)
        {
            //GameManager.Instance.matchController.MatchCountdownStarted += () => SetMenuActive(false);
            GameManager.Instance.matchController.MatchEnded += () => SetMenuActive(true);

            GameManager.Instance.matchController.PlayerScoreOnChange += UpdatePlayerScoreText;
            GameManager.Instance.matchController.SandwichMadeAmountOnChange += UpdateSandwichesMadeText;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        //GameManager.Instance.matchController.MatchStarted -= () => SetMenuActive(false);
        GameManager.Instance.matchController.MatchEnded -= () => SetMenuActive(true);

        GameManager.Instance.matchController.PlayerScoreOnChange -= UpdatePlayerScoreText;
        GameManager.Instance.matchController.SandwichMadeAmountOnChange -= UpdateSandwichesMadeText;
    }

    private void SetMenuActive(bool setActive)
    {
        //content.SetActive(setActive);
        if (setActive) StartMatchEndingDelay();
    }

    private void UpdatePlayerScoreText(int newValue)
    {
        playerScoreText.text = "Score: " + newValue;
    }

    private void UpdateSandwichesMadeText(int newValue)
    {
        sandwichesMadeText.text = "Sandwiches made: " + newValue;
    }

    public void StartMatchEndingDelay()
    {
        if (matchEndingDelay != null)
        {
            StopCoroutine(matchEndingDelay);
        }

        matchEndingDelay = StartCoroutine(MatchEndingDelay());
    }

    private IEnumerator MatchEndingDelay()
    {
        animDirector.Play(endMatchTimeline);
        yield return new WaitForSeconds((float)animDirector.playableAsset.duration);
        MatchEndingDelayEnded?.Invoke();
    }

    public void StartRestartMatchDelay()
    {
        if (restartMatchDelay != null)
        {
            StopCoroutine(restartMatchDelay);
        }

        restartMatchDelay = StartCoroutine(RestartMatchDelay());
    }

    private IEnumerator RestartMatchDelay()
    {
        animDirector.Play(restartMatchTimeline);
        yield return new WaitForSeconds((float)animDirector.playableAsset.duration);
        MatchRestartingDelayEnded?.Invoke();
    }
}
