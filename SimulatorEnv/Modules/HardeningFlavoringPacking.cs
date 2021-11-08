using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common;
using System;
using System.Collections.Generic;
using System.Text;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.SimulatorEnv.Calculations;

namespace ABB.InSecTT.SimulatorEnv.Modules
{
    internal class HardeningFlavoringPacking : TankModule
    {
        private double m_coolerTemperature;
        private string m_packagingType;
        private double m_tankBaseArea;
        private double m_fillVolume;
        private double m_packageArea;
        private double m_coneRadius = 0.03; //generic approximate values for an ice cream cone
        private double m_coneHeight = 0.15;
        private double m_hDPEHeight = 0.05; //value for a 750 ml HDPE package
        private double m_hDPEBaseArea = 0.015; //value for a 750 ml HDPE package
        private IParameter m_finishBatch;        

        private IParameter m_startFlavoring;
        private IParameter m_startPackaging;
        private IParameter m_startHardening;
        private double m_heatTransferCoef = 1.1;
        private double m_packagingConstant;
        private double m_processTime;
        private int m_producedPackages;

        public bool StartFlavoring
        {
            get { return m_startFlavoring.DigitalValue; }
            private set { m_startFlavoring.DigitalValue = value; }
        }
        public bool StartHardening
        {
            get { return m_startHardening.DigitalValue; }
            private set { m_startHardening.DigitalValue = value; }
        }
        public bool StartPackaging
        {
            get { return m_startPackaging.DigitalValue; }
            private set { m_startPackaging.DigitalValue = value; }
        }

        public bool FinishBatch
        {
            get { return m_finishBatch.DigitalValue; }
            private set { m_finishBatch.DigitalValue = value; }
        }
        public HardeningFlavoringPacking(string name, double baseArea, double outletArea, double height, string packagingType, double coolerTemperature) : base(name, baseArea, outletArea, height)
        {
            m_coolerTemperature = coolerTemperature;
            m_packagingType = packagingType;
            m_tankBaseArea = baseArea;
        }
        public override void Execute(int mils, MixProperties state)
        {
            if (StartFlavoring)
            {
                SolidFlavoring(mils, state);

            }

            if (StartPackaging)
            {
                Packaging(mils);
            }
            else
            {
                base.Execute(mils, state);
            }

            if (StartHardening)
            {
                StaticFreezing(mils, state);
            }

            if (FinishBatch)
            {
                FinalPackaging(mils, state);
            }

            /*
            if (CurrentLevel > 0.15)
            {
                double weight = (double)state.Density * CurrentLevel * m_baseArea;

                if (m_fillVolume != 0)
                    weight = (double)(state.Density * m_fillVolume * m_packageArea);

                //Uses Newton Law of Cooling T(t) = Tenv + (T(0) -Tenv)e^(-tAh/mc) m/c is simplified to h as it is constant.
                TankTemperature = state.AmbientTemperature + (TankTemperature - state.AmbientTemperature) * Math.Pow(Math.E, -1 * mils / 1000.0 * state.ThermalConductivity / weight);                 
            }
            */

        }
        /// <summary>
        /// This method Flavors the mix using solid objects such as nuts. It will use the calculations class to calculate the new density and mass of the mix. 
        /// </summary>
        private void SolidFlavoring(int mils, MixProperties state)
        {
            double volume = CurrentLevel * m_tankBaseArea;
            double mass = volume * state.Density;
            double timeIncrement = mils / 1000;

            mass *= 1 +  state.SolidFlavoring / 100 *  state.SolidFlavoringDensity * timeIncrement; //change in mass due to addition of nuts 
            volume *= 1 + state.SolidFlavoring / 100 * timeIncrement; //change in volume due to addition of nuts
             
            state.Density =  mass / volume;
            CurrentLevel = (double)volume / (double)m_tankBaseArea; //change in level after Solid flavoring is done

        }

        /// <summary>
        /// This method freezes the mix once more statically, if you don't want soft ice cream 
        /// </summary>
        private void StaticFreezing(int mils, MixProperties state)
        {
            var timeInc = mils / 1000;
            m_processTime += timeInc; //process state time variable

            double dTemperature;
            dTemperature = TankTemperature - m_coolerTemperature;
            if(dTemperature > 0)
            {
                TankTemperature = m_coolerTemperature + dTemperature * Math.Pow(Math.E, (double)(-1 * timeInc / (m_heatTransferCoef * m_packagingConstant) / ( state.Density * m_fillVolume) * m_packageArea)); //Newton's Law of Cooling 
            }
            if (dTemperature == 0)
            {
                m_processTime = 0;
            }
        }

        /// <summary>
        /// This method packages the mix into the containers you want to use, either HDPE or cone ice cream.
        /// </summary>
        private void Packaging(int mils)
        {
            switch (m_packagingType)
            {
                case "cone":
                    m_fillVolume = (1 / 3) * Math.PI * Math.Pow(m_coneRadius, 2) * m_coneHeight + (2 / 3) * Math.PI * Math.Pow(m_coneRadius, 3);
                    //sum of volume of hemisphere and cone to calculate the amount of volume that will be filled in the cone 
                    m_packagingConstant = 0.55; //assumed to have worse thermal transfer than HDPE 
                    m_packageArea = Math.PI * m_coneRadius*(m_coneRadius + Math.Pow(Math.Pow(m_coneRadius, 2) + Math.Pow(m_coneRadius, 2), 0.5));
                    break;
                case "HDPE": //fills in a regular plastic container
                    m_fillVolume = 0.75 * m_hDPEBaseArea * m_hDPEHeight;
                    m_packagingConstant = 0.48; //Value from https://www.substech.com/dokuwiki/doku.php?id=thermoplastic_high_density_polyethylene_hdpe
                    m_packageArea = m_hDPEBaseArea;
                    break;
                default:
                    m_fillVolume = 0.75 * m_hDPEBaseArea * m_hDPEHeight;
                    m_packagingConstant = 0.48; //Value from https://www.substech.com/dokuwiki/doku.php?id=thermoplastic_high_density_polyethylene_hdpe
                    m_packageArea = m_hDPEBaseArea;
                    //defaults to HDPE packaged ice cream
                    break;
            }
            OutletFlow = (double)m_fillVolume * 10;
            double packages = m_currLevel.AnalogValue * (double)m_tankBaseArea / (double)m_fillVolume;
            m_producedPackages = (int)packages;

            double timeIncrement = mils / 1000.0;
            CurrentLevel += timeIncrement * (InletFlow - OutletFlow) / m_baseArea;
            if (CurrentLevel < 0)
            {
                // Cannot be negative level...
                CurrentLevel = 0;
            }

            LevelPercent = CurrentLevel / Height * 100;
        }

        private void FinalPackaging(int mils, MixProperties state)
        {
            m_processTime += (mils / 1000.0); //process state time variable
            if(m_processTime == 5)
            {
                string produce = string.Format($"{0} {1} packages of {2} ice cream with {3} have been produced", m_producedPackages, m_packagingType, state.LiquidFlavor, state.SolidFlavor);
                Console.WriteLine(produce);
                SimulationEventSource.Log.Write("BatchResult", produce);
            }

        }

        public override void TieParameters(IParameterDataBase parameters)
        {
            base.TieParameters(parameters);
            string startFlavoringParam = string.Format("{0}/StartFlavoring", this.Name);
            string startPackagingParam = string.Format("{0}/StartPackaging", this.Name);
            string startHardeningParam = string.Format("{0}/StartHardening", this.Name);
            //string mixTemperatureParam = string.Format("{0}/MixTemperature", this.Name);
            string finishBatchParam = string.Format("{0}/FinishBatch", this.Name);

            m_finishBatch = parameters.GetParameter(finishBatchParam);
            //m_mixTemperature = parameters.GetParameter(mixTemperatureParam);
            m_startFlavoring = parameters.GetParameter(startFlavoringParam);
            m_startPackaging = parameters.GetParameter(startPackagingParam);
            m_startHardening = parameters.GetParameter(startHardeningParam);
        }
    }
}
