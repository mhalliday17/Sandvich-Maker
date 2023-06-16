using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerController : MonoBehaviour
{
    [SerializeField] private AudioSource uiAudioEffects;
    [SerializeField] private AudioSource heavyAudioEffects;
    [SerializeField] private AudioSource sandvichinatorAudioEffects;
    [SerializeField] private AudioSource miscAudioEffects;
    [SerializeField] private AudioSource musicAudio;

    [Header("UI Audio")]
    [SerializeField] private AudioClip uiButtonHoverAudio;
    [SerializeField] private AudioClip uiButtonClickAudio;

    [Header("Heavy Character Audio")]
    [SerializeField] private AudioClip heavyCheers1Audio;
    [SerializeField] private AudioClip heavyGoodJob1Audio;
    [SerializeField] private AudioClip heavyGoodJob2Audio;
    [SerializeField] private AudioClip heavyJeers1Audio;
    [SerializeField] private AudioClip heavyJeers2Audio;
    [SerializeField] private AudioClip heavyJeers3Audio;
    [SerializeField] private AudioClip heavyGoAudio;
    [SerializeField] private AudioClip heavySandwichTaunt1Audio;
    [SerializeField] private AudioClip heavySandwichTaunt2Audio;
    [SerializeField] private AudioClip heavySandwichTaunt3Audio;

    [Header("Misc Audio")]
    [SerializeField] private AudioClip applauseAudio;
    [SerializeField] private AudioClip clapSingleAudio;
    [SerializeField] private AudioClip errorAudio;
    [SerializeField] private AudioClip flameEngulfAudio;
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip drumsAudio;

    [Header("Music Audio")]
    [SerializeField] private AudioClip teamWonAudio;
    [SerializeField] private AudioClip teamLostAudio;

    private void OnEnable()
    {
        GameManager.Instance.matchController.MatchEnded += MatchEndRandomHeavyVoice;

        GameManager.Instance.sandwichBuilderController.SandwichFinished += (isGoodSandwich) =>
        {
            if (isGoodSandwich)
                GoodSandwich();
            else
                BadSandwich();

            if (RollDie(5))
                SandwichFinishedRandomHeavyVoice(isGoodSandwich);
        };
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.matchController.MatchEnded -= MatchEndRandomHeavyVoice;

        GameManager.Instance.sandwichBuilderController.SandwichFinished -= (isGoodSandwich) => 
        {
            if (isGoodSandwich)
                GoodSandwich();
            else
                BadSandwich();

            if (RollDie(5)) 
                SandwichFinishedRandomHeavyVoice(isGoodSandwich);
        };
    }

    private bool RollDie(int dieSize)
    {
        return Random.Range(0, dieSize+1) == 0;
    }

    private void SandwichFinishedRandomHeavyVoice(bool isGoodSandwich)
    {
        if (isGoodSandwich)
        {
            miscAudioEffects.clip = hitAudio;
            miscAudioEffects.Play();

            int randomAudio = Random.Range(0, 4);

            switch (randomAudio)
            {
                case 0:
                    heavyAudioEffects.clip = heavyGoAudio;
                    break;
                case 1:
                    heavyAudioEffects.clip = heavySandwichTaunt1Audio;
                    break;
                case 2:
                    heavyAudioEffects.clip = heavySandwichTaunt2Audio;
                    break;
                case 3:
                    heavyAudioEffects.clip = heavySandwichTaunt3Audio;
                    break;
                default:
                    break;
            }
        }
        else
        {
            int randomAudio = Random.Range(0, 3);

            switch (randomAudio)
            {
                case 0:
                    heavyAudioEffects.clip = heavyJeers1Audio;
                    break;
                case 1:
                    heavyAudioEffects.clip = heavyJeers2Audio;
                    break;
                case 2:
                    heavyAudioEffects.clip = heavyJeers3Audio;
                    break;
                default:
                    break;
            }
        }
        heavyAudioEffects.Play();
    }

    private void MatchEndRandomHeavyVoice(int playerScore)
    {
        if(playerScore <= 0)
        {
            SandwichFinishedRandomHeavyVoice(false);
            musicAudio.clip = teamLostAudio;
            musicAudio.Play();
            return;
        }

        musicAudio.clip = teamWonAudio;
        musicAudio.Play();

        int randomAudio = Random.Range(0, 3);

        switch (randomAudio)
        {
            case 0:
                heavyAudioEffects.clip = heavyCheers1Audio;
                break;
            case 1:
                heavyAudioEffects.clip = heavyGoodJob1Audio;
                break;
            case 2:
                heavyAudioEffects.clip = heavyGoodJob2Audio;
                break;
            default:
                break;
        }
        heavyAudioEffects.Play();
    }

    public void ButtonOnHover()
    {
        uiAudioEffects.clip = uiButtonHoverAudio;
        uiAudioEffects.Play();
    }

    public void ButtonOnClick()
    {
        uiAudioEffects.clip = uiButtonClickAudio;
        uiAudioEffects.Play();
    }

    public void GoodSandwich()
    {
        miscAudioEffects.clip = hitAudio;
        miscAudioEffects.Play();
    }

    public void BadSandwich()
    {
        miscAudioEffects.clip = errorAudio;
        miscAudioEffects.Play();
    }
}
