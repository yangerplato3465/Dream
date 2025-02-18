﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;
using System;
using Lean.Localization;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Camera cameras;
    public Light2D globalLight;
    public bool isEditorMode = true;
    public GameObject editorButtons;
    private void Awake() {
        //set up the instance
        if (instance == null) instance = this;
        else Destroy(this);
    }
    
    public Text levelNum;

    //player
    public Transform playerPos;
    public Transform playerMirrorPos;
    public GameObject playerPrefab;
    public GameObject playerMirrorPrefab;
    public bool isReverse = false;

    //tiles
    public List<CustomTile> tiles = new List<CustomTile>();
    public Tilemap tilemap;

    //key
    public Transform keyContainer;
    public GameObject keyPrefab;

    //lock
    public Transform lockContainer;
    public GameObject lockPrefab;

    //spike
    public Transform spikeContainer;
    public GameObject spikePrefab;

    //button
    public Transform buttonContainer;
    public GameObject buttonPrefab;

    //door
    public Transform doorContainer;
    public GameObject doorPrefab;

    //text
    public Transform textContainer;
    public GameObject textPrefab;

    //gem
    public Transform gemContainer;
    public GameObject gemPrefab;

    private void Start() {
        EventManager.AddListener(SystemEvents.LOAD_LEVEL, onLoadLevel);
        editorButtons.SetActive(isEditorMode);
        if(!isEditorMode) {
            LoadLevel(Config.CURRENT_LEVEL);
        }
    }

    private void Update() {
        //save level when pressing Ctrl + Z
        // if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z)) {
        //     if (levelNum.text == "") Debug.LogError("Please enter level number");
        //     else Savelevel();
        // }
        //load level when pressing Ctrl + L
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) {
            if (levelNum.text == "") Debug.LogError("Please enter level number");
            else LoadLevel(Int32.Parse(levelNum.text));
        }
    }

    public void Savelevel() {
        if(!isEditorMode) {
            Debug.Log("Can't save in gameplay mode");
            return;
        }
        if (levelNum.text == "") {
            Debug.LogError("Please enter level number");
            return;
        }
        //get the bounds of the tilemap
        BoundsInt bounds = tilemap.cellBounds;

        //create a new leveldata
        LevelData levelData = new LevelData();

        int level = Int32.Parse(levelNum.text);
        levelData.LevelNum = level;
        levelData.camera_size = cameras.orthographicSize;
        levelData.global_light_intensity = globalLight.intensity;

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

        //player data
        levelData.player_pos_x = playerPos.position.x;
        levelData.player_pos_y = playerPos.position.y;
        levelData.playerMirror_pos_x = playerMirrorPos.position.x;
        levelData.playerMirror_pos_y = playerMirrorPos.position.y;
        levelData.playerMirror_is_reverse = isReverse;

        //loop through key container and save data
        foreach (Transform key in keyContainer) {
            levelData.keys.Add(key.name);
            levelData.keys_poses_x.Add(key.position.x);
            levelData.keys_poses_y.Add(key.position.y);
            levelData.keys_link_code.Add(key.GetComponent<Key>().linkCode);
        }

        //loop through lock container and save data
        foreach (Transform locks in lockContainer) {
            levelData.locks.Add(locks.name);
            levelData.locks_poses_x.Add(locks.position.x);
            levelData.locks_poses_y.Add(locks.position.y);
            levelData.locks_link_code.Add(locks.GetComponent<Lock>().linkCode);
        }

        //loop through spike container and save data
        foreach (Transform spike in spikeContainer) {
            Spikes spikeComponent = spike.GetComponent<Spikes>();
            levelData.spikes.Add(spike.name);
            levelData.spikes_poses_x.Add(spikeComponent.startPoint.x);
            levelData.spikes_poses_y.Add(spikeComponent.startPoint.y);
            levelData.spikes_end_poses_x.Add(spikeComponent.endPoint.x);
            levelData.spikes_end_poses_y.Add(spikeComponent.endPoint.y);
            levelData.spikes_isMoving.Add(spikeComponent.isMoving);
            levelData.spikes_time.Add(spikeComponent.time);
            levelData.spikes_delay_time.Add(spikeComponent.delayTime);
        }

        //loop through button container and save data
        foreach (Transform button in buttonContainer) {
            levelData.buttons.Add(button.name);
            levelData.buttons_poses_x.Add(button.position.x);
            levelData.buttons_poses_y.Add(button.position.y);
            levelData.buttons_link_code.Add(button.GetComponent<DoubleButton>().linkCode);
        }

        //loop through door container and save data
        foreach (Transform door in doorContainer) {
            levelData.doors.Add(door.name);
            levelData.doors_poses_x.Add(door.position.x);
            levelData.doors_poses_y.Add(door.position.y);
            levelData.doors_link_code.Add(door.GetComponent<Door>().linkCode);
        }

        //loop through text container and save data
        foreach (Transform text in textContainer) {
            levelData.texts.Add(text.name);
            levelData.texts_poses_x.Add(text.position.x);
            levelData.texts_poses_y.Add(text.position.y);
            levelData.texts_start_time.Add(text.GetComponent<FadingText>().startTime);
            levelData.texts_appear_time.Add(text.GetComponent<FadingText>().appearTime);
            levelData.texts_duration_time.Add(text.GetComponent<FadingText>().duration);
            levelData.texts_id.Add(text.GetComponentInChildren<Text>().text);
        }

        //loop through gem container and save data
        foreach (Transform gem in gemContainer) {
            levelData.gems.Add(gem.name);
            levelData.gems_poses_x.Add(gem.position.x);
            levelData.gems_poses_y.Add(gem.position.y);
        }

        //save the data as a json
        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(Application.dataPath + "/Resources/Levels/Level" + level + ".json", json);
        //debug
        Debug.Log("Level was saved to Level" + level + ".json");
    }

    public void LoadLevel(int level) {
        // set current level to game manager
        Debug.Log("in LoadLevel level  = " + level);
        EventManager.TriggerEvent(SystemEvents.SET_LEVEL, level);
        //load the json file to a leveldata
        // string json = File.ReadAllText(Application.persistentDataPath + "/Level" + level + ".json");
        // var jsonTextFile = Resources.Load("Levels/Level" + level + ".json");
        UnityEngine.Object[] maps = Resources.LoadAll("Levels");
        
        // string jsonTextFile = maps[level - 1].ToString();
        string jsonTextFile = Array.Find(maps, item => item.name == "Level" + level).ToString();
        // foreach (UnityEngine.Object item in maps){
        //     Debug.Log("name " + item.name);
        // }
        Config.LEVEL_COUNT = maps.Length;
        LevelData data = JsonUtility.FromJson<LevelData>(jsonTextFile.ToString());

        //clear the tilemap
        tilemap.ClearAllTiles();
        ClearAllObject();

        // set camera size
        cameras.orthographicSize = data.camera_size;

        //place the tiles
        for (int i = 0; i < data.tiles.Count; i++) {
            tilemap.SetTile(new Vector3Int(data.poses_x[i], data.poses_y[i], 0), tiles.Find(t => t.name == data.tiles[i]).tile);
        }

        //generate key
        for (int i = 0; i < data.keys.Count; i++) {
            GameObject key = Instantiate(keyPrefab, new Vector3(data.keys_poses_x[i], data.keys_poses_y[i], 0), Quaternion.identity, keyContainer);
            key.GetComponent<Key>().linkCode = data.keys_link_code[i];
        }

        //generate lock
        for (int i = 0; i < data.locks.Count; i++) {
            GameObject locks = Instantiate(lockPrefab, new Vector3(data.locks_poses_x[i], data.locks_poses_y[i], 0), Quaternion.identity, lockContainer);
            locks.GetComponent<Lock>().linkCode = data.locks_link_code[i];
        }

        //generate spikes
        for (int i = 0; i < data.spikes.Count; i++) {
            GameObject spike = Instantiate(spikePrefab, new Vector3(data.spikes_poses_x[i], data.spikes_poses_y[i], 0), Quaternion.identity, spikeContainer);
            spike.GetComponent<Spikes>().startPoint = new Vector3(data.spikes_poses_x[i], data.spikes_poses_y[i], 0);
            spike.GetComponent<Spikes>().endPoint = new Vector3(data.spikes_end_poses_x[i], data.spikes_end_poses_y[i], 0);
            spike.GetComponent<Spikes>().isMoving = data.spikes_isMoving[i];
            spike.GetComponent<Spikes>().time = data.spikes_time[i];
            spike.GetComponent<Spikes>().delayTime = data.spikes_delay_time[i];
        }

        //generate buttons
        for (int i = 0; i < data.buttons.Count; i++) {
            GameObject button = Instantiate(buttonPrefab, new Vector3(data.buttons_poses_x[i], data.buttons_poses_y[i], 0), Quaternion.identity, buttonContainer);
            button.GetComponent<DoubleButton>().linkCode = data.buttons_link_code[i];
        }

        //generate door
        for (int i = 0; i < data.doors.Count; i++) {
            GameObject door = Instantiate(doorPrefab, new Vector3(data.doors_poses_x[i], data.doors_poses_y[i], 0), Quaternion.identity, doorContainer);
            door.GetComponent<Door>().linkCode = data.doors_link_code[i];
        }

        //generate text
        for (int i = 0; i < data.texts.Count; i++) {
            GameObject text = Instantiate(textPrefab, new Vector3(data.texts_poses_x[i], data.texts_poses_y[i], 0), Quaternion.identity, textContainer);
            text.GetComponent<FadingText>().startTime = data.texts_start_time[i];
            text.GetComponent<FadingText>().appearTime = data.texts_appear_time[i];
            text.GetComponent<FadingText>().duration = data.texts_duration_time[i];
            text.GetComponentInChildren<Text>().text = LeanLocalization.GetTranslationText(data.texts_id[i]);
        }

        //generate gem
        for (int i = 0; i < data.gems.Count; i++) {
            Instantiate(gemPrefab, new Vector3(data.gems_poses_x[i], data.gems_poses_y[i], 0), Quaternion.identity, gemContainer);
        }

        Instantiate(playerPrefab, new Vector3(data.player_pos_x, data.player_pos_y, 0), Quaternion.identity, playerPos);
        GameObject playerMirror = Instantiate(playerMirrorPrefab, new Vector3(data.playerMirror_pos_x, data.playerMirror_pos_y, 0), Quaternion.identity, playerMirrorPos);
        playerMirror.GetComponent<PlayerMirrorMovement>().isReverse = data.playerMirror_is_reverse;

        //debug
        Debug.Log("Level " + level + " was loaded");
    }

    private void ClearAllObject() {
        EventManager.TriggerEvent(SystemEvents.DESTROY_FOR_LOADING);
    }

    private void onLoadLevel(object sender) {
        Debug.Log("load level========" + (int)sender);
        LoadLevel((int)sender);
    }

    private void OnDestroy() {
        EventManager.RemoveListener(SystemEvents.LOAD_LEVEL, onLoadLevel);
    }
}

public class LevelData {
    public int LevelNum;
    public float camera_size;
    public float global_light_intensity;
    //tiles data
    public List<string> tiles = new List<string>();
    public List<int> poses_x = new List<int>();
    public List<int> poses_y = new List<int>();

    //player data
    public float player_pos_x;
    public float player_pos_y;
    public float playerMirror_pos_x;
    public float playerMirror_pos_y;
    public bool playerMirror_is_reverse;

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

    //spike data
    public List<string> spikes = new List<string>();
    public List<float> spikes_poses_x = new List<float>();
    public List<float> spikes_poses_y = new List<float>();
    public List<float> spikes_end_poses_x = new List<float>();
    public List<float> spikes_end_poses_y = new List<float>();
    public List<bool> spikes_isMoving = new List<bool>();
    public List<float> spikes_time = new List<float>();
    public List<float> spikes_delay_time = new List<float>();

    //button data
    public List<string> buttons = new List<string>();
    public List<float> buttons_poses_x = new List<float>();
    public List<float> buttons_poses_y = new List<float>();
    public List<int> buttons_link_code = new List<int>();

    //door data
    public List<string> doors = new List<string>();
    public List<float> doors_poses_x = new List<float>();
    public List<float> doors_poses_y = new List<float>();
    public List<int> doors_link_code = new List<int>();

    //text data
    public List<string> texts = new List<string>();
    public List<float> texts_poses_x = new List<float>();
    public List<float> texts_poses_y = new List<float>();
    public List<float> texts_start_time = new List<float>();
    public List<float> texts_appear_time = new List<float>();
    public List<float> texts_duration_time = new List<float>();
    public List<string> texts_id = new List<string>();

    //gem data
    public List<string> gems = new List<string>();
    public List<float> gems_poses_x = new List<float>();
    public List<float> gems_poses_y = new List<float>();

    //gem data for level button
    public List<string> level_name = new List<string>();
    public List<int> gem_count = new List<int>();
}