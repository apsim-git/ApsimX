using System;
using System.Collections.Generic;
using System.Text;
using Models.Core;
using Models.PMF.Functions;
using Models.PMF.Interfaces;

namespace Models.PMF.Organs
{
    /// <summary>
    /// Nodule organ
    /// </summary>
    [Serializable]
    public class Nodule : GenericOrgan, BelowGround
    {
        #region Paramater Input Classes
        /// <summary>The fixation metabolic cost</summary>
        [Link]
        IFunction FixationMetabolicCost = null;
        /// <summary>The specific nitrogenase activity</summary>
        [Link]
        IFunction SpecificNitrogenaseActivity = null;
        /// <summary>The ft</summary>
        [Link]
        IFunction FT = null;
        /// <summary>The fw</summary>
        [Link]
        IFunction FW = null;
        /// <summary>The f wlog</summary>
        [Link]
        IFunction FWlog = null;
        #endregion

        #region Class Fields
        /// <summary>The respired wt</summary>
        public double RespiredWt = 0;
        /// <summary>The property fixation demand</summary>
        public double PropFixationDemand = 0;
        /// <summary>The _ n fixed</summary>
        public double _NFixed = 0;
        #endregion

        #region Arbitrator methods
        /// <summary>Gets or sets the n fixation cost.</summary>
        /// <value>The n fixation cost.</value>
        public override double NFixationCost
        {
            get
            {
                return FixationMetabolicCost.Value;
            }
        }
        /// <summary>Sets the n allocation.</summary>
        /// <value>The n allocation.</value>
        public override BiomassAllocationType NAllocation
        {
            set
            {
                base.NAllocation = value;    // give N allocation to base first.
                _NFixed = value.Fixation;    // now get our fixation value.
            }
        }

        /// <summary>Gets the respired wt fixation.</summary>
        /// <value>The respired wt fixation.</value>
        public double RespiredWtFixation
        {
            get
            {
                return RespiredWt;
            }
        }
        /// <summary>Gets or sets the n supply.</summary>
        /// <value>The n supply.</value>
        public override BiomassSupplyType NSupply
        {
            get
            {
                BiomassSupplyType Supply = base.NSupply;   // get our base GenericOrgan to fill a supply structure first.
                if (Live != null)
                {
                    // Now add in our fixation
                    Supply.Fixation = Live.StructuralWt * SpecificNitrogenaseActivity.Value * Math.Min(FT.Value, Math.Min(FW.Value, FWlog.Value));
                }
                return Supply;
            }
        }
        /// <summary>Sets the dm allocation.</summary>
        /// <value>The dm allocation.</value>
        public override BiomassAllocationType DMAllocation
        {
            set
            //This is the DM that is consumed to fix N.  this is calculated by the arbitrator and passed to the nodule to report
            {
                base.DMAllocation = value;      // Give the allocation to our base GenericOrgan first
                RespiredWt = value.Respired;    // Now get the respired value for ourselves.
            }
        }

        /// <summary>Gets the n fixed.</summary>
        /// <value>The n fixed.</value>
        public double NFixed
        {
            get
            {
                return _NFixed;
            }
        }
        #endregion
    }
}
