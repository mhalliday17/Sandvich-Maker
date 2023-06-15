using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class UITimersDisplay : MonoBehaviour
{
    [SerializeField] private GameObject countdownTimeTextDisplayContent;
    [SerializeField] private TextMeshProUGUI countdownTimeTextDisplay;

    [SerializeField] private GameObject matchTimeTextDisplayContent;
    [SerializeField] private TextMeshProUGUI matchTimeTextDisplay;

    [SerializeField] private PlayableDirector animDirector;

    private void OnEnable()
    {
        if (countdownTimeTextDisplay != null)
        {
            GameManager.Instance.matchController.MatchCountdownOnCourseOnChange += StartCountdownDisplay;
            GameManager.Instance.matchController.MatchTimerCountdownOnChange += UpdateCountdownDisplay;
        }

        if (matchTimeTextDisplay != null)
        {
            GameManager.Instance.matchController.MatchOnCourseOnChange += StartMatchTimeDisplay;
            GameManager.Instance.matchController.MatchTimerCountdownOnChange += UpdateCountdownDisplay;
            GameManager.Instance.matchController.MatchTimerOnChange += UpdateMatchTimerDisplay;
        }

        countdownTimeTextDisplayContent.SetActive(false);
        matchTimeTextDisplayContent.SetActive(false);
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.matchController.MatchTimerCountdownOnChange -= UpdateCountdownDisplay;
        GameManager.Instance.matchController.MatchTimerOnChange -= UpdateMatchTimerDisplay;
    }

    private void StartCountdownDisplay(bool countdownOnCourse)
    {
        countdownTimeTextDisplayContent.SetActive(countdownOnCourse);
        if (countdownOnCourse) animDirector.Play();
    }

    private void StartMatchTimeDisplay(bool matchOnCourse)
    {
        matchTimeTextDisplayContent.SetActive(matchOnCourse);
    }

    private void UpdateCountdownDisplay(float currentCountdownTime)
    {
        countdownTimeTextDisplay.text = currentCountdownTime > 0 ? "Get ready in " + currentCountdownTime.ToString() : "Start!";
    }
    private void UpdateMatchTimerDisplay(float currentMatchTime)
    {
        matchTimeTextDisplay.text = "Match ends in " + currentMatchTime.ToString();
    }
}
