using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class MusicController
{
    private static List<ValueTuple<float, GameController.Notes>> _notesBuffer;
    private static readonly string[] _splitFile = { "\r\n", "\r", "\n" };
    private static readonly char[] _splitLine = { ' ' };

    public static void ReadFromFile(string textAssetName)
    {
        _notesBuffer = new List<ValueTuple<float, GameController.Notes>>();
        var file = Resources.Load<TextAsset>(textAssetName);
        var lines = file.text.Split(_splitFile, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 1)
        {
            throw new Exception($"TextAsset {textAssetName} is empty!");
        }
        
        foreach (var line in lines)
        {
            var data = line.Split(_splitLine, StringSplitOptions.None);
            var time = float.Parse(data[0], CultureInfo.InvariantCulture.NumberFormat);
            
            GameController.Notes note;
            if (!GameController.Notes.TryParse(data[1], out note))
            {
                throw new Exception($"Attempted conversion of [{data[1] ?? "<null>"}] failed");
            }

            _notesBuffer.Add(new ValueTuple<float, GameController.Notes>(time, note));
        }
        Debug.Log(_notesBuffer.ToString());
    }

    public static IEnumerator<ValueTuple<float, GameController.Notes>> NextNote()
    {
        return ((IEnumerable<(float, GameController.Notes)>)_notesBuffer).GetEnumerator();
    }
}
