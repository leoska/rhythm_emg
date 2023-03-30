using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var gameController = GameController.Instance;
        var pos = transform.position;
        pos.z -= 1f * gameController.gameNoteSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
