using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private NoteActivator _redActivator = null;
    private NoteActivator _yellowActivator = null;
    private NoteActivator _greenActivator = null;
    private IEnumerator<ValueTuple<float, Notes>> _nextNote;
    private GameState _gameState = GameState.Loading;
    private AudioSource _audioSource = null;
    private float _musicLength = 0f;
    private HudController _hudController = null;
    private int _comboNotes = 0;
    private const int MaxCombo = 8;


    [Header("Score & Rock")] 
    public int score = 0;
    public int rock = 5;
    public int combo = 1;
    public int comboNotesCount = 5;
    
    [Header("Note settings")] 
    [Range(5f, 25f)] public float gameNoteSpeed = 1f;
    public int noteCount = 0;
    public float distanceBetweenActivatorAndSpawn = 41.25f;
    public float collisionPositionActivator = 2.5f;

    [Header("Prefab Red Note")] public GameObject redNote;
    [Header("Prefab Yellow Note")] public GameObject yellowNote;
    [Header("Prefab Green Note")] public GameObject greenNote;

    [Header("Keyboard")] 
    public KeyCode redActivatorButton = KeyCode.LeftArrow;
    public KeyCode yellowActivatorButton = KeyCode.DownArrow;
    public KeyCode greenActivatorButton = KeyCode.RightArrow;
    public KeyCode restartGame = KeyCode.Alpha9;
    
    [Header("Music Time")]
    public float timer = 0.0f;
    public float musicTimer = 0.0f;
    public float gameSpeed = 1;

    [Header("Notes Parent")] public GameObject notesParent;

    [Header("Camera")] public GameObject mainCamera;

    public static GameController Instance { get; private set; } = null;
    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        
        _audioSource = GetComponent<AudioSource>();
        _hudController = GetComponent<HudController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameState = GameState.Loading;
        MusicController.ReadFromFile("Awolnation-Sail");
        
        _gameState = GameState.Start;
        
        score = 0;
        rock = 5;
        combo = 1;
        _comboNotes = 0;

        _redActivator = null;
        _yellowActivator = null;
        _greenActivator = null;
        timer = 0f;
        musicTimer = 0f;
        _musicLength = 0f;
        gameSpeed = 1;

        _nextNote = MusicController.NextNote();

        if (!_nextNote.MoveNext())
            throw new Exception("nextNote iterator is empty on start!");
        
        _hudController.UpdateScore(score);
        _hudController.UpdateCombo(combo);

        Invoke(nameof(_StartPlay), 1f);
    }

    // Coroutine for spawn next note and calculate music playing current time
    private IEnumerator _PlayTrack()
    {
        while (musicTimer < _musicLength)
        {
            musicTimer = _GetLengthMusic();

            var noteMovePerSec = gameNoteSpeed * (timer / musicTimer);
            var timerBeforeSpawn = distanceBetweenActivatorAndSpawn / noteMovePerSec;
            
            _SpawnNextNote(musicTimer, timerBeforeSpawn);

            yield return null;
        }
    }

    // Star play music track
    private void _StartPlay()
    {
        _musicLength = _audioSource.clip.length;
        _gameState = GameState.Playing;
        
        _audioSource.Play();
        StartCoroutine(_PlayTrack());
    }

    private float _GetLengthMusic()
    {
        return _audioSource.time;
    }

    // Update is called once per frame
    private void Update()
    {
        var isActivateRed = Input.GetKeyDown(redActivatorButton);
        var isActivateYellow = Input.GetKeyDown(yellowActivatorButton);
        var isActivateGreen = Input.GetKeyDown(greenActivatorButton);
        var isRestartGame = Input.GetKeyDown(restartGame);
        var isPause = Input.GetKeyDown(KeyCode.Escape);

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

        if (isRestartGame)
        {
            _RestartGame();
        }

        if (isPause)
        {
            if (_gameState == GameState.Pause)
            {
                
            }
            else if (_gameState == GameState.Playing)
            {
                gameSpeed = 0f;
                _gameState = GameState.Pause;
            }
        }

        _UpdateGameTimer();
    }

    private void _TryActivate(Notes noteType)
    {
        var isFail = true;
        const int addScore = 100;
        const int addRock = 1;
        
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
            _comboNotes = 0;
        }
        else
        {
            score += addScore;
            rock += addRock;

            if (_comboNotes >= comboNotesCount)
            {
                _comboNotes = 0;
                combo *= 2;
                _hudController.UpdateCombo(combo);
            }
            
            _hudController.UpdateScore(score);
        }
    }

    private void _UpdateGameTimer()
    {
        if (_gameState == GameState.Playing)
            timer += Time.deltaTime;
    }

    private void _SpawnNextNote(float timeTimer, float timeBeforeSpawn = 0)
    {
        if (_gameState != GameState.Playing)
            return;

        if (!(timeTimer > _nextNote.Current.Item1 - timeBeforeSpawn)) return;

        GameObject newNote = _nextNote.Current.Item2 switch
        {
            Notes.Red => Instantiate(redNote, notesParent.transform),
            Notes.Yellow => Instantiate(yellowNote, notesParent.transform),
            Notes.Green => Instantiate(greenNote, notesParent.transform),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var noteComponent = newNote.GetComponent<Note>();
        if (!noteComponent)
        {
            throw new Exception($"GameObject {newNote.ToString()} doesn't has a Note script!");
        }

        noteComponent.TimerCollisionActivator = _nextNote.Current.Item1;

        noteCount++;

        if (!_nextNote.MoveNext())
            _gameState = GameState.Ending;
    }

    private void _RestartGame()
    {
        StopCoroutine(nameof(_PlayTrack));
        _audioSource.Stop();
        
        foreach (Transform child in notesParent.transform)
        {
            Destroy(child.gameObject);
        }

        Start();
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

    private enum GameState
    {
        MainMenu = 0,
        Loading = 1,
        Start = 2,
        Playing = 3,
        Ending = 4,
        Pause = 5,
        Continue = 6,
    }
}
