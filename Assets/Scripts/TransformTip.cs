using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTip : MonoBehaviour
{
    public LevelCreatorManager levelCreatorManager;

    public GameObject attatchedObject;

    void Start() {
        levelCreatorManager = GameObject.FindObjectOfType<LevelCreatorManager>();
    }

    public void setAttatchedObject(GameObject gameObject) {
        attatchedObject = gameObject;
        // the gameobject the script is attatched to
        this.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(200, 200, 0));
        this.gameObject.transform.position = 
            new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
        this.gameObject.SetActive(true);
    }
}
