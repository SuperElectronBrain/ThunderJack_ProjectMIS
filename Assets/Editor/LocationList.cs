using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using System.IO;
using System.Text;

public class LocationList : EditorWindow
{
    static List<EditorLocationData> locationList = new List<EditorLocationData>();
    static CinemachineVirtualCamera topCam;

    GameObject selectPos;

    bool isActive = false;

    public const string locationName = "Location Name";

    public const string filePath = "Editor/LocationList.csv";

    [MenuItem("LocationList/List")]
    // Start is called before the first frame update   
    static void DataTableLoad()
    {
        LocationList window = (LocationList)EditorWindow.GetWindow(typeof(LocationList));

        var loadCam = Instantiate(Resources.Load<CinemachineVirtualCamera>("TopCam"));
        topCam = loadCam.GetComponent<CinemachineVirtualCamera>();

        window.Show();
    }

    void Init()
    {
        
    }

    void ChangeView()
    {
        isActive = !isActive;

        if(isActive)
            topCam.Priority = 1000;
        else
            topCam.Priority = 1;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(selectPos != null)
                Destroy(selectPos);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                selectPos = Instantiate(Resources.Load<GameObject>("SelectPos"), hit.point, Quaternion.identity) as GameObject;
            }            
        }

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var scroll = Input.GetAxis("Mouse ScrollWheel") * 3;

        var newPos = topCam.transform.position;
        newPos.x += x;
        newPos.z += z;

        newPos.y += scroll;

        topCam.transform.position = newPos;
    }

    void ShowLocationList()
    {
        for(int i = 0; i < locationList.Count; i++)
        {
            locationList[i].locationName = GUILayout.TextField(locationList[i].locationName);
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Label("Components");

        GUILayout.EndVertical();

        //obj = EditorGUILayout.ObjectField("Input GameObject", obj, typeof(GameObject), true) as GameObject;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Location List Info", GUILayout.Width(200), GUILayout.Height(30)))
        {
            WriteLocationInfo();
        }
        else if (GUILayout.Button("Add", GUILayout.Width(120), GUILayout.Height(30)))
        {
            locationList.Add(
                new EditorLocationData
                {
                    locationPos = selectPos.transform.position,
                    locationName = locationName
                }
                );
        }
        else if(GUILayout.Button("ChangeView", GUILayout.Width(120), GUILayout.Height(30)))
        {
            ChangeView();
        }      
        GUILayout.EndHorizontal();

        //////////////////////////////

        GUILayout.BeginVertical();

        GUILayout.Label("Location List");

        if (locationList.Count > 0)
        {
            ShowLocationList();
        }

        GUILayout.EndVertical();
    }

    void WriteLocationInfo()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));

        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        StreamWriter writer = new StreamWriter(fileStream);

        for (int i = 0; i < locationList.Count; i++)
        {
            writer.WriteLine(string.Format("{0}, {1}, {2}, {3}", locationList[i].locationName, locationList[i].locationPos.x, locationList[i].locationPos.y, locationList[i].locationPos.z));
        }

        writer.Close();
    }

    private void OnDestroy()
    {
        Destroy(topCam.gameObject);
    }

    class EditorLocationData
    {
        public Vector3 locationPos;
        public string locationName;
    }
}
