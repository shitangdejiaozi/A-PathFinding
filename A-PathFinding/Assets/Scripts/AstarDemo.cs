using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarDemo : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public GameObject cube;

    public float startx = 0;
    public float starty = 0;

    public Material defaultmat;
    public Material startMat;
    public Material endMat;
    public Material unwalkMat;
    public Material pathMat;

    private int start_x = 0;
    private int start_y = 0;
    private int end_x = 5;
    private int end_y = 5;
    private List<BaseNode> paths = new List<BaseNode>();
    private List<List<GameObject>> cubeList = new List<List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        AstarManager.Instance.InitMap(width, height);
        CreateMap();
       
    }

    private void ShowPath(List<BaseNode> paths)
    {
        if (paths == null)
            return;
        foreach (var path in paths)
        {
            if (path.parentNode == null)
                continue;
            GameObject cubego = cubeList[path.x][path.y];
            cubego.GetComponent<MeshRenderer>().material = pathMat;
        }
    }

    public void CreateMap()
    {
        List<List<BaseNode>> mapLists = AstarManager.Instance.mapLists;
        for(int i = 0; i < width; i++)
        {
            List<GameObject> colList = new List<GameObject>();
            for(int j = 0; j < height; j++)
            {
                GameObject newcube = GameObject.Instantiate(cube);
                newcube.transform.localPosition = new Vector3(startx + 2 * i, starty + 2 * j, 0);
                newcube.name = i + "_" + j;
                if(mapLists[i][j].nodeType == NodeType.unwalk)
                {
                    newcube.GetComponent<MeshRenderer>().material = unwalkMat;
                }

                if(i == 0 && j == 0)
                {
                    newcube.GetComponent<MeshRenderer>().material = startMat;
                }

                if(i == width -1 && j == height -1)
                {
                    
                }
                colList.Add(newcube);
            }
            cubeList.Add(colList);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out info, 1000))
            {
                string[] strs = info.collider.gameObject.name.Split('_');
                int x = int.Parse(strs[0]);
                int y = int.Parse(strs[1]);
                ResetPath();
                GameObject cubego = cubeList[x][y];
                cubego.GetComponent<MeshRenderer>().material = endMat;

                paths.Clear();
                paths = AstarManager.Instance.FindPath(0, 0, x, y);
                ShowPath(paths);

            }
        }
    }

    private void ResetPath()
    {
        for(int i = 0; i< paths.Count; i++)
        {
            GameObject cubego = cubeList[paths[i].x][paths[i].y];
            cubego.GetComponent<MeshRenderer>().material = defaultmat;
        }
    }
}
