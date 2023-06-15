using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject content;
    
    [SerializeField] private Button startMatchButton;

    [SerializeField] private PlayableDirector animDirector;

    public event Action MatchStartDelayEnded;

    Coroutine matchInitializeDelay;

    private void Start()
    {
        SetMenuActive(true);
    }

    private void OnEnable()
    {
        if(content != null)
        {
            GameManager.Instance.matchController.MatchCountdownStarted += () => SetMenuActive(false);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.matchController.MatchCountdownStarted -= () => SetMenuActive(false);
    }

    private void SetMenuActive(bool setActive)
    {
        content.SetActive(setActive);
    }

    public void StartMatchInitializeDelay()
    {
        startMatchButton.interactable = false;
        if (matchInitializeDelay != null)
        {
            StopCoroutine(matchInitializeDelay);
        }

        matchInitializeDelay = StartCoroutine(MatchInitializeDelay());
    }

    private IEnumerator MatchInitializeDelay()
    {
        animDirector.Play();
        yield return new WaitForSeconds((float)animDirector.playableAsset.duration);
        MatchStartDelayEnded?.Invoke();
    }
}
