using System.Linq;
using System;
using UnityEngine;

public class VisBarChart : Vis
{
    // Start is called before the first frame update
    public VisBarChart()
    {
        title = "VisBar";
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Bar");
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
    }

    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);

        double[][] test = { dataSets[0].ElementAt(1).Value, dataSets[0].ElementAt(2).Value };

        xyValues binData = SturgesFormulaBinning(dataSets[0].ElementAt(0).Value, test);

        //## 01:  Create Axes and Grids

        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, binData.xValues, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(1).Key, binData.yValues[1], Direction.Y);

        //## 02: Set Remaining Vis Channels (Color,...)

        visContainer.SetChannel(VisChannel.XPos, binData.xValues);
        visContainer.SetChannel(VisChannel.YSize, binData.yValues[1]);

        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);

        return visContainerObject;
    }

    public class xyValues
    {
        public double[] xValues { get; set; }
        public double[][] yValues { get; set; }
    }

    private xyValues SturgesFormulaBinning(double[] xArray, double[][] yArray)
    {
        int binCount = (int)(1 + Math.Log(xArray.Length, 2)) + 1;   // calculate the number of bins
                                                                    // create list to store processed data
        double[][] processedyArray = new double[yArray.Length][];   // processedyArray => [[y11, y12, y13...], [y21, y22, y23...], ...]
        for (int i = 0; i < yArray.Length; i++)                     // double[][] processedyArray = new double[yArray.Length][binCount];
        {
            processedyArray[i] = new double[binCount];
        }
        double[] processedxArray = new double[binCount];            // processedxArray => [x1, x2,...]
        double[][] xyArray = new double[xArray.Length][];           // xyArray => [[x1, y11, y12, y13...], [x2, y21, y22, y23...], ...]
        for (int i = 0; i < xArray.Length; i++)                     // double[][] xyArray = new double[xArray.Length][yArray.Length];
        {
            xyArray[i] = new double[yArray.Length + 1];
        }

        for (int i = 0; i < xArray.Length; i++)
        {
            xyArray[i][0] = 0.0;
            for (int j = 1; j < yArray.Length + 1; j++)
            {
                xyArray[i][j] = yArray[j - 1][i];
            }
            if (xArray[i] > 0)
            {
                xyArray[i][0] = xArray[i];
            }
        }

        Array.Sort(xyArray, (a, b) => a[0].CompareTo(b[0]));

        double xMax = xyArray[xyArray.Length - 1][0];
        double xMin = xyArray[0][0];

        double xRange = xMax - xMin;
        double binSingleRange = xRange / binCount;

        int bookMark = 0;
        for (int i = 0; i < binCount; i++)
        {
            for (int j = bookMark; j < xyArray.Length; j++)
            {
                if (xyArray[j][0] > (xMin + (i + 1) * binSingleRange))
                {
                    bookMark = j;
                    break;
                }
                for (int k = 0; k < yArray.Length; k++)
                {
                    processedyArray[k][i] += xyArray[j][k + 1];
                }
            }

            for (int j = 0; j < yArray.Length; j++)
            {
                processedyArray[j][i] = processedyArray[j][i] / bookMark;
            }

            processedxArray[i] = xMin + binSingleRange * (i + 0.5);
        }

        return new xyValues { xValues = processedxArray, yValues = processedyArray };
    }
}