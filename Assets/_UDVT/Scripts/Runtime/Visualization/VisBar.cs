using System.Linq;
using System;
using UnityEngine;

public class VisBar : Vis
{
    public VisBar()
    {
        title = "Bar";

        //Define Data Mark and Tick Prefab
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Bar"); //dataMarkPrefab Prefabs/DataVisPrefabs/Marks/Sphere"
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
    }

    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);

        //## 01:  Create Axes and Grids

        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X); //check it
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(1).Key, dataSets[0].ElementAt(1).Value, Direction.Y);

        // Z Axis
        //visContainer.CreateAxis(dataSets[0].ElementAt(2).Key, dataSets[0].ElementAt(2).Value, Direction.Z);
        //visContainer.CreateGrid(Direction.Y, Direction.Z);
        //visContainer.CreateGrid(Direction.Z, Direction.X);

        //## 02: Set Remaining Vis Channels (Color,...)
        visContainer.SetChannel(VisChannel.XPos, dataSets[0].ElementAt(0).Value);
        visContainer.SetChannel(VisChannel.YSize, dataSets[0].ElementAt(1).Value);
        //visContainer.SetChannel(VisChannel.ZPos, dataSets[0].ElementAt(2).Value);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(3).Value);

        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height/*, depth*/); // end of checking

        //xyValues binData = SturgesFormulaBinning(dataSets[0].ElementAt(0).Value, dataSets[0].ElementAt(1).Value); // mute if should be
        //for (int i = 0; i < binData.xValues.Length; i++)
        //{
        //    Debug.Log(string.Join(", ", binData.xValues[i]));
        //}
        //Debug.Log(string.Join(", ", binData.xValues[0]));

        //## 01:  Create Axes and Grids

        // X Axis
        //visContainer.CreateAxis(/*dataSets[0].ElementAt(1).Key*/ "Everything", binData.xValues, Direction.X);
        //visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        //visContainer.CreateAxis(dataSets[0].ElementAt(1).Key, binData.yValues, Direction.Y);

        /*Debug.Log(dataSets[0].GetType());
        for (int i = 0; i < dataSets[0].Count; i++)
        {
            Debug.Log(string.Join(", ", dataSets[0].ElementAt(i)));
        }
        //## 02: Set Remaining Vis Channels (Color,...)*/

        //Debug.Log(string.Join(", ", dataSets[0].ElementAt(0).Value));
        //Debug.Log(string.Join(", ", dataSets[0].Value));
        //Debug.Log(dataSets[0].Value.GetType());

        //double[] binData = SturgesFormulaBinning(dataSets[0].ElementAt(3).Value)
        //visContainer.SetChannel(VisChannel.XPos, binData.xValues);

        //visContainer.SetChannel(VisChannel.YSize, dataSets[0].ElementAt(3).Value);

        //visContainer.SetChannel(VisChannel.YSize, binData.yValues);
        //## 03: Draw all Data Points with the provided Channels 
        //visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        //visContainerObject.transform.localScale = new Vector3(width, height, depth);*/  // end of checking*/

        return visContainerObject;
    }

    /*public class xyValues
    {
        public double[] xValues { get; set; }
        public double[] yValues { get; set; }

    }*/

    /*public xyValues SturgesFormulaBinning(double[] xArray, double[] yArray)
    {
        //int binCount = (int)(1 + Math.Log(yArray.Length, 2)) + 1;
        int binCount = (int)( 2*Math.Pow(yArray.Length , 1/3)) + 12;
        //int binContent = (int)(yArray.Length / binCount);
        //Debug.Log(binCount);

        double[] processedyArray = new double[binCount];
        double[] processedxArray = new double[binCount];
        double[][] xyArray = new double[xArray.Length][];

        for (int i = 0; i < xArray.Length; i++)
        {
            if (xArray[i] < 0)
            {
                xyArray[i] = new double[] { 0.0, yArray[i] };
            }
            else
            {
                xyArray[i] = new double[] { xArray[i], yArray[i] };
            }
        }

        Array.Sort(xyArray, (a, b) => a[0].CompareTo(b[0]));

        double xMax = xyArray[xyArray.Length - 1][0];
        double xMin = xyArray[0][0];

        double xRange = xMax - xMin;
        //Debug.Log(xRange);
        double binSingleRange = xRange / binCount;

        //for (int i = 0; i < xyArray.Length; i++)
        //{
        //    Debug.Log(string.Join(", ", xyArray[i]));
        //}

        int bookMark = 0;
        for (int i = 0; i < binCount; i++)
        {
            double tempValueY = 0.0;
            for (int j = bookMark; j < xArray.Length; j++)
            {
                if (xyArray[j][0] > (xMin + (i + 1) * binSingleRange))
                {
                    bookMark = j;
                    break;
                }
                tempValueY += xyArray[j][1];
                //Debug.Log(tempValueY);
            }

            processedyArray[i] = tempValueY / bookMark;
            processedxArray[i] = xMin + binSingleRange * (i + 0.5);
            //Debug.Log(bookMark * (i + 0.5));
            //Debug.Log(processedyArray[i]);
        }

        return new xyValues { xValues = processedxArray, yValues = processedyArray };
        //return xArray;
    }*/

}