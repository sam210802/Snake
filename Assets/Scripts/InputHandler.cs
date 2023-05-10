using System.Collections.Generic;
using UnityEngine;

public enum Commands {UpCommand, RightCommand, DownCommand, LeftCommand, PauseCommand, RewindCommand}

public class InputHandler
{
    public static Dictionary<Commands, KeyCode> defaultKeys = new Dictionary<Commands, KeyCode> () {
        {Commands.UpCommand, KeyCode.W},
        {Commands.RightCommand, KeyCode.D},
        {Commands.DownCommand, KeyCode.S},
        {Commands.LeftCommand, KeyCode.A},
        {Commands.PauseCommand, KeyCode.Space},
        {Commands.RewindCommand, KeyCode.Return}
    };

    public static void setKey(Commands command, KeyCode key) {
        PlayerPrefs.SetString(command.ToString(), key.ToString());
    }

    public static KeyCode getKey(Commands command) {
        string key = PlayerPrefs.GetString(command.ToString(), defaultKeys[command].ToString());
        return System.Enum.Parse<KeyCode>(key);
    }

    private List<(Command, KeyCode)> commands = new List<(Command, KeyCode)> ();

    public void AssignCommand(Command cmd, KeyCode k) {
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
        foreach ((Command command, KeyCode key) in commands) {
            if (Input.GetKeyDown(key)) command.Execute(true);
            else if (Input.GetKeyUp(key)) command.Execute(false);
        }
    }
}