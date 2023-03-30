using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    private static GameController _gameController = null;
    
    [Header("Note speed")] 
    [Range(5f, 25f)]
    public float gameNoteSpeed = 1f;
    
    [Header("Prefab Green Note")] public GameObject greenNote;
    [Header("Prefab Yellow Note")] public GameObject yellowNote;
    [Header("Prefab Red Note")] public GameObject redNote;

    [Header("Keyboard")] 
    public KeyCode redActivatorButton = KeyCode.Z;
    public KeyCode yellowActivatorButton = KeyCode.X;
    public KeyCode greenActivatorButton = KeyCode.C;
    
    public static GameController Instance 
    {
        get => _gameController;
        private set => _gameController = value;
    }


    private void Awake()
    {
        _gameController = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
