﻿using PalmSens;
using PalmSens.Core.Simplified.Data;
using PSHeavyMetal.Common.Models;
using System;
using System.Threading.Tasks;
using static PalmSens.Core.Simplified.PSCommSimple;

namespace PSHeavyMetal.Core.Services
{
    public interface IMeasurementService
    {
        public event SimpleCurveStartReceivingDataHandler DataReceived;

        public event EventHandler MeasurementEnded;

        public event EventHandler MeasurementStarted;

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        /// <summary>
        /// Calculations based on the configuration of the caluclation method
        /// </summary>
        public void CalculateConcentration();

        public HeavyMetalMeasurement CreateMeasurement(string name, string description);

        public Method LoadMethod(string filename);

        public void SetCalculationMethod(MethodType method);

        public Task<SimpleMeasurement> StartMeasurement(Method method);
    }
}