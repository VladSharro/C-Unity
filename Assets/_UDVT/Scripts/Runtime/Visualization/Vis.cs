using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//ToDo: Add here your own Visualization Type
/// <summary>
/// Possible Visulization Types to choose from.
/// </summary>
public enum VisType
{
    Bar,//Scatterplot
    NumberOfVisTypes
}

/// <summary>
/// Possible Visulization Channels
/// </summary>
public enum VisChannel
{
    XPos,
    YPos,
    ZPos,
    XSize,
    YSize,
    ZSize,
    XRotation,
    YRotation,
    ZRotation,
    Color,
    NumberOfVisChannels
}

/// <summary>
/// Base class to create different data visualizations charts. Defines default values and methods which every child class inherits.
/// </summary>
[Serializable]
public class Vis
{
    // Vis container and used Prefabs
    public VisContainer visContainer;
    public GameObject visContainerObject;
    public GameObject dataMarkPrefab;
    public GameObject tickMarkPrefab;


    // Data
    public List<Dictionary<string, double[]>> dataSets;         // List of Datasets as Dictionaries with all data attributes with their <name,values>. Dictionaries should all have the same attributes
    public int dimensions = 0;                                  // Number of attributes retrieved from the dataSet.
    public List<int> numberOfValues;                            // Number of values for each attribut from the dataSet.

    public List<Dictionary<string, double[]>> valuess;


    // Visualization Properties:
    public string title = "Basic Euclidean Vis";                // Title of vis.

    public float width = 0.25f;                                 // Vis container width in centimeters.
    public float height = 0.25f;                                // Vis container height in centimeters.
    public float depth = 0.25f;                                 // Vis container depth in centimeters.

    public float[] xyzOffset = new[] { 0.1f, 0.1f, 0.1f };         // Offset from origin (0,0) and End (1,0) for the Axes (x,y,z).
    public int[] xyzTicks = { 21, 10, 10 };                     // Amount of Ticks between min/max tick for Axes (x,y,z).
    public Color[] colorScheme = { Color.cyan, Color.magenta };       // Defines Color Scheme for Color Channel


    /// <summary>
    /// Convenience function to set all visualization parameters at once.
    /// </summary>
    /// <param name="visTitle"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <param name="xyzOffset"></param>
    /// <param name="xyzTicks"></param>
    /// <param name="colorScheme"></param>
    public virtual void InitVisParams(string visTitle, float width, float height, float depth, float[] xyzOffset, int[] xyzTicks, Color[] colorScheme)
    {
        title = visTitle;
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.xyzOffset = xyzOffset;
        this.xyzTicks = xyzTicks;
        this.colorScheme = colorScheme;
    }

    /// <summary>
    /// Main method to creates a new VisContainer with the default values.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public virtual GameObject CreateVis(GameObject container)
    {

        visContainer = new VisContainer();

        visContainerObject = visContainer.CreateVisContainer(title);
        visContainerObject.transform.SetParent(container.transform);

        visContainer.SetAxisOffsets(xyzOffset);
        visContainer.SetAxisTickNumber(xyzTicks);
        visContainer.SetColorScheme(colorScheme);

        return visContainerObject;
    }

    /// <summary>
    /// Method to append data to the visualization. Data is stored in a list of dictionaries
    /// with the attribute name as key and the values as double array.
    /// Method checks if the data is correct and if the number of attributes and values match.
    /// </summary>
    /// <param name="values"></param>
    public virtual void AppendData(Dictionary<string, double[]> values)
    {

        if (dataSets == null)
        {
            dataSets = new List<Dictionary<string, double[]>>();
            numberOfValues = new List<int>();
        }

        // Preprocess Data
        if (values == null || values.Count < 1)
        {
            Debug.LogError("Appended Data is incorrect (insufficient dimensions, missing values, ...)");
            return;
        }
        dimensions = values.Count;
        //Debug.Log("DIM  " + dimensions);

        //Check other data sets if they have the same amount of attributes
        if (dataSets.Count > 0)
        {
            if (values.Count != dimensions)
            {
                Debug.LogError("Number of data attributes do not match with other loaded datasets (Missing Attributes!)");
                return;
            }
        }


        dataSets.Add(values);
        numberOfValues.Add(values.ElementAt(0).Value.Length);

        // Test if every attribute has the same amount of values
        for (int dim = 0; dim < dimensions; dim++)
        {
            var currentValueCount = values.ElementAt(dim).Value.Length;
            //return currentValueCount;

            if (currentValueCount <= 0 || currentValueCount - numberOfValues[numberOfValues.Count - 1] != 0)
            {
                Debug.LogError("Number of data values do not match (Missing Values!)");
                return;
            }
        }


    }

    /// <summary>
    /// Returns the appended data as a list of dictionaries.
    /// </summary>
    /// <returns></returns>
    public virtual List<Dictionary<string, double[]>> GetAppendedData()
    {
        //Debug.Log("Look  " + GetAppendedData());
        return dataSets;//dataSets
    }


    public virtual void Recalculate(Dictionary<string, double[]> values) { //public virtual void Recalculate(List<Dictionary<string, double[]>> dataSets public virtual List<Dictionary<string, double[]>> Recalculate()
                                                                           //start

        numberOfValues.Add(values.ElementAt(0).Value.Length);
        //Debug.Log(values.Count);
        int asd = numberOfValues.Max();
        double[] numb = new double[values.Count];

        for (int dim = 0; dim < asd; dim++)
        {
            double sum = 0;
            int i = 0;
            foreach (var kvp in values)
            {
                double[] val = kvp.Value;
                double a = Math.Log(val.Sum(), 2) + 1;
                //Debug.Log(i);
                numb[i] = a;
                i++;
            }

        }
        /*for (int i = 0; i < 4; i++)
        {
            Debug.Log("03/06   " + string.Join(", ", dataSets[0].ElementAt(i).Value));
        }*/
        //Debug.Log("For03/06  " +  dataSets[0].ElementAt(1).Value);

        dataSets.Clear();
        Dictionary<string, double[]> valuess = new Dictionary<string, double[]>();
        int u = 0;

        //valuess.Add(values);
        //int i = 0;
        foreach (var kvp in values)
        {
            valuess.Add(kvp.Key, numb);
            //Debug.Log("Value   " + kvp.Value);
            //dataSets[kvp.Value] = numb;
            dataSets.Add(valuess);
            //Debug.Log(numb[u]);
            u++;
        }

        //dataSets.Add(values);
        //return valuess;

    }

    class Species
    {
        public Dictionary<string, double> Properties { get; set; }

        public Species()
        {
            Properties = new Dictionary<string, double>();
        }
    }




    public virtual void Visa(Dictionary<string, double[]> values)
    {
        /*numberOfValues.Add(values.ElementAt(0).Value.Length);
        int asd = numberOfValues.Max();
        int binCount = (int)(1 + Math.Log(asd, 2)) + 1;
        double[][] processedyArray = new double[asd][];   // processedyArray => [[y11, y12, y13...], [y21, y22, y23...], ...]
        
        


        for (int i = 0; i < asd; i++)                     // double[][] processedyArray = new double[yArray.Length][binCount];
        {
            processedyArray[i] = new double[binCount];
        }

        double[] processedxArray = new double[binCount];            // processedxArray => [x1, x2,...]
        double[][] xyArray = new double[asd][];           // xyArray => [[x1, y11, y12, y13...], [x2, y21, y22, y23...], ...]

        for (int i = 0; i < asd; i++)                     // double[][] xyArray = new double[xArray.Length][yArray.Length];
        {
            xyArray[i] = new double[asd + 1];
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
        }*/

        numberOfValues.Add(values.ElementAt(0).Value.Length);
        int asd = numberOfValues.Max();

        double[] xArray = new double[asd];

        //xArray = dataSets[0].values;

        var firstKey = values.Keys.First();
        var firstValue = values[firstKey];
        Array.Copy(firstValue, xArray, firstValue.Length);

        //Debug.Log(xArray[0] + "  " + xArray[1]);




    }


    public virtual List<Dictionary<string, double[]>> GetRecalculate()
    {
        return valuess;//dataSets
    }

    /// <summary>
    /// Each child class can define how the axis attribute in the visualization can be changed.
    /// </summary>
    /// <param name="axisId"></param>
    /// <param name="selectedDimension"></param>
    /// <param name="numberOfTicks"></param>
    public virtual void ChangeAxisAttribute(int axisId, int selectedDimension, int numberOfTicks)
    {
        
    }

    /// <summary>
    /// Each child class can define how the Data Marks in the visualization can be changed.
    /// </summary>
    public virtual void ChangeDataMarks()
    {

    }

    /// <summary>
    /// Method to update the grids in the visualization with the current viewing angle.
    /// </summary>
    public virtual void UpdateGrids()
    {
        // Update Grid
        if(visContainer != null) visContainer.MoveGridBasedOnViewingDirection();
    }

    //ToDo: Add your Vis to the switch statement
    /// <summary>
    /// Method returns the selected Visualization child class based on the defined types in the enum
    /// </summary>
    /// <param name="vistype"></param>
    /// <returns></returns>
    public static Vis GetSpecificVisType(Enum vistype)
    {
        switch (vistype)
        {
            default:
            case VisType.Bar://Scatterplot
                return new VisBarChart_2();//Scatterplot
        }
    }

    

}

public class xyValues
{
    public double[] xValues { get; set; }
    public double[] yValues { get; set; }

}

