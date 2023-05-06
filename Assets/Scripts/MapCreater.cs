using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UI;

public class MapCreater : MonoBehaviour
{
    [SerializeField] TestMap _testMap;
    [SerializeField] MapData _mapData;
    public GameObject Button;
    public int X;
    public int Y;
    public int BlackCubeCount;
    public int[,] matrix;
    int tempBlackCubeCount;
    float startX;
    void Awake()
    {
        LoadMap();
    }
    void Start()
    {
        AddClickerEvents();
        _testMap.LoadValues(X,Y,this);
    }
    public void CreateMap()
    {
        tempBlackCubeCount = BlackCubeCount;
        matrix = new int[X,Y];
        startX = ((X - 1) * -30) / 2f;

        for (int b=0; b < Y; b++)
        {
            for (int a = 0; a < X;a++)
            {
                matrix[a,b] = 0;
                CreateButton(a,b);
            }
        }
        SetBlackCubes();
        UpdateCubeColors();
        SaveMap();
    }
    public void ClearMap()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    void LoadMap()
    {
        int counter = -1;
        matrix = new int[X,Y];
        DataManager.LoadData(_mapData);
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                counter++;
                matrix[i, j] = _mapData.MatrixData[counter];
            }
        }
    }
    void SaveMap()
    {
        _mapData.MatrixData.Clear();
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                _mapData.MatrixData.Add(matrix[i, j]);
            }
        }
        DataManager.SaveData(_mapData);
    }
    void CreateButton(int x,int y)
    {
        GameObject createdButton = Instantiate(Button);
        createdButton.gameObject.name = "Button["+x + "-" + y+"]";
        createdButton.transform.parent = transform;
        createdButton.transform.localScale = Vector3.one;
        createdButton.transform.localPosition = new Vector3(startX + (30 * x), (30 * -y), 0);
    }
    void AddClickerEvents()
    {
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                int tempX = i;
                int tempY= j;

                Button selectedButton = transform.GetChild((i%X) + j*Y).gameObject.GetComponent<Button>();
                selectedButton.onClick.RemoveAllListeners();
                selectedButton.onClick.AddListener(()=>PressedButton(tempX,tempY));
            }
        }
    }
    void SetBlackCubes()
    {
        while(tempBlackCubeCount!=0)
        {
            int rndX = UnityEngine.Random.Range(0, X);
            int rndY = UnityEngine.Random.Range(0, Y);
            if (matrix[rndX, rndY] != 2)
            {
                matrix[rndY, rndX] = 2;
                tempBlackCubeCount--;
            }
        }
    }
   public void UpdateCubeColors()
    {
        for (int i = 0; i < X; i++)
        {
            for(int j = 0; j < Y; j++)
            {
                Image selectedImage= transform.GetChild((X*j)+i).gameObject.GetComponent<Image>(); // 00 10 01   

                switch (matrix[i,j])
                {
                    case 0:
                        selectedImage.color = Color.white;
                        break;
                    case 1:
                        selectedImage.color = Color.green;
                        break;
                    case 2:
                        selectedImage.color = Color.black;
                        break;
                }
            }
        }   
    }
    public void PressedButton(int firstX, int firstY)
    {
        if (matrix[firstX, firstY] == 2)
        {
            return;
        }
        ConvertToValue(firstX, firstY);
        if (firstX - 1 >= 0)
        {
            ConvertToValue(firstX-1, firstY);
        }
        if (firstX + 1 < X) 
        {
            ConvertToValue(firstX+1, firstY);
        }
        if (firstY - 1 >= 0)
        {
            ConvertToValue(firstX, firstY - 1);
        }
        if (firstY + 1 < Y)
        {
            ConvertToValue(firstX, firstY + 1);
        }
        //Çapraz
        if (firstX - 1 >= 0 && firstY - 1 >= 0)
        {
            ConvertToValue(firstX - 1, firstY - 1);
        }
        if (firstX + 1 < X && firstY - 1 >= 0)
        {
            ConvertToValue(firstX + 1, firstY - 1);
        }
        if (firstX - 1 >= 0 && firstY + 1 < Y)
        {
            ConvertToValue(firstX - 1, firstY + 1);
        }
        if (firstX + 1 < X && firstY + 1 < Y)
        {
            ConvertToValue(firstX + 1, firstY + 1);
        }
        UpdateCubeColors();
    }
    void ConvertToValue(int newX,int newY)
    {
        if (matrix[newX, newY] == 2)
        {
            return;
        }
        matrix[newX, newY] = matrix[newX,newY] == 0 ? 1 : 0;
    }
}
[CustomEditor(typeof(MapCreater))]
public class MapInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapCreater mapCreater = (MapCreater)target;
        if (GUILayout.Button("Create Map"))
        {
            mapCreater.CreateMap();   
        }
        if (GUILayout.Button("Clear Map"))
        {
            mapCreater.ClearMap();
        }
    }
}
