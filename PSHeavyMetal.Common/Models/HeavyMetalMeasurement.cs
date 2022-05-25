﻿using PalmSens.Core.Simplified.Data;

namespace PSHeavyMetal.Common.Models
{
    public class HeavyMetalMeasurement : DataObject
    {
        /// <summary>
        /// The name of the analyte
        /// </summary>
        public string AnalyteName { get; set; }

        /// <summary>
        /// The concentration amount which is calculated based on the found peaks and the concentration method
        /// </summary>
        public double Concentration { get; set; }

        /// <summary>
        /// Gets or sets the calculation values of the concentration
        /// </summary>
        public ConcentrationMethod ConcentrationMethod { get; set; }

        /// <summary>
        /// Gets or sets the concentration unit
        /// </summary>
        public ConcentrationUnit ConcentrationUnit { get; set; }

        /// <summary>
        /// Gets or sets the configured measured which is configured in the simplified core
        /// </summary>
        public SimpleMeasurement ConfiguredMeasurement
        { get; set; }

        /// <summary>
        /// Gets or sets the descriptions which is based on the input of the user
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the methodtype which is used to determine the concentration method and unit
        /// </summary>
        public MethodType MethodType { get; set; }
    }
}