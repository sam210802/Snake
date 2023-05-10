using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonRemap : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    Commands command = Commands.UpCommand;

    bool remapKey = false;

    void OnEnable() {
        string key = InputHandler.getKey(command).ToString();
        button.GetComponentInChildren<TMP_Text>().text = key;
    }

    public void remapComand() {
        remapKey = true;        
    }

    void Update() {
        if (remapKey) { 
            // lock cursor
            Cursor.lockState = CursorLockMode.Locked;       
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKey(key)) {
                    // don't assign key if key is escape
                    if (key != KeyCode.Escape) {
                        // bind command to this key
                        InputHandler.setKey(command, key);
                        button.GetComponentInChildren<TMP_Text>().text = key.ToString();
                    }
                    remapKey = false;
                    Cursor.lockState = CursorLockMode.None;
                    return;
                }
            }
        }
    }
}
