using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;

public class Calibration : MonoBehaviour
{
    public GameObject CalibrationObject;
    public GameObject countdownText;
    public GameObject taskManagerObject;

    private GameObject currentObject;
    private GameObject grid;
    private GameObject edges;

    private List<Transform> gridTransforms = new List<Transform>();
    private Transform end = null;
    private List<string> log = new List<string>();
    private bool recording = false;
    private string filename;
    private string movement = "start";
    private int frameNumber = 0;
    private bool isCalibrating = false;
    private bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    void OnEnable()
    {
        countdownText.GetComponent<TextMesh>().text = "Calibration";
        countdownText.SetActive(true);
    }

    void OnDisable()
    {
        countdownText.SetActive(false);
        if (recording)
        {
            filename = "calibration_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            //string filePath = Path.Combine(Application.persistentDataPath, filename);
            string filePath = Path.Combine(Application.dataPath, filename);
            Debug.Log(filePath);
            File.WriteAllLines(filePath, log);
            log.Clear();
            recording = false;
        }
        GetComponent<Renderer>().enabled = false;
        isCalibrating = false;
        isReady = false;
        movement = "start";
        frameNumber = 0;
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
            StartCalibration();
        }
        if (Input.GetKeyDown("return"))
        {
            if (isReady)
            {
                edges.SetActive(false);
                currentObject.SetActive(false);
                recording = true;
                StartCoroutine(calibration());
            }
        }
        if (Input.GetKeyDown("up") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(0, 0.03f, 0);
            }
        }
        if (Input.GetKeyDown("down") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(0, -0.03f, 0);
            }
        }
        if (Input.GetKeyDown("left") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(-0.03f, 0, 0);
            }
        }
        if (Input.GetKeyDown("right") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += new Vector3(0.03f, 0, 0);
            }
        }
    }

    public void StartCalibration()
    {
        Debug.Log("Calibration");
        isCalibrating = false;
        if (edges != null)
        {
            edges.SetActive(false);
            currentObject.SetActive(false);
        }
        currentObject = CalibrationObject;
        grid = currentObject.transform.Find("Positions").gameObject;
        edges = currentObject.transform.Find("Edges").gameObject;
        edges.SetActive(true);
        currentObject.SetActive(true);
        gridTransforms.Clear();
        foreach (Transform child in grid.transform)
        {
            gridTransforms.Add(child);
        }
        isReady = true;
    }

    IEnumerator calibration()
    {
        isReady = false;
        isCalibrating = true;
        filename = "calibration_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        frameNumber = 0;
        movement = "start";
        AddHeader();
        countdownText.SetActive(true);
        List<int> indices = new List<int>();
        for (int i = 0; i < 13; i++)
        {
            indices.Add(i);
        }
        int nextIndex = Random.Range(0, indices.Count);
        indices.Remove(nextIndex);
        end = gridTransforms[nextIndex];
        transform.position = end.position;
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
            transform.position = end.position;
            yield return new WaitForSeconds(2);
            nextIndex = indices[Random.Range(0, indices.Count)];
            indices.Remove(nextIndex);
            end = gridTransforms[nextIndex];
        }
        GetComponent<Renderer>().enabled = false;
        recording = false;
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        //string filePath = Path.Combine(Application.dataPath, filename);
        Debug.Log(filePath);
        File.WriteAllLines(filePath, log);
        log.Clear();
        frameNumber = 0;
        isCalibrating = false;
        countdownText.GetComponent<TextMesh>().text = "calibration";
        countdownText.SetActive(true);
        taskManagerObject.GetComponent<TaskManager>().StartNextTask();
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
