using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;

public class EyeTracking : MonoBehaviour
{
    public GameObject TwoDGrid;
    public GameObject ThreeDHeadStabilizedGrid;
    public GameObject ThreeDHeadPositionStabilizedGrid;
    public GameObject countdownText;

    private GameObject grid;

    private List<Transform> gridTransforms = new List<Transform>();
    private Transform start = null;
    private Transform end = null;
    private int startIndex;
    private int endIndex;
    private List<string> log = new List<string>();
    private bool recording = false;
    private string filename;

    // Start is called before the first frame update
    void Start()
    {
        StartRecording();
    }

    // Update is called once per frame
    void Update()
    {
        if (recording)
        {
            AddFrame();
        }
    }

    public void Start2DEvaluation()
    {
        grid = TwoDGrid;
        recording = true;
        StartCoroutine(Evaluation());

    }
    
    public void Start3DHeadStabilizedEvaluation()
    {
        grid = ThreeDHeadStabilizedGrid;
        recording = true;
        StartCoroutine(Evaluation());
    }

    public void Start3DHeadPositionStabilizedEvaluation()
    {
        grid = ThreeDHeadPositionStabilizedGrid;
        recording = true;
        StartCoroutine(Evaluation());
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
        gridTransforms.Clear();
        foreach (Transform child in grid.transform)
        {
            gridTransforms.Add(child);
        }
        filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        AddHeader();
        chooseNewPath();
        transform.position = start.position;
        countdownText.SetActive(true);
        GetComponent<Renderer>().enabled = true;
        for (int i = 3; i > 0; i--)
        {
            countdownText.GetComponent<TextMesh>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            for (float j = 0; j < 1; j += 0.005f)
            {
                transform.position = Vector3.Lerp(start.position, end.position, j);
                yield return null;
            }
            chooseNewPath();
        }
        GetComponent<Renderer>().enabled = false;
        recording = false;
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        //string filePath = Path.Combine(Application.dataPath, filename);
        Debug.Log(filePath);
        File.WriteAllLines(filePath, log);
        log.Clear();
    }

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

    void AddFrame()
    {
        log.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}",
                System.DateTime.Now.ToString("HHmmss"),
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
                transform.position.z
                ));
    }

    void AddHeader()
    {
        log.Clear();
        log.Add(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}, {28}",
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
                "transform.position.z"
                ));
    }
}

