using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using SWS;
using System.IO;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class PathGenerator : MonoBehaviour                                     //This script is responsible for generating waypoints for PEDs and generating PEDS, based on the external waypoint data set
{
    public Transform path_seedPosition;
    public GameObject[] pedPrefabs;
    public GameObject pathPrefab;
    string walkerName = "Ped";
    string pathName = "Custom_Path";
    public bool drawAllPaths;
    public List<bool> pathTextFile;

    private string[] textFilePaths = { "D:/Noot_Noot/n_Unity_projects/Pedestrian Behavior Toolkit/Assets/Resources/0222-path1.txt",
                                       "D:/Noot_Noot/n_Unity_projects/Pedestrian Behavior Toolkit/Assets/Resources/0222-path2.txt",
                                       "D:/Noot_Noot/n_Unity_projects/Pedestrian Behavior Toolkit/Assets/Resources/0222-path3.txt",
                                       "D:/Noot_Noot/n_Unity_projects/Pedestrian Behavior Toolkit/Assets/Resources/0222-path4.txt",
                                       "D:/Noot_Noot/n_Unity_projects/Pedestrian Behavior Toolkit/Assets/Resources/0222-path5.txt",
                                       "D:/Noot_Noot/n_Unity_projects/Pedestrian Behavior Toolkit/Assets/Resources/0222-path6.txt"
                                      };
   
    public List<FsmTemplate> pedFSM_templatePool;
    public List<bool> pedFSM_templatesFlagList;
    private FsmTemplate pedFSM_templateToAttach;
    public Transform t_WaypointMgr, t_GeneratedPeds;

    List<Vector3>[] waypoint_groups;                    //array of lists        
    //[SerializeField]
    private Transform[] waypoint_transforms;
    private int AllowednumberOfPaths;
    private static int pathID, numberOfWaypointsPerPath;
    private bool[] pathFlags;

    //readTextFile("D:/nikhil/Unity_IntersectionSimulation_Toolkit/Assets/Resources/20-x-mar-5-z-mar-2.txt");    //input file field     

    // Use this for initialization
    void Start()
    {
        int pathFileIndex = 0;
        int pedFSM_templateIndex = 0;

        foreach (bool path in pathTextFile)
        {
            if (path)
            {
                numberOfWaypointsPerPath = readTextFile(textFilePaths[pathFileIndex]);    //input file field
                waypoint_transforms = new Transform[numberOfWaypointsPerPath];
                break;
            }
            pathFileIndex++;
        }


        foreach (bool pedFSM_templateFlag in pedFSM_templatesFlagList)
        {
            if (pedFSM_templateFlag)
            {
                pedFSM_templateToAttach = pedFSM_templatePool[pedFSM_templateIndex];
                break;
            }
            pedFSM_templateIndex++;
        }


        pathID = 0;
        pathFlags = new bool[AllowednumberOfPaths];
        for (int i = 0; i < AllowednumberOfPaths; i++)
        {
            pathFlags[i] = false;
        }
        //drawAllPaths = false;
    }


    public List<Vector3>[] getWaypoints()           //method to get the array of  list type Vector3, containing the transform positons of the waypoints of each path
    {
        return waypoint_groups;
    }

    public void DrawCustomPathAndGeneratePed()
    {
        if (!drawAllPaths)
        {
            pathID = Random.Range(0, AllowednumberOfPaths);
        }

        if (!pathFlags[pathID])
        {
            //Instantiate path
            GameObject path = new GameObject();
            path.name = "Custom_Path" + pathID.ToString();
            path.transform.SetParent(t_WaypointMgr);

            //Initialize waypoints
            //print(waypoint_groups[0][0]);
            for (int i = 0; i < numberOfWaypointsPerPath; i++)
            {
                GameObject temp = new GameObject();
                temp.name = "Waypoint" + i.ToString();
                temp.transform.position = new Vector3(waypoint_groups[pathID][i].x, waypoint_groups[pathID][i].y, waypoint_groups[pathID][i].z);
                temp.transform.SetParent(path.transform);
                waypoint_transforms[i] = temp.transform;

                //if (i == 0)
                //{
                //    GameObject temp1 = new GameObject();
                //    temp1.name = "Waypoint1";
                //    temp1.transform.position = new Vector3(waypoint_groups[pathID][i].x - 40f, waypoint_groups[pathID][i].y - 1.7f, waypoint_groups[pathID][i].z + 25.0f);
                //    temp1.transform.SetParent(path.transform);
                //    waypoint_transforms[1] = temp1.transform;
                //    i++;
                //}


            }

            //instantiate walker prefab
            GameObject walker = (GameObject)Instantiate(pedPrefabs[(int)Random.Range(0, pedPrefabs.Length)], path_seedPosition.position, Quaternion.identity);
            walker.name = walkerName + pathID.ToString();
            walker.transform.SetParent(t_GeneratedPeds);
            PlayMakerFSM FSM_temp = walker.gameObject.AddComponent<PlayMakerFSM>();

            //Add the Path Manager component to the Initialized path
            //PathManager pathManager_component = path.AddComponent<PathManager>();
            //pathManager_component.waypoints = waypoint_transforms;
            //pathManager_component.drawCurved = false;
            //pathManager_component.skipCustomNames = false;

            //start movement on the new path
            //WaypointManager.AddPath(path);        //Done by PathManager script
            walker.GetComponent<splineMove>().SetPath(SWS.WaypointManager.Paths[path.name]);        //assign new path to the instantiated PED
            FSM_temp.SetFsmTemplate(pedFSM_templateToAttach);

            pathFlags[pathID] = true;           //set the type of Paths generated flag to true, to avoid generating the same path again           

            if (drawAllPaths)
            {
                pathID++;
            }
        }
    }

    public void DrawAllCustomPathsAndGenerateAllPeds()
    {

        if (drawAllPaths)
        {
            StartCoroutine("DelayInstantiate");             //Start a co routine to delay PED spawning as path manager component takes slightly more time to instantiate and attach to every PED that is spawned
        }

        else
        {
            DrawCustomPathAndGeneratePed();
        }

    }

    public IEnumerator DelayInstantiate()
    {
        for (int j = 0; j < AllowednumberOfPaths; j++)
        {
            DrawCustomPathAndGeneratePed();
            yield return new WaitForSeconds(0.0f);
        }
    }

    int readTextFile(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path);
        string inp_ln = "";
        string inp_text = "";
        List<string> xPos = new List<string>();
        List<string> zPos = new List<string>();
        List<string> xPos_temp = new List<string>();
        List<string> zPos_temp = new List<string>();
        int numberOfPointsPerPath = 0;

        while (!inp_stm.EndOfStream)
        {
            inp_ln = inp_stm.ReadLine();
            // Do Something with the input.
            //print(inp_ln);
            if (inp_ln.Contains(","))
                numberOfPointsPerPath++;

            if (inp_ln.Contains("s"))
            {
                numberOfPointsPerPath++;
                continue;
            }


            inp_text = inp_ln;
            inp_text = inp_text.Replace("[", "");
            inp_text = inp_text.Replace("_", "");
            inp_text = inp_text.Replace(" ", "");
            inp_text = inp_text.Replace("]", "");
            inp_text = inp_text.Replace(",", "");

            if (inp_text.Contains("x"))
            {
                xPos.Add(inp_text);
            }

            if (inp_text.Contains("z"))
            {
                zPos.Add(inp_text);
            }
        }

        numberOfPointsPerPath = numberOfPointsPerPath / 40;
        //print(numberOfPointsPerPath);
        //print(xPos.Count);
        AllowednumberOfPaths = (xPos.Count) / numberOfPointsPerPath;

        for (int i = 0; i < AllowednumberOfPaths; i++)
        {
            for (int j = 0; j < numberOfPointsPerPath; j++)
            {
                xPos_temp.Add(xPos[j + (i * numberOfPointsPerPath)]);
                //print(j + (i * 5));
                //print(xPos[j + (i * numberOfPointsPerPath)]);
                zPos_temp.Add(zPos[j + (i * numberOfPointsPerPath)]);
            }
            //print("--------------");

            xPos_temp.Sort();
            zPos_temp.Sort();
            for (int j = 0; j < numberOfPointsPerPath; j++)
            {
                xPos[j + (i * numberOfPointsPerPath)] = xPos_temp[j];
                zPos[j + (i * numberOfPointsPerPath)] = zPos_temp[j];
            }
            xPos_temp.Clear();
            zPos_temp.Clear();
        }

        waypoint_groups = new List<Vector3>[AllowednumberOfPaths];

        for (int i = 0; i < AllowednumberOfPaths; i++)
        {
            waypoint_groups[i] = new List<Vector3>();       //every entry in the waypoint_groups(array of lists) is a list containing waypoints of one path each

            for (int j = 0; j < numberOfPointsPerPath; j++)
            {
                string[] xTemp = xPos[j + (i * numberOfPointsPerPath)].Split('=');
                string[] zTemp = zPos[j + (i * numberOfPointsPerPath)].Split('=');
                //xPos[j + (i * 5)] = xPos[j + (i * 5)].Substring(3);         //change this
                //zPos[j + (i * 5)] = zPos[j + (i * 5)].Substring(3);         //change this

                waypoint_groups[i].Add(new Vector3(float.Parse(xTemp[1]), 0.0f, float.Parse(zTemp[1])));        //capture the value to the right of the string split i.e transform postion cordinates
                //print(waypoint_groups[i][j]);
            }
        }

        return numberOfPointsPerPath;
    }
}
