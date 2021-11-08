using System;
using System.Globalization;
using System.Xml;

namespace ABB.InSecTT.SimulatorEnv.Calculations
{
    public class MixProperties
    {
        private double m_msnf;
        private double m_totalSolids;
        private double m_fat;
        private double m_sucrose;
        private double m_stabilizers;

        private double m_density;
        private double m_viscosity;
        private double  m_temperature;
        private double m_particleSize;
        private readonly double m_thermalConductivity = 0.6;
        private double m_liquidFlavoring;
        private double m_solidFlavoring;
        private double m_solidFlavoringDensity;
        private string m_solidFlavor;
        private string m_liquidFlavor;
        private double m_pasteurUnits;

        public double Density
        {
            get { return m_density; }
            set { m_density = value; }
        }
        public double ThermalConductivity
        {
            get { return m_thermalConductivity; }
        }
        public double ParticleSize
        {
            get { return m_particleSize; }
            set { m_particleSize = value; }
        }

        public double AmbientTemperature
        { 
            get { return m_temperature; }
            set { m_temperature = value; }
        }

        public double Viscosity
        {
            get { return m_viscosity; }
            set { m_viscosity = value; }
        }
        public double LiquidFlavoring
        {
            get { return m_liquidFlavoring; }
            private set { m_liquidFlavoring = value; }
        }
        public double SolidFlavoring
        {
            get { return m_solidFlavoring; }
        }
        public double SolidFlavoringDensity
        {
            get { return m_solidFlavoringDensity; }
        }
        public string SolidFlavor
        {
            get { return m_solidFlavor; }
        }
        public string LiquidFlavor
        {
            get { return m_liquidFlavor; }
        }
        public double PasteurUnits
        {
            get { return m_pasteurUnits; }
            set { m_pasteurUnits = value; }
        }


        public MixProperties()
        {
            // Using some simple assumptions:
            InitializeProperties(0, 0, 0, 0, 0, 0, 0, 100, 0, 0);
        }



        /// <summary>
        /// Initializes an object of the ice cream mix as it would be after mixing.
        /// </summary>
        /// <param name="fileName"></param>
        public MixProperties(string fileName)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileName);
            XmlNodeList recipe = xDoc.LastChild.ChildNodes;

            double.TryParse(recipe[0].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double butter);
            double.TryParse(recipe[0].Attributes["fat"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double fatpercent);
            double.TryParse(recipe[0].Attributes["Solids"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double butterSnfPercentage);
            double.TryParse(recipe[1].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double skimMilkPowder);
            double.TryParse(recipe[1].Attributes["Solids"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double sMPSnfPercentage);
            double.TryParse(recipe[2].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double sucrose);
            double.TryParse(recipe[3].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double stabilizers);
            double.TryParse(recipe[4].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double water);
            double.TryParse(recipe[5].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double solidFlavoring);
            double.TryParse(recipe[5].Attributes["Density"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double solidFlavoringDensity);
            m_solidFlavor = recipe[5].Attributes["Flavor"].Value;
            double.TryParse(recipe[6].InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double liquidFlavoring);
            m_liquidFlavor = recipe[5].Attributes["Flavor"].Value;

            var solids = butter * butterSnfPercentage / 100 + skimMilkPowder * sMPSnfPercentage / 100;
            InitializeProperties(butter, butter * fatpercent, sucrose, stabilizers, solids, liquidFlavoring, solidFlavoring, water, solidFlavoringDensity, skimMilkPowder);
        }


        private void InitializeProperties(double butter, double fat, double sucrose, double stabilizers, double totalSolids,  double liqFlavuring, double solidFlavoring, double water, double solidFlavoringDensity, double skimMilkPowder)
        {
            m_fat = fat;
            m_sucrose = sucrose;
            m_stabilizers = stabilizers;
            m_msnf = totalSolids;
            m_totalSolids = m_msnf;
            m_liquidFlavoring = liqFlavuring;
            m_solidFlavoring = solidFlavoring;
            m_solidFlavoringDensity = solidFlavoringDensity;
            //density calculation from https://www.ehow.co.uk/how_8500299_calculate-density-ice-cream-mix.html in kg/m^3
            Density = 1000 / (m_fat / (100 * 1.07527) + ((m_totalSolids / 100 - m_fat / 100) * 0.6329) + (water / 100));

            AmbientTemperature = 277; //Assuming about 5 degrees Celsius in factory

            //TODO find proper initial viscosity value, currently middle of a range given by https://www.journalofdairyscience.org/article/S0022-0302(94)77163-0/pdf 
            Viscosity = 0.2; // in Pa*s

            if (100 != water + butter + sucrose + stabilizers + skimMilkPowder + liqFlavuring + solidFlavoring) //Simple check to see if percentages add up
            {
                Console.WriteLine("Incorrect recipe used");
                throw new ArgumentException("Incorrect recipe used");
            }

        }


        /*
         * Could be used if more than one batch is to be ran at the same time
         * 
        public MixProperties(MixProperties old, decimal amount) //only used by SplitBatch
        {
            this.m_fat = old.m_fat;
            this.m_sucrose = old.m_sucrose;
            this.m_stabilizers = old.m_stabilizers;
            this.m_msnf = old.m_msnf;


            this.m_amount = amount;
            old.m_amount -= amount;
        }

        public MixProperties SplitBatch(decimal amount)
        {
            return new MixProperties(this, amount);
        }

        public MixProperties MergeBatch(MixProperties other)
        {
            this.m_fat         = (this.m_fat * this.m_amount          + other.m_fat * other.m_amount)           / this.m_amount + other.m_amount;
            this.m_msnf        = (this.m_msnf * this.m_amount         + other.m_msnf * other.m_amount)          / this.m_amount + other.m_amount;
            this.m_stabilizers = (this.m_stabilizers * this.m_amount  + other.m_stabilizers * other.m_amount)   / this.m_amount + other.m_amount;
            this.m_sucrose     = (this.m_sucrose * this.m_amount      + other.m_sucrose * other.m_amount)       / this.m_amount + other.m_amount;
            this.m_viscosity   = (this.m_viscosity * this.m_amount    + other.m_viscosity * other.m_amount)     / this.m_amount + other.m_amount;
            this.Density       = (this.m_density * this.m_amount      + other.m_density * other.m_amount)       / this.m_amount + other.m_amount;

            this.m_amount =+ other.m_amount;

            return this;
        }
        */
    }
}
