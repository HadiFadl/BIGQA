using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using System.Threading;

namespace BIGDQ
{
    class Program
    {
        static double Sampling_ratio = 0.5;

        static void Main(string[] args)
        {
            Console.WriteLine($"Importing file @ {DateTime.Now}");
            List<Dictionary<string, object>> data_units = PreProcessing.ImportFlatFile(@"E:\StackOverflow_Users2010.csv", ";", true);
            Console.WriteLine($"Sampling file @ {DateTime.Now}");
            List<Dictionary<string, object>> sample_units = DataQuality.Sample(data_units, Sampling_ratio);
            Console.WriteLine();
            Console.WriteLine("Single Threaded data quality assessement");
            Console.WriteLine("----------------------------------------");
            RunSingleThreaded(sample_units);
            Console.WriteLine();
            Console.WriteLine("Sequentiel operations with parallel loops");
            Console.WriteLine("----------------------------------------");
            RunParallelLoops(sample_units);
            Console.WriteLine();
            Console.WriteLine("Run RunMultiThreaded");
            Console.WriteLine("----------------------------------------");
            RunMultiThreaded(sample_units);
            Console.WriteLine();
            Console.WriteLine("Run RunMultiThreadedParallel");
            Console.WriteLine("----------------------------------------");
            RunMultiThreadedParallel(sample_units);
        }

        public static void RunSingleThreaded(List<Dictionary<string, object>> data_units)
        {
            DateTime startdate = DateTime.Now;
            double Sampling_ratio = 0.1;
            List<BaseMeasure> MeasureGroup1 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup2 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup3 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup4 = new List<BaseMeasure>();
            List<DerivedMeasure> DerivedMeasures = new List<DerivedMeasure>();

            List<Dictionary<string, object>> filtered_units = DataQuality.Filter(data_units, new List<string> { "Age", "CreationDate", "DisplayName", "Location", "Reputation" });

            MeasureGroup1.Add(DataQuality.BaseMeasure(filtered_units, "[Age] <> ''", 0.75));
            MeasureGroup1.Add(DataQuality.BaseMeasure(filtered_units, "[DisplayName] <> ''", 0.25));
            MeasureGroup2.Add(DataQuality.BaseMeasure(filtered_units, "[Reputation] <> 0", 0.5));
            MeasureGroup2.Add(DataQuality.BaseMeasure(filtered_units, "[CreationDate] <> '19000101'", 0.5));
            MeasureGroup3.Add(DataQuality.BaseMeasure(filtered_units, "[CreationDate] NOT LIKE '2010*'", 1));
            MeasureGroup4.Add(DataQuality.BaseMeasure(filtered_units, "[DisplayName] NOT LIKE '*$*'", 1));

            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup1, DataQuality.QualityDimension.Completeness, 0.5));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup2, DataQuality.QualityDimension.Accuracy, 0.25));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup3, DataQuality.QualityDimension.Currentness, 0.125));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup4, DataQuality.QualityDimension.Understandability, 0.125));

            double DQScore = DataQuality.Assess(DerivedMeasures);

            /* Writing report */
            //Confidence level 
            Console.WriteLine(string.Format("Confidence level = {0}", Sampling_ratio));

            //Quality rules scores
            foreach (BaseMeasure bs in MeasureGroup1)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {2}", bs.QualityRule, bs.Weight, bs.Score));
            }

            foreach (BaseMeasure bs in MeasureGroup2)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {2}", bs.QualityRule, bs.Weight, bs.Score));
            }

            //Derived Measures
            foreach (DerivedMeasure dm in DerivedMeasures)
            {
                Console.WriteLine(string.Format("Derived Measure ({0}): Weight = {1} Score = {2}", dm.Name.ToString(), dm.Weight, dm.Score));
            }

            //General Quality Score

            Console.WriteLine(string.Format("Data Quality Score = {0}", DQScore));

            Console.WriteLine(string.Format("Elapsed Time: {0} seconds", DateAndTime.DateDiff(DateInterval.Second, startdate, DateTime.Now)));
        }

        public static void RunParallelLoops(List<Dictionary<string, object>> data_units)
        {
            DateTime startdate = DateTime.Now;
            double Sampling_ratio = 0.1;
            List<BaseMeasure> MeasureGroup1 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup2 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup3 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup4 = new List<BaseMeasure>();

            List<DerivedMeasure> DerivedMeasures = new List<DerivedMeasure>();


            List<Dictionary<string, object>> filtered_units = DataQuality.ParallelFilter(data_units, new List<string> { "Age", "CreationDate", "DisplayName", "Location", "Reputation" });

            MeasureGroup1.Add(DataQuality.ParallelBaseMeasure(filtered_units, "[Age] <> ''", 0.75));
            MeasureGroup1.Add(DataQuality.ParallelBaseMeasure(filtered_units, "[DisplayName] <> ''", 0.25));
            MeasureGroup2.Add(DataQuality.ParallelBaseMeasure(filtered_units, "[Reputation] <> 0", 0.5));
            MeasureGroup2.Add(DataQuality.ParallelBaseMeasure(filtered_units, "[CreationDate] <> '19000101'", 0.5));
            MeasureGroup3.Add(DataQuality.BaseMeasure(filtered_units, "[CreationDate] NOT LIKE '2010*'", 1));
            MeasureGroup4.Add(DataQuality.BaseMeasure(filtered_units, "[DisplayName] NOT LIKE '*$*'", 1));

            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup1, DataQuality.QualityDimension.Completeness, 0.5));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup2, DataQuality.QualityDimension.Accuracy, 0.25));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup3, DataQuality.QualityDimension.Currentness, 0.125));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup4, DataQuality.QualityDimension.Understandability, 0.125));


            double DQScore = DataQuality.Assess(DerivedMeasures);

            /* Writing report */
            //Confidence level 
            Console.WriteLine(string.Format("Confidence level = {0}", Sampling_ratio));

            //Quality rules scores
            foreach (BaseMeasure bs in MeasureGroup1)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {2}", bs.QualityRule, bs.Weight, bs.Score));
            }

            foreach (BaseMeasure bs in MeasureGroup2)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {2}", bs.QualityRule, bs.Weight, bs.Score));
            }

            //Derived Measures
            foreach (DerivedMeasure dm in DerivedMeasures)
            {
                Console.WriteLine(string.Format("Derived Measure ({0}): Weight = {1} Score = {2}", dm.Name.ToString(), dm.Weight, dm.Score));
            }

            //General Quality Score

            Console.WriteLine(string.Format("Data Quality Score = {0}", DQScore));

            Console.WriteLine(string.Format("Elapsed Time: {0} seconds", DateAndTime.DateDiff(DateInterval.Second, startdate, DateTime.Now)));
        }

        public static void RunMultiThreaded(List<Dictionary<string, object>> data_units)
        {
            DateTime startdate = DateTime.Now;
            double Sampling_ratio = 0.1;
            List<BaseMeasure> MeasureGroup1 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup2 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup3 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup4 = new List<BaseMeasure>();
            List<DerivedMeasure> DerivedMeasures = new List<DerivedMeasure>();



            List<Dictionary<string, object>> filtered_units = DataQuality.ParallelFilter(data_units, new List<string> { "Age", "CreationDate", "DisplayName", "Location", "Reputation" });


            int counter = 0;
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup1.Add(DataQuality.BaseMeasure(filtered_units, "[Age] <> ''", 0.75)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup1.Add(DataQuality.BaseMeasure(filtered_units, "[DisplayName] <> ''", 0.25)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup2.Add(DataQuality.BaseMeasure(filtered_units, "[Reputation] <> 0", 0.5)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup2.Add(DataQuality.BaseMeasure(filtered_units, "[CreationDate] <> '19000101'", 0.5)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup3.Add(DataQuality.BaseMeasure(filtered_units, "[CreationDate] NOT LIKE '2010*'", 1)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup4.Add(DataQuality.BaseMeasure(filtered_units, "[DisplayName] NOT LIKE '*$*'", 1)); Interlocked.Increment(ref counter); });


            while (counter < 6)
            {

            }

            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup1, DataQuality.QualityDimension.Completeness, 0.5));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup2, DataQuality.QualityDimension.Accuracy, 0.25));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup3, DataQuality.QualityDimension.Currentness, 0.125));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup4, DataQuality.QualityDimension.Understandability, 0.125));


            double DQScore = DataQuality.Assess(DerivedMeasures);

            /* Writing report */
            //Confidence level 
            Console.WriteLine(string.Format("Confidence level = {0}", Sampling_ratio));

            //Quality rules scores
            foreach (BaseMeasure bs in MeasureGroup1)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {2}", bs.QualityRule, bs.Weight, bs.Score));
            }

            foreach (BaseMeasure bs in MeasureGroup2)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {2}", bs.QualityRule, bs.Weight, bs.Score));
            }

            //Derived Measures
            foreach (DerivedMeasure dm in DerivedMeasures)
            {
                Console.WriteLine(string.Format("Derived Measure ({0}): Weight = {1} Score = {2}", dm.Name.ToString(), dm.Weight, dm.Score));
            }

            //General Quality Score

            Console.WriteLine(string.Format("Data Quality Score = {0}", DQScore));

            Console.WriteLine(string.Format("Elapsed Time: {0} seconds", DateAndTime.DateDiff(DateInterval.Second, startdate, DateTime.Now)));
        }

        public static void RunMultiThreadedParallel(List<Dictionary<string, object>> data_units)
        {
            DateTime startdate = DateTime.Now;
            double Sampling_ratio = 0.1;
            List<BaseMeasure> MeasureGroup1 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup2 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup3 = new List<BaseMeasure>();
            List<BaseMeasure> MeasureGroup4 = new List<BaseMeasure>();
            List<DerivedMeasure> DerivedMeasures = new List<DerivedMeasure>();



            List<Dictionary<string, object>> filtered_units = DataQuality.ParallelFilter(data_units, new List<string> { "Age", "CreationDate", "DisplayName", "Location", "Reputation" });


            int counter = 0;
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup1.Add(DataQuality.ParallelBaseMeasure(filtered_units, "[Age] <> ''", 0.75)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup1.Add(DataQuality.ParallelBaseMeasure(filtered_units, "[DisplayName] <> ''", 0.25)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup2.Add(DataQuality.ParallelBaseMeasure(filtered_units, "[Reputation] <> 0", 0.5)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup2.Add(DataQuality.ParallelBaseMeasure(filtered_units, "[CreationDate] <> '19000101'", 0.5)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup3.Add(DataQuality.BaseMeasure(filtered_units, "[CreationDate] NOT LIKE '2010*'", 1)); Interlocked.Increment(ref counter); });
            ThreadPool.QueueUserWorkItem(state => { MeasureGroup4.Add(DataQuality.BaseMeasure(filtered_units, "[DisplayName] NOT LIKE '*$*'", 1)); Interlocked.Increment(ref counter); });


            while (counter < 4)
            {

            }

            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup1, DataQuality.QualityDimension.Completeness, 0.5));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup2, DataQuality.QualityDimension.Accuracy, 0.25));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup3, DataQuality.QualityDimension.Currentness, 0.125));
            DerivedMeasures.Add(DataQuality.DeriveMeasure(MeasureGroup4, DataQuality.QualityDimension.Understandability, 0.125));


            double DQScore = DataQuality.Assess(DerivedMeasures);

            /* Writing report */
            //Confidence level 
            Console.WriteLine(string.Format("Confidence level = {0}", Sampling_ratio));

            //Quality rules scores
            foreach (BaseMeasure bs in MeasureGroup1)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {2}", bs.QualityRule, bs.Weight, bs.Score));
            }

            foreach (BaseMeasure bs in MeasureGroup2)
            {
                Console.WriteLine(string.Format("Base Measure ({0}): Weight = {1} Score = {2}", bs.QualityRule, bs.Weight, bs.Score));
            }

            //Derived Measures
            foreach (DerivedMeasure dm in DerivedMeasures)
            {
                Console.WriteLine(string.Format("Derived Measure ({0}): Weight = {1} Score = {2}", dm.Name.ToString(), dm.Weight, dm.Score));
            }

            //General Quality Score

            Console.WriteLine(string.Format("Data Quality Score = {0}", DQScore));

            Console.WriteLine(string.Format("Elapsed Time: {0} seconds", DateAndTime.DateDiff(DateInterval.Second, startdate, DateTime.Now)));
        }
    }
}
