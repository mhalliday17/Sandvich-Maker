using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreDisplay : MonoBehaviour
{
    [SerializeField] private GameObject scoreDisplayContent;
    [SerializeField] private TextMeshProUGUI scoreDisplay;

    private void OnEnable()
    {
        if (scoreDisplay != null)
        {
            GameManager.Instance.matchController.MatchOnCourseOnChange += StartScoreDisplay;
            GameManager.Instance.matchController.PlayerScoreOnChange += UpdateScoreDisplay;
        }

        scoreDisplayContent.SetActive(false);
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.matchController.MatchOnCourseOnChange -= StartScoreDisplay;
        GameManager.Instance.matchController.PlayerScoreOnChange -= UpdateScoreDisplay;
    }

    private void StartScoreDisplay(bool matchOnCourse)
    {
        scoreDisplayContent.SetActive(matchOnCourse);
    }

    private void UpdateScoreDisplay(int newScore)
    {
        scoreDisplay.text = "Score: " + newScore.ToString();
    }
}
