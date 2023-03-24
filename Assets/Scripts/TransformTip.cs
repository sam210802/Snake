using UnityEngine;

public class TransformTip : MonoBehaviour
{
    public LevelCreatorManager levelCreatorManager;

    private GameObject attatchedObject;
    public GameObject attatchedObjectProperty {
        get {
            return attatchedObject;
        } set {
            attatchedObject = value;
            gameObject.SetActive(true);
        }
    }

    void Start() {
        levelCreatorManager = GameObject.FindObjectOfType<LevelCreatorManager>();
    }

    void OnEnable() {
        // the gameobject the script is attatched to
        // set position to bottom left
        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(200, 200, 0));
        // brings object to front/foreground
        newPos.z = -1;
        this.gameObject.transform.position = newPos;
    }
}
