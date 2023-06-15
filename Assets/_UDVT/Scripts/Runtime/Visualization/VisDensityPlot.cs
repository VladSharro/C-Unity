using System.Linq;
using System;
using UnityEngine;

public class VisDencityPlot : Vis
{
    // Start is called before the first frame update
    public VisDencityPlot()
    {
        title = "VisBar";
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Bar");
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
    }

    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);


        xyValues binData = SquareRoot(dataSets[0].ElementAt(0).Value);

        //## 01:  Create Axes and Grids

        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, binData.xValues, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis("Vani", binData.yValues, Direction.Y);
        visContainer.CreateAxis(dataSets[0].ElementAt(1).Key, binData.yValues, Direction.Y);
           

        //## 02: Set Remaining Vis Channels (Color,...)

        visContainer.SetChannel(VisChannel.XPos, binData.xValues);
        visContainer.SetChannel(VisChannel.YSize, binData.yValues);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(3).Value);


        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);

        return visContainerObject;
    }

    public class xyValues
    {
        public double[] xValues { get; set; }
        public double[] yValues { get; set; }
    }

    private xyValues SquareRoot(double[] xArray)
    {
        int binCount = (int)(Math.Pow(xArray.Length, 1/2)) + 20;   // calculate the number of bins
                                                                    // create list to store processed data
        
        double Xmin = xArray.Min();
        double Xmax = xArray.Max();


        double diff = Xmax - Xmin;

        double rate = (diff / binCount);

        //Array.Sort(xArray);


        double[] newX = new double[(int)(binCount)];
        double[] newY = new double[(int)(binCount)];
        int bookmark = 0;

        double cc = Xmin;
        //Debug.Log("ASDAFSASFA    " + rate);

        for (int i = 0; i < binCount; i++)
        {
            double counter = 0;

            for (int j = 0; j < xArray.Length; j++)
            {
                if ((xArray[j] >= cc) & (xArray[j] < (cc + rate))){
                    counter++;
                }
               
            }
            newY[i] = cc;
            cc = cc + rate;
            newX[i] = counter;
            Debug.Log("Counter   " + counter);
            //newY[i] = cc;
            Debug.Log(newX[i]);
        }
        
        return new xyValues { xValues = newY, yValues = newX};
    }
}