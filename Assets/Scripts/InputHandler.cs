using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler
{
    private List<(MoveCommand, KeyCode)> commands = new List<(MoveCommand, KeyCode)> ();

    public void AssignCommand(MoveCommand cmd, KeyCode k) {
        for (int i = 0; i < commands.Count; i++) {
            KeyCode key = commands[i].Item2;
            if (key == k) {
                commands[i] = (cmd, k);
                return;
            }
        }
        Debug.Log("Command Added");
        commands.Add((cmd, k));
    }

    public void InputUpdate() {
        foreach ((MoveCommand command, KeyCode key) in commands) {
            if (Input.GetKeyDown(key)) command.Execute();
        }
    }
}
