using System;
using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{

    public TextMeshPro textScore = null;
    public TextMeshPro textCombo = null;

    // Start is called before the first frame update
    void Start()
    {
        
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
    
    
}
