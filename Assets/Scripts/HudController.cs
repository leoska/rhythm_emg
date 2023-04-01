using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{

    private int _previosProgress = 0;

    [Header("3D Score HUD")]
    public TextMeshPro textScore = null;
    public TextMeshPro textCombo = null;

    [Header("3D Status HUD")]
    public TextMeshPro textNotes = null;
    public TextMeshPro progressText = null;

    [Header("Pause Panel")]
    public GameObject pausePanel = null;
    public TextMeshProUGUI pauseTitle = null;
    public Slider musicProgress = null;
    
    // Start is called before the first frame update
    void Start()
    {
        _previosProgress = 0;
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score)
    {
        if (!textScore)
            throw new Exception("textScore pointer is null");
                
        textScore.text = $"Score: {score}";
    }

    public void UpdateCombo(int combo)
    {
        if (!textCombo)
            throw new Exception("textCombo pointer is null");
        
        textCombo.text = $"Combo: x{combo}";
    }

    public void ShowPause()
    {
        pauseTitle.text = "PAUSE";
        musicProgress.value = GameController.Instance.musicTimer / GameController.MusicLength;
        pausePanel.SetActive(true);
    }

    public void ContinuePlay(int remainingTime)
    {
        pauseTitle.text = remainingTime.ToString();
    }

    public void HidePause()
    {
        pausePanel.SetActive(false);
    }

    public void UpdateNotes(int notes, int total)
    {
        textNotes.text = $"{notes}/{total}";
    }

    public void UpdateProgress(int progress)
    {
        if (_previosProgress == progress)
            return;
        
        progressText.text = $"{progress}%";
        _previosProgress = progress;
    }
}
