using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

public class TestMap : MonoBehaviour
{
    MapCreater _mapCreater;
    int X;
    int Y;
    int[,] defaultMatrix;
    public List<string> triedList;
    int trycount;
    bool completed;
    public void LoadValues(int newX,int newY, MapCreater _newmapCreater)
    {
        X = newX;
        Y = newY;
        _mapCreater = _newmapCreater;
        defaultMatrix = new int[X,Y];
        SetMatrix(_mapCreater.matrix, defaultMatrix);
    }
    
    public void TryButton()
    {
        ClearTint();
        StartCoroutine(TryButtonIE());
    }
    public IEnumerator TryButtonIE()
    {
        int MaxTryCount = _mapCreater.matrix.Length -_mapCreater.BlackCubeCount;
        for(int i = 1; i <= MaxTryCount - 1; i++)
        {
            trycount = 0;
            triedList.Clear();
            long resultTop = GetFakt(MaxTryCount); 
            long resultBot = GetFakt(i) * GetFakt(MaxTryCount - i);
            int requiredTryCount =(int)( resultTop / resultBot);
            while (requiredTryCount != trycount)
            {
                StartTry(i);
                if(completed)
                {
                    break;
                }
                yield return null;
            }
            if (completed)
            {
                break;
            }
        }
    }
    public void StartTry(int size)
    {
        int[] triedSize = new int[size];
        for (int nullVal = 0; nullVal < triedSize.Length; nullVal++)
        {
            triedSize[nullVal] = -1;
        }

        for (int i = 0; i < size; i++)
        {
            int element = GetRandom(triedSize);
            triedSize[i] = element;
        }
        Array.Sort(triedSize);
        string triedString = "";
        for (int st = 0;st < triedSize.Length; st++)
        {
            string space = "_";
            if (st == 0)
            {
                space ="";
            }
            triedString = triedString + space + triedSize[st];
        }
        if (!triedList.Contains(triedString))
        {
            if (TryMatrix(triedSize)) 
            {
                completed = true;
                Debug.Log("Baþarýlý: "+triedString);
                SetMatrix(defaultMatrix, _mapCreater.matrix);
                SetTint(triedSize);
            }
            else
            {
                trycount++;
                triedList.Add(triedString);
            }
            _mapCreater.UpdateCubeColors();
        }
        SetMatrix(defaultMatrix, _mapCreater.matrix);
    }
    public bool TryMatrix(int[] arrayElement)
    {
        bool result = true;
        for (int i = 0; i < arrayElement.Length; i++)
        {
            int x = arrayElement[i] % X;
            int y = arrayElement[i] / X;
            Converter(x, y);
        }
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                if (_mapCreater.matrix[i, j] == 0)
                {
                    result = false;
                    break;
                }
            }
        }
        if (result)
        {
            for (int i = 0; i < arrayElement.Length; i++)
            {
                Debug.Log(arrayElement[i] % X +" "+arrayElement[i] /X);
            }
        }
        return result;
    }
    int GetRandom(int[]target)
    {
        Again:
        int random= UnityEngine.Random.Range(0, _mapCreater.matrix.Length);
        
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] == random )
            {
                goto Again;
            }
        }
        int tX = random % X;
        int tY = random / X;
        if (_mapCreater.matrix[tX, tY] == 2)
        {
            goto Again;
        }
        return random;
    }
    void Converter(int firstX, int firstY)
    {
        if (_mapCreater.matrix[firstX, firstY] == 2)
        {
            return;
        }
        ConvertToValue(firstX, firstY);
        if (firstX - 1 >= 0)
        {
            ConvertToValue(firstX - 1, firstY);
        }
        if (firstX + 1 < X)
        {
            ConvertToValue(firstX + 1, firstY);
        }
        if (firstY - 1 >= 0)
        {
            ConvertToValue(firstX, firstY - 1);
        }
        if (firstY + 1 < Y)
        {
            ConvertToValue(firstX, firstY + 1);
        }
        ////Çapraz
        //if (firstX - 1 >= 0 && firstY - 1 >= 0)
        //{
        //    ConvertToValue(firstX - 1, firstY - 1);
        //}
        //if (firstX + 1 < X && firstY - 1 >= 0)
        //{
        //    ConvertToValue(firstX + 1, firstY - 1);
        //}
        //if (firstX - 1 >= 0 && firstY + 1 < Y)
        //{
        //    ConvertToValue(firstX - 1, firstY + 1);
        //}
        //if (firstX + 1 < X && firstY + 1 < Y)
        //{
        //    ConvertToValue(firstX + 1, firstY + 1);
        //}
    }
    void ClearTint()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _mapCreater.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
    void SetTint(int []seri)
    {
        for (int i = 0; i < seri.Length; i++)
        {
            _mapCreater.transform.GetChild(seri[i]).GetChild(0).gameObject.SetActive(true);
        }
    }
    void SetMatrix(int [,]nowMatrix,int [,]targetMatrix)
    {
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                targetMatrix[i, j] = nowMatrix[i, j];
            }
        }
    }
    void ConvertToValue(int newX, int newY)
    {
        if (_mapCreater.matrix[newX, newY] == 2)
        {
            return;
        }
        _mapCreater.matrix[newX, newY] = _mapCreater.matrix[newX, newY] == 0 ? 1 : 0;
    }
    long GetFakt(int number)
    {
        long fakt = 1;
        for (int i = 1; i<= number; i++)
        {
            fakt*= i;
        }
        return fakt;
    }
}
