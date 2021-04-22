using System;
using System.Collections.Generic;

namespace BIGDQ
{
    class Program
    {
        static void Main(string[] args)
        {
            double Sampling_ratio = 0.1;
            List<BaseMeasure> MeasureGroup1 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup2 = new List<BaseMeasure>();
            List<DerivedMeasure> DerivedMeasures = new List<DerivedMeasure>();

            List<Dictionary<string, object>> data_units = PreProcessing.ImportFlatFile(@"E:\StackOverflow_Users2010.csv", ";", true);

            List<Dictionary<string, object>> sample_units = DataQuality.Sample(data_units, Sampling_ratio);
            
            List<Dictionary<string, object>> filtered_units = DataQuality.Filter(sample_units, new List<string> { "Age", "CreationDate", "DisplayName", "Location", "Reputation" });

            MeasureGroup1.Add(DataQuality.BaseMeasure(filtered_units, "Age <> ''", 0.5));
            MeasureGroup1.Add(DataQuality.BaseMeasure(filtered_units, "DisplayName <> ''", 0.5));
            MeasureGroup2.Add(DataQuality.BaseMeasure(filtered_units, "Reputation <> 0", 1));
            MeasureGroup2.Add(DataQuality.BaseMeasure(filtered_units, "CreationDate <> '19000101'", 1));

            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup1, DataQuality.QualityDimension.Completeness, 0.75));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup2, DataQuality.QualityDimension.Accuracy, 0.25));

            double DQScore = DataQuality.Assess(DerivedMeasures);

            /* Writing report */
            //Confidence level 
            Console.WriteLine(string.Format("Confidence level = {0}", Sampling_ratio));
            
            //Quality rules scores
            foreach(BaseMeasure bs in MeasureGroup1)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {1}", bs.QualityRule, bs.Weight, bs.Score));
            }

            foreach (BaseMeasure bs in MeasureGroup2)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {1}", bs.QualityRule, bs.Weight, bs.Score));
            }

            //Derived Measures
            foreach(DerivedMeasure dm in DerivedMeasures)
            {
                Console.WriteLine(string.Format("Derived Measure ({0}): Weight = {1} Score = {1}", dm.Name.ToString(), dm.Weight, dm.Score));
            }

            //General Quality Score

            Console.WriteLine(string.Format("Data Quality Score = {0}", DQScore));
        }
    }
}
