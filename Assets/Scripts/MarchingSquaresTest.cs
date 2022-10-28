using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class squareData {
    public int sCase , xStart , yStart;
}

public class MarchingSquaresTest : MonoBehaviour
{
    public int sizeW,sizeH;
    public List<squareData> squares =  new List<squareData>();


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
                
                //squares are constructed clockwise, to match unity mesh renderer
                //que "saquareCase" is constructed in a binary format, with numbers 0-15 possible
                if (points[y, x] > 0.5f) { caseValue |= 1; }
                if (points[y + 1, x] > 0.5f) { caseValue |= 2; }
                if (points[y + 1, x + 1] > 0.5f) { caseValue |= 4; }
                if (points[y, x + 1] > 0.5f) { caseValue |= 8; }

                squareData tempSq = new squareData();

                tempSq.sCase = caseValue;
                tempSq.xStart = x;
                tempSq.yStart = y;

                squares.Add(tempSq);
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

            for (int i = 0; i < squares.Count; i++)
            {
                HandleSquareCase(squares[i].sCase, squares[i].xStart , squares[i].yStart);
            }

        }
    }

    void HandleSquareCase(int sCase, int startPosX , int startPosY) {
        int[,] triangulationTable;

        Vector3[] pointPos = new Vector3[4];

        //assigning a position on 3D space to the points that make up the selected square
        pointPos[0] = new Vector3(startPosX, 0, startPosY);
        pointPos[1] = new Vector3(startPosX, 0, startPosY + 1);
        pointPos[2] = new Vector3(startPosX + 1, 0, startPosY + 1);
        pointPos[3] = new Vector3(startPosX + 1, 0, startPosY);

        //list of possible triangle configurations acording to the square case (16)
        triangulationTable = new int[16,18] 
        { 
            {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //0 

            { 0,-1, 0, 1, 3, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //1

            { 0, 1, 1,-1, 1, 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //2

            { 0,-1, 1,-1, 1, 2, 0,-1, 1, 2, 3, 0,-1,-1,-1,-1,-1,-1}, //3

            { 1, 2, 2,-1, 2, 3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //4

            { 0,-1, 0, 1, 3, 0, 1, 2, 2,-1, 2, 3,-1,-1,-1,-1,-1,-1}, //5

            { 0, 1, 1,-1, 2,-1, 2,-1, 2, 3, 0, 1,-1,-1,-1,-1,-1,-1}, //6

            { 0,-1, 1,-1, 3, 0, 1,-1, 2, 3, 3, 0, 1,-1, 2,-1, 2, 3}, //7

            { 2, 3, 3,-1, 3, 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}, //8

            { 0,-1, 0, 1, 2, 3, 2, 3, 3,-1, 0,-1,-1,-1,-1,-1,-1,-1}, //9

            { 0, 1, 1,-1, 1, 2, 2, 3, 3,-1, 3, 0,-1,-1,-1,-1,-1,-1}, //10

            { 0,-1, 1,-1, 1, 2, 0,-1, 1, 2, 2, 3, 2, 3, 3,-1, 0,-1}, //11

            { 1, 2, 2,-1, 3, 0, 2,-1, 3,-1, 3, 0,-1,-1,-1,-1,-1,-1}, //12

            { 0,-1, 0, 1, 3,-1, 0, 1, 1, 2, 3,-1, 1, 2, 2,-1, 3,-1}, //13

            { 0, 1, 1,-1, 2,-1, 2,-1, 3, 0, 0, 1, 2,-1, 3,-1, 3, 0}, //14

            { 0,-1, 1,-1, 2,-1, 2,-1, 3,-1, 0,-1,-1,-1,-1,-1,-1,-1}  //15
        };

        //every 6 numbers on the table represent 1 triangle
        for (int i = 0; i < 16; i += 6)
        {
            if (triangulationTable[sCase, i] != -1)
            {
                Vector3 vertexA = new Vector3();
                Vector3 vertexB = new Vector3();
                Vector3 vertexC = new Vector3();

                //the ints are orginized on pairs, each pair represents a side of the square comprised of 2 corners
                //if the second int in the pair is a -1 (mening it doesnt exist in the shape), it means the vertex is in the corner of the first int
                if (triangulationTable[sCase, i + 1] != -1)
                {
                    vertexA = Vector3.Lerp(pointPos[triangulationTable[sCase,i]], pointPos[triangulationTable[sCase, i + 1]], 0.5f);
                }
                else 
                {
                    vertexA = pointPos[triangulationTable[sCase, i]];
                }

                if (triangulationTable[sCase, i + 3] != -1)
                {
                    vertexB = Vector3.Lerp(pointPos[triangulationTable[sCase, i + 2]], pointPos[triangulationTable[sCase, i + 3]], 0.5f);
                }
                else
                {
                    vertexB = pointPos[triangulationTable[sCase, i + 2]];
                }

                if (triangulationTable[sCase, i + 5] != -1)
                {
                    vertexC = Vector3.Lerp(pointPos[triangulationTable[sCase, i + 4]], pointPos[triangulationTable[sCase, i + 5]], 0.5f);
                }
                else
                {
                    vertexC = pointPos[triangulationTable[sCase, i + 4]];
                }

                Gizmos.color = Color.green;
                Gizmos.DrawLine(vertexA,vertexB);
                Gizmos.DrawLine(vertexB,vertexC);
                Gizmos.DrawLine(vertexC,vertexA);
            }
        }
    }
}

