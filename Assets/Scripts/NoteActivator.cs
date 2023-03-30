using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteActivator : MonoBehaviour
{

    public GameController.Notes noteType = GameController.Notes.Red;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var gameController = GameController.Instance;
        
        if (other.gameObject.CompareTag("Activator Trigger"))
        {
            switch (noteType)
            {
                case GameController.Notes.Red:
                    gameController.NoteCanActivateRed(this);
                    break;
                
                case GameController.Notes.Yellow:
                    gameController.NoteCanActivateYellow(this);
                    break;
                
                case GameController.Notes.Green:
                    gameController.NoteCanActivateGreen(this);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(noteType), noteType, null);
            }
        }
        
        Debug.Log(other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Activator Trigger"))
        {
            Fail();
        }
    }

    public void Activate()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    private void Fail()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
