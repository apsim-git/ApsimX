﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Models.Core;

namespace Models.PMF.Functions
{
    /// <summary>
    /// Starting with the first child function, recursively divide by the values of the subsequent child functions
    /// </summary>
    /// \pre All children have to contain a public function "Value"
    /// \retval Starting with the first child function, recursively divide by the values of the subsequent child functions. Return 0 if no child. The value of first child if only one child.
    [Serializable]
    [Description("Starting with the first child function, recursively divide by the values of the subsequent child functions")]
    public class DivideFunction : Model, IFunction
    {
        /// <summary>The child functions</summary>
        private List<IModel> ChildFunctions;

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public double Value
        {
            get
            {
                if (ChildFunctions == null)
                    ChildFunctions = Apsim.Children(this, typeof(IFunction));

                double returnValue = 0.0;
                if (ChildFunctions.Count > 0)
                {
                    IFunction F = ChildFunctions[0] as IFunction;
                    returnValue = F.Value;

                    if (ChildFunctions.Count > 1)
                        for (int i = 1; i < ChildFunctions.Count; i++)
                        {
                            F = ChildFunctions[i] as IFunction;
                            returnValue = returnValue / F.Value;
                        }

                }
                return returnValue;
            }
        }

    }
}