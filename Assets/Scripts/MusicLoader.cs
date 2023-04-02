using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class MusicController
{
    private static List<ValueTuple<float, GameController.Notes>> _notesBuffer = null;
    private static readonly string[] SplitFile = { "\r\n", "\r", "\n" };
    private static readonly string[] SplitLine = { "\t", " " };

    public static void ReadFromFile(string textAssetName)
    {
        _notesBuffer = new List<ValueTuple<float, GameController.Notes>>();
        var file = Resources.Load<TextAsset>(textAssetName);
        var lines = file.text.Split(SplitFile, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 1)
        {
            throw new Exception($"TextAsset {textAssetName} is empty!");
        }
        
        foreach (var line in lines)
        {
            var data = line.Split(SplitLine, StringSplitOptions.RemoveEmptyEntries);
            var time = float.Parse(data[0], CultureInfo.InvariantCulture.NumberFormat);
            
            GameController.Notes note;
            if (!Enum.TryParse(data[1], out note))
            {
                throw new Exception($"Attempted conversion of [{data[1] ?? "<null>"}] failed");
            }

            _notesBuffer.Add(new ValueTuple<float, GameController.Notes>(time, note));
        }
    }

    public static IEnumerator<ValueTuple<float, GameController.Notes>> NextNote()
    {
        return ((IEnumerable<(float, GameController.Notes)>)_notesBuffer).GetEnumerator();
    }

    public static int NoteLength()
    {
        if (_notesBuffer == null)
        {
            throw new NullReferenceException("noteBuffer is null!");
        }

        return _notesBuffer.Count;
    }

    public static void Dispose()
    {
        _notesBuffer.Clear();
        _notesBuffer = null;
    }
}
