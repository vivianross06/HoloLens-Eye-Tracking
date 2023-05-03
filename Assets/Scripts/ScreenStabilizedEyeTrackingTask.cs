﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;

public class ScreenStabilizedEyeTrackingTask : MonoBehaviour
{
    public GameObject HeadStabilized;
    public GameObject countdownText;
    public GameObject taskManager;
    public string filenamePrefix;

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
    private bool isReady = false;

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

    void OnEnable()
    {
        countdownText.GetComponent<TextMesh>().text = filenamePrefix;
        countdownText.SetActive(true);
        StartEvaluation();
    }

    void OnDisable()
    {
        countdownText.SetActive(false);
        if (recording)
        {
            filename = filenamePrefix + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            GetComponent<DataLogger>().SaveFile(filename);
            recording = false;
        }
        GetComponent<Renderer>().enabled = false;
        isEvaluating = false;
        isReady = false;
        movement = "start";
        frameNumber = 0;
        edges.SetActive(false);
        currentObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (recording)
        {
            GetComponent<DataLogger>().AddFrame(frameNumber, movement);
            frameNumber++;
        }
        if (Input.GetKeyDown("q"))
        {
            StartEvaluation();
        }
        if (Input.GetKeyDown("return") && !isEvaluating)
        {
            if (isReady)
            {
                edges.SetActive(false);
                currentObject.SetActive(false);
                recording = true;
                GetComponent<AudioSource>().Play(0);
                StartCoroutine(Evaluation());
            }
        }
        if (Input.GetKeyDown("up") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += currentObject.transform.up * 0.005f;
                transform.position = start.position;
            }
        }
        if (Input.GetKeyDown("down") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position -= currentObject.transform.up * 0.005f;
                transform.position = start.position;
            }
        }
        if (Input.GetKeyDown("left") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position -= currentObject.transform.right * 0.005f;
                transform.position = start.position;
            }
        }
        if (Input.GetKeyDown("right") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += currentObject.transform.right * 0.005f;
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
        isReady = true;
    }

    IEnumerator Evaluation()
    {
        isEvaluating = true;
        end = null;
        filename = filenamePrefix + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        frameNumber = 0;
        GetComponent<DataLogger>().AddHeader();
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
            while (timeElapsed < pathTime || transform.position != end.position)
            {
                movement = "moving";
                transform.position = Vector3.Lerp(start.position, end.position, Mathf.Min(timeElapsed / pathTime, 1));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            movement = "static";
            chooseNewPath();
            yield return new WaitForSeconds(1.5f);
        }
        GetComponent<Renderer>().enabled = false;
        recording = false;
        GetComponent<DataLogger>().SaveFile(filename);
        isReady = false;
        isEvaluating = false;
        countdownText.GetComponent<TextMesh>().text = "Done";
        countdownText.SetActive(true);
        yield return new WaitForSeconds(1);
        countdownText.SetActive(false);
        taskManager.GetComponent<TaskManager>().StartNextTask();
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
}
