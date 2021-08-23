using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class GameController : MonoBehaviour
{
    static private GameController S;

    [Header("Set in Inspector")]
    public Text tLevel;
    public Text tShots;
    public Text tButton;
    public Vector2 castlePos;
    public GameObject cTPos;
    public GameObject[] castles;

    [Header("Set Dinamicaly")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    // Start is called before the first frame update
    void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void StartLevel()
    {
        // так как создаем НОВЫЙ уровень
        // хана старому замку
        if (castle != null) Destroy(castle);
        // туда же снаряды
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        // сторим новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // камеру на старт
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        UpdateGUI();
        mode = GameMode.playing;
    }

    private void UpdateGUI()
    {
        tLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        tShots.text = "Shots taken: " + shotsTaken;
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "") eView = tButton.text;
        showing = eView;

        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                tButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.cTPos;
                tButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                tButton.text = "Show Slingshot";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax) level = 0;
        StartLevel();
    }

    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
