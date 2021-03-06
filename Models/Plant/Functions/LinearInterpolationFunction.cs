using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using System.Collections;
using Models.Core;

namespace Models.PMF.Functions
{
    /// <summary>
    /// Linear interpolation function
    /// </summary>
    [Serializable]
    [Description("A value is returned via linear interpolation of a given set of XY pairs")]
    public class LinearInterpolationFunction : Model, IFunction, IFunctionArray
    {
        /// <summary>The ys are all the same</summary>
        private bool YsAreAllTheSame = false;
        /// <summary>Gets or sets the xy pairs.</summary>
        /// <value>The xy pairs.</value>
        public XYPairs XYPairs { get; set; }
        /// <summary>The x property</summary>
        public string XProperty = "";

        /// <summary>Called when [loaded].</summary>
        [EventSubscribe("Loaded")]
        private void OnLoaded()
        {
            if (XYPairs != null)
            {
                for (int i = 1; i < XYPairs.Y.Length; i++)
                    if (XYPairs.Y[i] != XYPairs.Y[i - 1])
                    {
                        YsAreAllTheSame = false;
                        return;
                    }

                // If we get this far then the Y values must all be the same.
                YsAreAllTheSame = XYPairs.Y.Length > 0;
            }
        }

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        /// <exception cref="System.Exception">Cannot find value for  + Name +  XProperty:  + XProperty</exception>
        public double Value
        {
            get
            {
                // Shortcut exit when the Y values are all the same. Runs quicker.
                if (YsAreAllTheSame)
                    return XYPairs.Y[0];

                string PropertyName = XProperty;
                object v = Apsim.Get(this, PropertyName);
                if (v == null)
                    throw new Exception("Cannot find value for " + Name + " XProperty: " + XProperty);
                double XValue = (double) v;
                return XYPairs.ValueIndexed(XValue);
            }
        }

        /// <summary>Values for x.</summary>
        /// <param name="XValue">The x value.</param>
        /// <returns></returns>
        public double ValueForX(double XValue)
        {
            return XYPairs.ValueIndexed(XValue);
        }

        /// <summary>Gets the values.</summary>
        /// <value>The values.</value>
        /// <exception cref="System.Exception">Cannot find value for  + Name +  XProperty:  + XProperty</exception>
        public double[] Values
        {
            get
            {
                string PropertyName = XProperty;

                double[] v = (double[])Apsim.Get(this, XProperty);
                if (v == null)
                    throw new Exception("Cannot find value for " + Name + " XProperty: " + XProperty);
                if (v is Array)
                {
                    double[] ReturnValues = new double[v.Length];
                    for (int i = 0; i < v.Length; i++)
                        ReturnValues[i] = XYPairs.ValueIndexed(v[i]);
                    return ReturnValues;
                }
                else
                {
                    double XValue = Convert.ToDouble(v);
                    return new double[1] { XYPairs.ValueIndexed(XValue) };
                }
            }
        }

    }

}