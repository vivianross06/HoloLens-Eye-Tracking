﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit;

public class ScreenStabilizedEyeTrackingTask : MonoBehaviour
{
    public GameObject HeadStabilized;
    public GameObject countdownText;

    private GameObject currentObject;
    private GameObject grid;
    private GameObject edges;

    private List<Transform> gridTransforms = new List<Transform>();
    private Transform start = null;
    private Transform end = null;
    private int startIndex;
    private int endIndex;
    private List<string> log = new List<string>();
    private bool recording = false;
    private string filename;
    private string movement = "start";
    private int frameNumber;
    private bool isEvaluating = false;

    private float pathTime = 5f;

    private int[] nextPos = {
                    24,19, 0,23,20,
                    9, 0, 0, 0,21,
                     0, 0, 0, 0, 0,
                     3, 0, 0, 0,15,
                     1, 2, 0, 5, 4};

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (recording)
        {
            AddFrame();
            frameNumber++;
        }
        if (Input.GetKeyDown("q"))
        {
            StartEvaluation();
        }
        if (Input.GetKeyDown("x"))
        {
            SceneManager.LoadScene("StartingScene");
        }
        if (Input.GetKeyDown("return"))
        {
            if (edges != null)
            {
                edges.SetActive(false);
                currentObject.SetActive(false);
                recording = true;
                StartCoroutine(Evaluation());
            }
        }
        if (Input.GetKeyDown("up") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(0, 0.03f, 0);
                transform.position = start.position;
            }
        }
        if (Input.GetKeyDown("down") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(0, -0.03f, 0);
                transform.position = start.position;
            }
        }
        if (Input.GetKeyDown("left") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(-0.03f, 0, 0);
                transform.position = start.position;
            }
        }
        if (Input.GetKeyDown("right") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(0.03f, 0, 0);
                transform.position = start.position;
            }
        }
    }

    public void StartEvaluation()
    {
        Debug.Log("Head Stabilized");
        if (edges != null)
        {
            edges.SetActive(false);
            currentObject.SetActive(false);
        }
        currentObject = HeadStabilized;
        grid = HeadStabilized.transform.Find("Positions").gameObject;
        edges = HeadStabilized.transform.Find("Edges").gameObject;
        edges.SetActive(true);
        currentObject.SetActive(true);
        gridTransforms.Clear();
        foreach (Transform child in grid.transform)
        {
            gridTransforms.Add(child);
        }
        startIndex = 0;
        start = gridTransforms[startIndex];
        transform.position = start.position;
        GetComponent<Renderer>().enabled = true;
        isEvaluating = false;
    }

    IEnumerator Evaluation()
    {
        isEvaluating = true;
        end = null;
        filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        frameNumber = 0;
        AddHeader();
        chooseNewPath();
        transform.position = start.position;
        countdownText.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.GetComponent<TextMesh>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.SetActive(false);
        for (int i = 0; i < 12; i++)
        {
            float timeElapsed = 0.0f;
            while (timeElapsed < pathTime)
            {
                movement = "moving";
                transform.position = Vector3.Lerp(start.position, end.position, timeElapsed / pathTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            movement = "static";
            chooseNewPath();
            yield return new WaitForSeconds(1.5f);
        }
        GetComponent<Renderer>().enabled = false;
        recording = false;
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        //string filePath = Path.Combine(Application.dataPath, filename);
        Debug.Log(filePath);
        File.WriteAllLines(filePath, log);
        log.Clear();
        SceneManager.LoadScene("StartingScene");
    }

    void chooseNewPath()
    {
        if (end != null)
        {
            start = end;
            startIndex = endIndex;
            endIndex = nextPos[startIndex];
            end = gridTransforms[endIndex];
        }
        else
        {
            startIndex = 0;
            endIndex = nextPos[startIndex];
            start = gridTransforms[startIndex];
            end = gridTransforms[endIndex];
        }
    }
   
    void AddFrame()
    {
        log.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}",
                System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ffff"),
                frameNumber,
                CoreServices.InputSystem.EyeGazeProvider.IsEyeTrackingEnabled,
                CoreServices.InputSystem.EyeGazeProvider.IsEyeCalibrationValid,
                CoreServices.InputSystem.EyeGazeProvider.IsEyeTrackingEnabledAndValid,
                CoreServices.InputSystem.EyeGazeProvider.IsEyeTrackingDataValid,
                CoreServices.InputSystem.EyeGazeProvider.GazeOrigin.x,
                CoreServices.InputSystem.EyeGazeProvider.GazeOrigin.y,
                CoreServices.InputSystem.EyeGazeProvider.GazeOrigin.z,
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.x,
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.y,
                CoreServices.InputSystem.EyeGazeProvider.GazeDirection.z,
                CoreServices.InputSystem.EyeGazeProvider.HitInfo.point.x,
                CoreServices.InputSystem.EyeGazeProvider.HitInfo.point.y,
                CoreServices.InputSystem.EyeGazeProvider.HitInfo.point.z,
                CoreServices.InputSystem.EyeGazeProvider.HitPosition.x,
                CoreServices.InputSystem.EyeGazeProvider.HitPosition.y,
                CoreServices.InputSystem.EyeGazeProvider.HitPosition.z,
                CoreServices.InputSystem.EyeGazeProvider.HitNormal.x,
                CoreServices.InputSystem.EyeGazeProvider.HitNormal.y,
                CoreServices.InputSystem.EyeGazeProvider.HitNormal.z,
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z,
                Camera.main.transform.rotation.x,
                Camera.main.transform.rotation.y,
                Camera.main.transform.rotation.z,
                transform.position.x,
                transform.position.y,
                transform.position.z,
                movement
                ));
    }

    void AddHeader()
    {
        log.Clear();
        log.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}, {30}",
                "Time",
                "Frame",
                "EyeTrackingEnabled",
                "EyeCalibrationValid",
                "EyeTrackingEnabledAndValid",
                "EyeTrackingDataValid",
                "EyeGazeOrigin.x",
                "EyeGazeOrigin.y",
                "EyeGazeOrigin.z",
                "EyeGazeDirection.x",
                "EyeGazeDirection.y",
                "EyeGazeDirection.z",
                "EyeHitInfo.point.x",
                "EyeHitInfo.point.y",
                "EyeHitInfo.point.z",
                "EyeHitPosition.x",
                "EyeHitPosition.y",
                "EyeHitPosition.z",
                "EyeHitNormal.x",
                "EyeHitNormal.y",
                "EyeHitNormal.z",
                "HeadGazeOrigin.x",
                "HeadGazeOrigin.y",
                "HeadGazeOrigin.z",
                "HeadGazeDirection.x",
                "HeadGazeDirection.y",
                "HeadGazeDirection.z",
                "transform.position.x",
                "transform.position.y",
                "transform.position.z",
                "Movement"
                ));
    }
}