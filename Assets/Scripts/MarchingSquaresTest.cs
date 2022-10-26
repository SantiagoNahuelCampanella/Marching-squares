using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MarchingSquaresTest : MonoBehaviour
{
    public int sizeW,sizeH;
    public List<int> squareCase;


    public float[,] points;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,0);

        points = new float[sizeH, sizeW];


        for (int y = 0; y < sizeH; y++)
        {
            for (int x = 0; x < sizeW; x++)
            {
                points[y,x] = Random.Range(0f, 1f);
            }
        }


        for (int y = 0; y < sizeH - 1; y++)
            {
                for (int x = 0; x < sizeW - 1; x++)
                {
                    int caseValue = 0;
                
                    if (points[y, x] > 0.5f) { caseValue |= 1; }
                    if (points[y, x + 1] > 0.5f) { caseValue |= 2; }
                    if (points[y + 1, x + 1] > 0.5f) { caseValue |= 4; }
                    if (points[y + 1, x] > 0.5f) { caseValue |= 8; }

                    squareCase.Add(caseValue);
                }
            }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (points != null) {
            for (int y = 0; y < sizeH; y++)
            {
                for (int x = 0; x < sizeW; x++)
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.black, points[y,x]);
                    Handles.Label(new Vector3(x, 0, y) + transform.position, "" + points[y,x]);
                    Gizmos.DrawWireSphere(new Vector3(x, 0, y) + transform.position, 0.05f);
                }
            }
        }
    }

    void HandleSquareCase(int squareCase, int startPosX , int startPosY) {
        int[,] triangulationTable;

        Vector3[] pointPos = new Vector3[4];

        pointPos[0] = new Vector3(startPosY, 0, startPosX);
        pointPos[1] = new Vector3(startPosY, 0, startPosX + 1);
        pointPos[2] = new Vector3(startPosY + 1, 0, startPosX + 1);
        pointPos[3] = new Vector3(startPosY + 1, 0, startPosX);

        triangulationTable = new int[16,18] 
        { 
            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //0 

            { 0,-1, 0, 1, 3, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //1

            { 0, 1, 1,-1, 1, 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //2

            { 0,-1, 1,-1, 1, 2, 0,-1, 1, 2, 3, 0,-1,-1,-1,-1,-1,-1}, //3

            { 1, 2, 2,-1, 2, 3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //4

            { 0,-1, 0, 1, 3, 0, 1, 2, 2,-1, 2, 3,-1,-1,-1,-1,-1,-1}, //5

            { 0, 1, 1,-1, 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //6

            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //7

            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //8

            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //9

            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //10

            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //11

            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //12

            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //13

            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //14

            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}  //15
        };

        for (int i = 0; i < 16; i += 2)
        {
            //Gizmos.DrawLine();
        }
    }
}

