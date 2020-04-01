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

    public Material startMat;
    public Material endMat;
    public Material unwalkMat;
    public Material pathMat;

    private int start_x = 0;
    private int start_y = 0;
    private int end_x = 5;
    private int end_y = 5;

    private List<List<GameObject>> cubeList = new List<List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        AstarManager.Instance.InitMap(width, height);
        CreateMap();

        List<BaseNode> paths = AstarManager.Instance.FindPath(0, 0, 5, 5);
        if (paths == null)
            return;
        foreach(var path in paths)
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
                    newcube.GetComponent<MeshRenderer>().material = endMat;

                }
                colList.Add(newcube);
            }
            cubeList.Add(colList);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
