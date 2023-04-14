using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDisplay
{
    private string _display, _command, _original;

    public CommandDisplay(string display, string command, string original)
    {
        _display = display;
        _command = command;
        _original = original;
    }

    public string Display
    {
        get => _display;
        set => _display = value;
    }

    public string Command
    {
        get => _command;
    }

    public string Original
    {
        get => _original;
    }
}
