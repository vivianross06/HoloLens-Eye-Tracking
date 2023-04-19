using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public GameObject screenTrackingSphere;
    public GameObject worldTrackingSphere;

    private static int taskIndex = 0;
    private static List<int> tasks;

    private List<int> order1 = new List<int>() { 4, 1, 2, 3, 5 };
    private List<int> order2 = new List<int>() { 1, 3, 4, 5, 2 };
    private List<int> order3 = new List<int>() { 5, 2, 3, 4, 1 };
    private List<int> order4 = new List<int>() { 2, 4, 5, 1, 3 };
    private List<int> order5 = new List<int>() { 3, 5, 1, 2, 4 };

    private bool automaticMode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            DisableEverything();
            automaticMode = !automaticMode;
            taskIndex = 0;
        }
        if (Input.GetKeyDown("1"))
        {
            if (automaticMode && (taskIndex == 0))
            {
                tasks = order1;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartCalibrationTask();
            }
        }
        if (Input.GetKeyDown("2"))
        {
            if (automaticMode && (taskIndex == 0))
            {
                tasks = order2;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartScreenStabilizedHeadConstrainedTask();
            }
        }
        if (Input.GetKeyDown("3"))
        {
            if (automaticMode && (taskIndex == 0))
            {
                tasks = order3;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartWorldStabilizedBodyConstrainedTask();
            }
        }
        if (Input.GetKeyDown("4"))
        {
            if (automaticMode && (taskIndex == 0))
            {
                tasks = order4;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartScreenStabilizedWalkingTask();
            }
        }
        if (Input.GetKeyDown("5"))
        {
            if (automaticMode && (taskIndex == 0))
            {
                tasks = order5;
                taskIndex = 0;
                StartNextTask();
            }
            else if (!automaticMode)
            {
                StartWorldStabilizedWalkingTask();
            }
        }
        
    }

    public void StartNextTask()
    {
        DisableEverything();
        if (!automaticMode || (taskIndex >= tasks.Count))
        {
            return;
        }
        if (tasks[taskIndex] == 1)
        {
            StartCalibrationTask();
        }
        if (tasks[taskIndex] == 2) {
            StartScreenStabilizedHeadConstrainedTask();
        }
        if (tasks[taskIndex] == 3)
        {
            StartWorldStabilizedBodyConstrainedTask();
        }
        if (tasks[taskIndex] == 4)
        {
            StartScreenStabilizedWalkingTask();
        }
        if (tasks[taskIndex] == 5)
        {
            StartWorldStabilizedWalkingTask();
        }
        taskIndex++;
    }

    public void StartCalibrationTask()
    {
        DisableEverything();
        screenTrackingSphere.SetActive(true);
        screenTrackingSphere.GetComponent<Calibration>().enabled = true;
    }

    public void StartScreenStabilizedHeadConstrainedTask()
    {
        DisableEverything();
        screenTrackingSphere.SetActive(true);
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().filenamePrefix = "ssHeadConstrained";
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().enabled = true;
    }

    public void StartWorldStabilizedBodyConstrainedTask()
    {
        DisableEverything();
        worldTrackingSphere.SetActive(true);
        worldTrackingSphere.GetComponent<WorldStabilized>().filenamePrefix = "wsBodyConstrained";
        worldTrackingSphere.GetComponent<WorldStabilized>().enabled = true;
    }

    public void StartScreenStabilizedWalkingTask()
    {
        DisableEverything();
        screenTrackingSphere.SetActive(true);
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().filenamePrefix = "ssWalking";
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().enabled = true;
    }

    public void StartWorldStabilizedWalkingTask()
    {
        DisableEverything();
        worldTrackingSphere.SetActive(true);
        worldTrackingSphere.GetComponent<WorldStabilized>().filenamePrefix = "wsWalking";
        worldTrackingSphere.GetComponent<WorldStabilized>().enabled = true;
    }

        public void DisableEverything()
    {
        screenTrackingSphere.GetComponent<Calibration>().enabled = false;
        screenTrackingSphere.GetComponent<ScreenStabilizedEyeTrackingTask>().enabled = false;
        screenTrackingSphere.SetActive(false);
        worldTrackingSphere.GetComponent<WorldStabilized>().enabled = false;
        worldTrackingSphere.SetActive(false);
    }
}
