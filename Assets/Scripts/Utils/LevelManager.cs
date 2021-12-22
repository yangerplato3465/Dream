using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private void Awake() {
        //set up the instance
        if (instance == null) instance = this;
        else Destroy(this);
    }

    //tiles
    public List<CustomTile> tiles = new List<CustomTile>();
    public Tilemap tilemap;

    //key
    public Transform keyContainer;
    public GameObject keyPrefab;

    //lock
    public Transform lockContainer;
    public GameObject lockPrefab;

    private void Update() {
        //save level when pressing Ctrl + Z
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z)) Savelevel();
        //load level when pressing Ctrl + X
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X)) LoadLevel();
    }

    void Savelevel() {
        //get the bounds of the tilemap
        BoundsInt bounds = tilemap.cellBounds;

        //create a new leveldata
        LevelData levelData = new LevelData();

        //loop trougth the bounds of the tilemap
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {
                //get the tile on the position
                TileBase temp = tilemap.GetTile(new Vector3Int(x, y, 0));
                //find the temp tile in the custom tiles list
                CustomTile temptile = tiles.Find(t => t.tile == temp);

                //if there's a customtile associated with the tile
                if (temptile != null)
                {
                    //add the values to the leveldata
                    levelData.tiles.Add(temptile.id);
                    levelData.poses_x.Add(x);
                    levelData.poses_y.Add(y);
                }
            }
        }

        //loop through key container and save data
        foreach (Transform key in keyContainer) {
            levelData.keys.Add(key.name);
            levelData.keys_poses_x.Add(key.position.x);
            levelData.keys_poses_y.Add(key.position.y);
            levelData.keys_link_code.Add(key.GetComponent<Key>().linkCode);
        }

        //loop through key container and save data
        foreach (Transform locks in lockContainer) {
            levelData.locks.Add(locks.name);
            levelData.locks_poses_x.Add(locks.position.x);
            levelData.locks_poses_y.Add(locks.position.y);
            levelData.locks_link_code.Add(locks.GetComponent<Lock>().linkCode);
        }

        //save the data as a json
        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(Application.dataPath + "/LevelData/Level.json", json);

        //debug
        Debug.Log("Level was saved");
    }

    void LoadLevel() {
        //load the json file to a leveldata
        string json = File.ReadAllText(Application.dataPath + "/LevelData/Level.json");
        LevelData data = JsonUtility.FromJson<LevelData>(json);

        //clear the tilemap
        tilemap.ClearAllTiles();

        //place the tiles
        for (int i = 0; i < data.tiles.Count; i++) {
            tilemap.SetTile(new Vector3Int(data.poses_x[i], data.poses_y[i], 0), tiles.Find(t => t.name == data.tiles[i]).tile);
        }

        //generate key
        for (int i = 0; i < data.keys.Count; i++) {
            Instantiate(keyPrefab, new Vector3(data.keys_poses_x[i], data.keys_poses_y[i], 0), Quaternion.identity, keyContainer);
        }

        //generate lock
        for (int i = 0; i < data.locks.Count; i++) {
            Instantiate(lockPrefab, new Vector3(data.locks_poses_x[i], data.locks_poses_y[i], 0), Quaternion.identity, lockContainer);
        }

        //debug
        Debug.Log("Level was loaded");
    }
}

public class LevelData {
    //tiles data
    public List<string> tiles = new List<string>();
    public List<int> poses_x = new List<int>();
    public List<int> poses_y = new List<int>();

    //keys data
    public List<string> keys = new List<string>();
    public List<float> keys_poses_x = new List<float>();
    public List<float> keys_poses_y = new List<float>();
    public List<int> keys_link_code = new List<int>();

    //lock data
    public List<string> locks = new List<string>();
    public List<float> locks_poses_x = new List<float>();
    public List<float> locks_poses_y = new List<float>();
    public List<int> locks_link_code = new List<int>();
}