using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private NoteActivator _redActivator = null;
    private NoteActivator _yellowActivator = null;
    private NoteActivator _greenActivator = null;

    [Header("Score & Rock")] 
    public int score = 0;
    public int rock = 5;
    
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
    
    public static GameController Instance { get; private set; } = null;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var isActivateRed = Input.GetKeyDown(redActivatorButton);
        var isActivateYellow = Input.GetKeyDown(yellowActivatorButton);
        var isActivateGreen = Input.GetKeyDown(greenActivatorButton);

        if (isActivateRed)
        {
            _TryActivate(Notes.Red);
        } 
        else if (isActivateYellow)
        {
            _TryActivate(Notes.Yellow);
        } 
        else if (isActivateGreen)
        {
            _TryActivate(Notes.Green);
        }
    }

    private void _TryActivate(Notes noteType)
    {
        var isFail = true;
        var addScore = 100;
        var addRock = 1;
        
        switch (noteType)
        {
            case Notes.Red:
                if (_redActivator != null)
                {
                    isFail = false;
                    _redActivator.Activate();
                    _redActivator = null;
                }
                break;

            case Notes.Yellow:
                if (_yellowActivator != null)
                {
                        isFail = false;
                        _yellowActivator.Activate();
                        _yellowActivator = null;
                } 
                break;
            
            case Notes.Green:
                if (_greenActivator != null)
                {
                    isFail = false;
                    _greenActivator.Activate();
                    _greenActivator = null;
                }
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(noteType), noteType, null);
        }
        
        if (isFail)
        {
            rock -= addRock;
        }
        else
        {
            score += addScore;
            rock += addScore;
        }
    }

    public void NoteCanActivateRed(NoteActivator noteActivator)
    {
        if (_redActivator != null)
            throw new Exception("Previous Red Note Can Activate!");

        _redActivator = noteActivator;
    }
    
    public void NoteCanActivateYellow(NoteActivator noteActivator)
    {
        if (_yellowActivator != null)
            throw new Exception("Previous Red Note Can Activate!");

        _yellowActivator = noteActivator;
    }
    
    public void NoteCanActivateGreen(NoteActivator noteActivator)
    {
        if (_greenActivator != null)
            throw new Exception("Previous Red Note Can Activate!");

        _greenActivator = noteActivator;
    }

    public enum Notes
    {
        Red = 0,
        Yellow = 1,
        Green = 2,
    }
}
