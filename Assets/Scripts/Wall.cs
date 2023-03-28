using System;
using UnityEngine;

public class Wall
{
    string name;
    public string nameProperty {
        get {
            return name;
        } set {
            name = value;
            updateWall();
        }
    }
    Vector3 scale;
    public Vector3 scaleProperty {
        get {
            return scale;
        } set {
            scale = value;
            updateWall();
        }
    }

    Vector3 position;
    public Vector3 positionProperty {
        get {
            return position;
        } set {
            position = value;
            updateWall();
        }
    }

    static string wallPrefabLocation = "Wall";
    static Transform wallPrefab = (Resources.Load(wallPrefabLocation) as GameObject).transform;
    public Transform transform;

    public Wall(bool editMode = false) {
        // create transform
        transform = GameObject.Instantiate(wallPrefab);

        if (editMode) transform.gameObject.AddComponent<WallMouseManager>();

        scaleProperty = new Vector3(1, 1, 1);
        positionProperty = new Vector3(0, 0, 0);
        nameProperty = "Wall";
    }

    public Wall(WallData wallData, bool editMode = false) {
        // create transform
        transform = GameObject.Instantiate(wallPrefab);

        if (editMode) transform.gameObject.AddComponent<WallMouseManager>();

        scaleProperty = wallData.scale;
        positionProperty = wallData.position;
        nameProperty = wallData.name;
    }

    public WallData toJson() {
        return new WallData(transform.name, transform.localScale, transform.position);
    }

    void updateWall() {
        transform.name = nameProperty;
        transform.localScale = scaleProperty;
        transform.localPosition = positionProperty;
    }
}

[Serializable]
public class WallData {
    public string name;
    public Vector3 scale;
    public Vector3 position;

    public WallData(string name) {
        this.name = name;
        this.scale = new Vector3(1, 1, 1);
        this.position = new Vector3(0, 0, 0);
    }

    public WallData(string name, Vector3 scale, Vector3 position) {
        this.name = name;
        this.scale = scale;
        this.position = position;
    }
}
