﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class CSVWrite : MonoBehaviour
{
    public String fileName;
    private List<string[]> rowData = new List<string[]>();

    void Start()
    {
        CreateHeaders();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            Save(10f,1,5,8);
        }
    }

    void CreateHeaders(){

        // Creating headers by them selves
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = "Game Time";
        rowDataTemp[1] = "Current Maze";
        rowDataTemp[2] = "Current Row";
        rowDataTemp[3] = "Current Column";
        rowData.Add(rowDataTemp);
    }
    
    void Save(float gameTime, int maze, int row, int column){

        // Input data
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = "" + gameTime;
        rowDataTemp[1] = "" + maze;
        rowDataTemp[2] = "" + row;
        rowDataTemp[3] = "" + column;
        rowData.Add(rowDataTemp);

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++){
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ";";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));
        
        
        string filePath = getPath();

        StreamWriter outStream = new StreamWriter(filePath, false);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        return Application.dataPath + "/CSV/" + fileName + ".csv";
    }
}