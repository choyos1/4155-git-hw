using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UCharts : MonoBehaviour
{
    private RectTransform m_Transform;

    [SerializeField]
    private float class_A = 0.1f;           //块.
    [SerializeField]
    private float class_B = 0.3f;
    [SerializeField]
    private float class_Other;

    [Header("份数")]
    [SerializeField]
    private int num = 10;                   //份数.
    [Header("厚度")]
    [SerializeField]
    private float height = 0.5f;            //厚度.

    [SerializeField]
    [Header("蓝色材质球")]
    private Material material_Blue;         //蓝色材质球.
    [SerializeField]
    [Header("红色材质球")]
    private Material material_Red;          //红色材质球.
    [SerializeField]
    [Header("正常材质球")]
    private Material material_Other;        //正常颜色的材质球.

    private List<Transform> pieChartList;   //饼状图基块集合.

    //private bool isDrag = false;        //是否拖拽.
    //public bool IsDrag { get { return isDrag; } set { isDrag = value; } }

    void Awake()
    {
        //初始化.
        m_Transform = gameObject.GetComponent<RectTransform>();
        pieChartList = new List<Transform>();
      

        //根据份数，生成饼状图.
        GeneratePieChart();

        //根据比例，划分饼状图颜色.
        SetPieChartColor();
	}

    void Update()
    {
        //if (isDrag)
        //{
        //    //手动旋转饼状图.
        //    m_Transform.localRotation = Quaternion.Euler(m_Transform.localRotation.eulerAngles.x, m_Transform.localRotation.eulerAngles.y - Input.GetAxis("Mouse X") * 5, 0);
        //}

        //自动旋转.
        m_Transform.localRotation = Quaternion.Euler(m_Transform.localRotation.eulerAngles.x, m_Transform.localRotation.eulerAngles.y - Time.deltaTime * 50, 0);        
    }

    /// <summary>
    /// 生成饼状图.
    /// </summary>
    private void GeneratePieChart()
    {
        float rot = 360.0f / num;
        for (int i = 0; i < num; i++)
        {
            Transform child = GenerateChildPieChart(3, num, height);   //饼状图基块.
            child.localRotation = Quaternion.Euler(0, rot * i, 0);      
            child.localPosition = new Vector3(0, height / 2.0f, 0);    
            child.gameObject.layer = LayerMask.NameToLayer("UI");
            child.SetParent(m_Transform, false);
            pieChartList.Add(child);
        }
        m_Transform.localScale = Vector3.one * 20;
        m_Transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    /// <summary>
    /// 创建饼状图基块.
    /// </summary>
    /// <param name="r">半径</param>
    /// <param name="num">份数</param>
	/// <param name="height">厚度</param>
	/// <returns></returns>
    private Transform GenerateChildPieChart(float r, int num, float height)
    {
        float y = r * Mathf.Sin((180.0f / num) * Mathf.Deg2Rad);
        float x = Mathf.Sqrt(r * r - y * y);
        Vector3 origin1 = new Vector3(0, 0, 0);
        Vector3 origin2 = new Vector3(x, 0, y);
        Vector3 origin3 = new Vector3(x, 0, -y);
        Vector3 offset = new Vector3(0, height, 0);
 
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[6];
        vertices[0] = origin1;                  
        vertices[1] = origin2;
        vertices[2] = origin3;
        vertices[3] = vertices[0] - offset;     
        vertices[4] = vertices[1] - offset;
        vertices[5] = vertices[2] - offset;
        int[] triangles = new int[48] {     //双面都要.
            0, 1, 2, 3, 4, 5,   //顺时针 正面.
            0, 2, 5, 0, 5, 3, 
            0, 1, 4, 0, 4, 3,
            2, 1, 4, 2, 4, 5,

            0, 2, 1, 3, 5, 4,   //逆时针 反面.
            0, 5, 2, 0, 3, 5, 
            0, 4, 1, 0, 3, 4,
            2, 4, 1, 2, 5, 4 
        };
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        GameObject temp = new GameObject();
        MeshFilter meshFilter = temp.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = temp.AddComponent<MeshRenderer>();
        //meshRenderer.material.shader = Shader.Find("Diffuse");
        meshRenderer.material = material_Other;

        return temp.transform;
    }

    /// <summary>
    /// 设置饼状图基块颜色.
    /// </summary>
    private void SetPieChartColor()
    {
        int class_A_Num = Mathf.FloorToInt(num * class_A);
        int class_B_Num = Mathf.FloorToInt(num * class_B);
        int class_Other_Num = Mathf.FloorToInt(num * class_Other);
        for (int i = 0; i < pieChartList.Count; i++)
        {
            if (i <= class_A_Num)
            {
                pieChartList[i].GetComponent<MeshRenderer>().material = material_Blue;
                //pieChartList[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.blue);
                //pieChartList[i].GetComponent<MeshRenderer>().material.SetColor("_RimColor", Color.blue);
                //pieChartList[i].position = pieChartList[i].position + pieChartList[i].right * 0.1f;
            }
            else if (i > class_A_Num && i <= class_B_Num + class_A_Num)
            {
                pieChartList[i].GetComponent<MeshRenderer>().material = material_Red;
                //pieChartList[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
                //pieChartList[i].GetComponent<MeshRenderer>().material.SetColor("_RimColor", Color.red);
                //pieChartList[i].position = pieChartList[i].position + pieChartList[i].up * 0.05f;
            }
        }
    }

}
