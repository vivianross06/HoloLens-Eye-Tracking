using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;

public class WorldStabilized : MonoBehaviour
{
    public GameObject WorldStabilizedObject;
    public GameObject countdownText;
    public GameObject taskManager;
    public string filenamePrefix;
    public bool isBodyConstrained;

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
    //private bool worldStabilizedGridFound = false;
    private string filename;
    private string movement = "start";
    private int frameNumber = 0;
    private bool isEvaluating = false;
    private bool isReady = false;
    private string movementMode = "translate";

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
            filename = filenamePrefix + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            string filePath = Path.Combine(Application.persistentDataPath, filename);
            File.WriteAllLines(filePath, log);
            log.Clear();
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
            AddFrame();
            frameNumber++;
        }
        /*
        if (worldStabilizedGridFound)
        {
            transform.position = start.position;
            GetComponent<Renderer>().enabled = true;
        }
        */
        if (isReady)
        {
            transform.position = start.position;
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
                if (movementMode == "translate")
                {
                    currentObject.transform.position += currentObject.transform.up * 0.03f;
                }
                else if (movementMode == "x")
                {
                    currentObject.transform.Rotate(1.0f, 0f, 0f);
                }
            }
        }
        if (Input.GetKeyDown("down") && !isEvaluating)
        {
            if (currentObject != null)
            {
                if (movementMode == "translate")
                {
                    currentObject.transform.position -= currentObject.transform.up * 0.03f;
                }
                else if (movementMode == "x")
                {
                    currentObject.transform.Rotate(-1.0f, 0f, 0f);
                }
            }
        }
        if (Input.GetKeyDown("left") && !isEvaluating)
        {
            if (currentObject != null)
            {
                if (movementMode == "translate")
                {
                    currentObject.transform.position -= currentObject.transform.right * 0.03f;
                }
                else if (movementMode == "x")
                {
                    currentObject.transform.Rotate(1.0f, 0f, 0f);
                }
                else if (movementMode == "y")
                {
                    currentObject.transform.Rotate(0f, 1.0f, 0f);
                }
                else if (movementMode == "z")
                {
                    currentObject.transform.Rotate(0f, 0f, 1.0f);
                }
            }
        }
        if (Input.GetKeyDown("right") && !isEvaluating)
        {
            if (currentObject != null)
            {
                if (movementMode == "translate")
                {
                    currentObject.transform.position += currentObject.transform.right * 0.03f;
                }
                else if (movementMode == "x")
                {
                    currentObject.transform.Rotate(-1.0f, 0f, 0f);
                }
                else if (movementMode == "y")
                {
                    currentObject.transform.Rotate(0f, -1.0f, 0f);
                }
                else if (movementMode == "z")
                {
                    currentObject.transform.Rotate(0f, 0f, -1.0f);
                }
            }
        }
        if (Input.GetKeyDown("t"))
        {
            movementMode = "translate";
        }
        if (Input.GetKeyDown("y"))
        {
            movementMode = "x";
        }
        if (Input.GetKeyDown("u"))
        {
            movementMode = "y";
        }
        if (Input.GetKeyDown("i"))
        {
            movementMode = "z";
        }
    }
    public void StartEvaluation()
    {
        Debug.Log("World Stabilized");
        isEvaluating = false;
        currentObject = WorldStabilizedObject;
        if (isBodyConstrained)
        {
            currentObject.transform.localScale = new Vector3(1.1f, 0.9f, 1f);
        }
        else
        {
            currentObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
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
        isReady = true;
        transform.position = start.position;
        GetComponent<Renderer>().enabled = true;
    }
    /*
    public void FindWorldStabilizedObject()
    {
        worldStabilizedGridFound = true;
    }
    */

    IEnumerator Evaluation()
    {
        isEvaluating = true;
        isReady = false;
        end = null;
        filename = filenamePrefix + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        frameNumber = 0;
        AddHeader();
        chooseNewPath();
        transform.position = start.position;
        countdownText.SetActive(true);
        /*
        if (!worldStabilizedGridFound)
        {
            countdownText.GetComponent<TextMesh>().text = "Tracked image not found";
            yield return new WaitForSeconds(3);
            countdownText.GetComponent<TextMesh>().text = filenamePrefix;
            yield break;
        }
        */
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
        frameNumber = 0;
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
