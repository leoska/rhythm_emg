using System;
using UnityEngine;

public class Note : MonoBehaviour
{
    private GameController _gameController = GameController.Instance;
    private const float Tolerance = 0.05f;
    
    public float TimerCollisionActivator { private get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        var pos = transform.position;
        var remainingTime = TimerCollisionActivator - _gameController.musicTimer;
        
        var move = _gameController.gameNoteSpeed;
        
        if (remainingTime > 0)
        {
            var remainingDistance = pos.z - _gameController.collisionPositionActivator;
            var noteMovePerSec = _gameController.gameNoteSpeed * (_gameController.timer / _gameController.musicTimer);
            var distanceCoeff = remainingDistance / noteMovePerSec;

            var diffCoeff = distanceCoeff - remainingTime;
            if (Math.Abs(diffCoeff) > Tolerance)
            {
                move += noteMovePerSec * diffCoeff;
            }
        }

        pos.z -= move * Time.deltaTime * _gameController.gameSpeed;

        transform.position = pos;
    }
}
