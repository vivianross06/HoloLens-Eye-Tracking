using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Vuforia;

public class EyeTracking : MonoBehaviour
{
    public GameObject Calibration;
    public GameObject HeadStabilized;
    public GameObject WorldStabilized;
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
    private bool isStarting = false;
    private bool isCalibration = false;
    private bool worldStabilized = false;
    private bool worldStabilizedGridFound = false;
    private string filename;
    private string movement = "start";

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
        GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (recording)
        {
            AddFrame();
        }
        if (worldStabilized && worldStabilizedGridFound)
        {
            isStarting = true;
            transform.position = start.position;
            GetComponent<Renderer>().enabled = true;
        }
        if (isStarting)
        {
            transform.position = start.position;
        }
        if (Input.GetKeyDown("return"))
        {
            if (edges != null)
            {
                edges.SetActive(false);
                currentObject.SetActive(false);
                recording = true;
                if (isCalibration)
                {
                    StartCoroutine(calibration());
                }
                else
                {
                    StartCoroutine(Evaluation());
                }
            }
        }
        if (Input.GetKeyDown("up"))
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(0, 0.03f, 0);
            }
        }
        if (Input.GetKeyDown("down"))
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(0, -0.03f, 0);
            }
        }
        if (Input.GetKeyDown("left"))
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(-0.03f, 0, 0);
            }
        }
        if (Input.GetKeyDown("right"))
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(0.03f, 0, 0);
            }
        }
    }
    
    public void StartHeadStabilizedEvaluation()
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
        isStarting = true;
        GetComponent<Renderer>().enabled = true;
    }

    public void StartWorldStabilizedEvaluation()
    {
        Debug.Log("World Stabilized");
        worldStabilized = true;
        if (edges != null)
        {
            edges.SetActive(false);
            currentObject.SetActive(false);
        }
        currentObject = WorldStabilized;
        grid = currentObject.transform.Find("Positions").gameObject;
        edges = currentObject.transform.Find("Edges").gameObject;
        edges.SetActive(true);
        currentObject.SetActive(true);
        gridTransforms.Clear();
        foreach (Transform child in grid.transform)
        {
            gridTransforms.Add(child);
        }
        startIndex = 0;
        start = gridTransforms[startIndex];
    }

    public void StartCalibration()
    {
        Debug.Log("Calibration");
        if (edges != null)
        {
            edges.SetActive(false);
            currentObject.SetActive(false);
        }
        currentObject = Calibration;
        grid = currentObject.transform.Find("Positions").gameObject;
        edges = currentObject.transform.Find("Edges").gameObject;
        edges.SetActive(true);
        currentObject.SetActive(true);
        gridTransforms.Clear();
        foreach (Transform child in grid.transform)
        {
            gridTransforms.Add(child);
        }
        isCalibration = true;
    }

    public void FindWorldStabilizedObject()
    {
        worldStabilizedGridFound = true;
    }

    public void StartRecording()
    {
        recording = true;
        StartCoroutine(Record());
    }

    IEnumerator Record()
    {
        filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        AddHeader();
        yield return new WaitForSeconds(3);
        recording = false;
        string filePath = Path.Combine(Application.dataPath, filename);
        Debug.Log(filePath);
        File.WriteAllLines(filePath, log);
        log.Clear();
    }

    IEnumerator Evaluation()
    {
        end = null;
        filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        AddHeader();
        chooseNewPath();
        transform.position = start.position;
        countdownText.SetActive(true);
        if (worldStabilized && !worldStabilizedGridFound)
        {
            countdownText.GetComponent<TextMesh>().text = "Tracked image not found";
            yield return new WaitForSeconds(3);
            countdownText.SetActive(false);
            yield break;
        }
        for (int i = 3; i > 0; i--)
        {
            countdownText.GetComponent<TextMesh>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.SetActive(false);
        isStarting = false;
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
        worldStabilized = false;
        worldStabilizedGridFound = false;
    }

    IEnumerator calibration()
    {
        filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        AddHeader();
        countdownText.SetActive(true);
        List<int> indices = new List<int>();
        for (int i=0; i<13; i++)
        {
            indices.Add(i);
        }
        int nextIndex = Random.Range(0, indices.Count);
        indices.Remove(nextIndex);
        Transform nextPosition = gridTransforms[nextIndex];
        transform.position = nextPosition.position;
        countdownText.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.GetComponent<TextMesh>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.SetActive(false);
        movement = "static";
        GetComponent<Renderer>().enabled = true;
        while (indices.Count > 0)
        {
            Debug.Log(nextIndex);
            transform.position = nextPosition.position;
            yield return new WaitForSeconds(2);
            nextIndex = indices[Random.Range(0, indices.Count)];
            indices.Remove(nextIndex);
            nextPosition = gridTransforms[nextIndex];
        }
        GetComponent<Renderer>().enabled = false;
        recording = false;
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        //string filePath = Path.Combine(Application.dataPath, filename);
        Debug.Log(filePath);
        File.WriteAllLines(filePath, log);
        log.Clear();
        isCalibration = false;
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
    /*
    void chooseNewPath()
    {
        if (end != null)
        {
            start = end;
            startIndex = endIndex;
            while (endIndex == startIndex)
            {
                endIndex = Random.Range(0, gridTransforms.Count);
            }
            end = gridTransforms[endIndex];
        }
        else
        {
            startIndex = 0;
            endIndex = startIndex;
            while (endIndex == startIndex)
            {
                endIndex = Random.Range(0, gridTransforms.Count);
            }
            start = gridTransforms[startIndex];
            end = gridTransforms[endIndex];
        }
    }
    */
    void AddFrame()
    {
        log.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}",
                System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ffff"),
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
        log.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}, {29}",
                "Time",
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

