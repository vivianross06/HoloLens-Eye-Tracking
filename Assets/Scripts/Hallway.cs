using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;

public class Hallway : MonoBehaviour
{
    public GameObject TrackingPositions;
    public GameObject taskManager;
    public GameObject countdownText;

    private List<Transform> gridTransforms = new List<Transform>();
    private Transform start = null;
    private Transform end = null;
    private List<string> log = new List<string>();
    private bool recording = false;
    private string filename;
    private string movement = "start";
    private int frameNumber = 0;
    private float pathTime =5f;

    private float minDistance = 1.5f;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        gridTransforms.Clear();
        foreach (Transform child in TrackingPositions.transform)
        {
            gridTransforms.Add(child);
        }
        TrackingPositions.SetActive(true);
    }

    void OnEnable()
    {
        countdownText.GetComponent<TextMesh>().text = "hallway";
        countdownText.SetActive(true);
        TrackingPositions.SetActive(true);
    }

    void OnDisable()
    {
        countdownText.SetActive(false);
        if (recording)
        {
            filename = "hallway_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            string filePath = Path.Combine(Application.persistentDataPath, filename);
            File.WriteAllLines(filePath, log);
            log.Clear();
            recording = false;
        }
        GetComponent<Renderer>().enabled = false;
        movement = "start";
        frameNumber = 0;
        TrackingPositions.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (recording)
        {
            AddFrame();
            frameNumber++;
        }
        if (Input.GetKeyDown("return"))
        {
            StartCoroutine(Evaluation());
        }
        if (Input.GetKeyDown("p"))
        {
            TrackingPositions.SetActive(!TrackingPositions.activeSelf);
        }
    }

    IEnumerator Evaluation()
    {
        filename = "hallway_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        frameNumber = 0;
        AddHeader();
        recording = true;
        currentIndex = 0;
        start = gridTransforms[currentIndex];
        end = gridTransforms[currentIndex + 1];
        TrackingPositions.SetActive(false);
        transform.position = start.position;
        GetComponent<Renderer>().enabled = true;
        countdownText.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.GetComponent<TextMesh>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.SetActive(false);
        while (currentIndex < gridTransforms.Count-1)
        {
            Debug.Log(currentIndex);
            start = gridTransforms[currentIndex];
            end = gridTransforms[currentIndex + 1];
            float timeElapsed = 0.0f;
            while (timeElapsed < pathTime)
            {
                movement = "moving";
                transform.position = Vector3.Lerp(start.position, end.position, timeElapsed / pathTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            movement = "static";
            while (Vector3.Distance(transform.position, Camera.main.transform.position) > minDistance)
            {
                yield return null;
            }
            currentIndex++;
        }
        GetComponent<Renderer>().enabled = false;
        recording = false;
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        //string filePath = Path.Combine(Application.dataPath, filename);
        Debug.Log(filePath);
        File.WriteAllLines(filePath, log);
        log.Clear();
        frameNumber = 0;
        TrackingPositions.SetActive(true);
        taskManager.GetComponent<TaskManager>().StartNextTask();
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
