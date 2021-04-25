using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Threading;

namespace BIGDQ
{
    public static class DataQuality
    {
        public enum QualityDimension
        {
            Completeness,
            Uniqueness,
            Accuracy,
            Consistency,
            Credibility,
            Compliance,
            Understandability,
            Currentness
        }

        #region Wrappers

        #region Parallel
        public static Dictionary<string, object> ParallelFilter(Dictionary<string, object> data_unit, List<string> data_elements)
        {
            Dictionary<string, object> new_data_unit = new Dictionary<string, object>();

            Parallel.ForEach(data_unit.Keys, key =>
            {
                if (data_elements.Contains(key))
                {
                    new_data_unit.Add(key, data_unit[key]);
                }
            });

            return new_data_unit;
        }

        public static BaseMeasure ParallelBaseMeasure(List<Dictionary<string, object>> data_units, string quality_rule, double rule_weight,
            Dictionary<string, object> reference_data = null, string reference_key = null)
        {
            try { 
            if (rule_weight <= 0 || rule_weight > 1)
                throw new Exception("weight value must be between 0 and 1");

            if (reference_data != null)
                throw new NotImplementedException();

            int score = 0;

            Parallel.ForEach(data_units, unit =>
            {
                string element = "";
                string key = Block(unit, quality_rule,ref element).ToString();

                if (key != "" && Evaluate(quality_rule, element, key)){ 
                    Interlocked.Increment(ref score); 
                }

            });
                return new BaseMeasure(quality_rule, rule_weight, score / data_units.Count);

            }
            catch(Exception ex)
            {
                throw ex;
            }

            
        }

        public static List<Dictionary<string, object>> ParallelFilter(List<Dictionary<string, object>> data_units, List<string> data_elements)
        {
            List<Dictionary<string, object>> new_data_units = new List<Dictionary<string, object>>();

            Parallel.ForEach(data_units, unit =>
           {
               new_data_units.Add(Filter(unit, data_elements));
           });

            return new_data_units;
        }

        #endregion

        public static List<Dictionary<string, object>> Sample(List<Dictionary<string, object>> data_units, double ratio)
        {
            if (ratio == 1)
                return data_units;

            if (ratio == 0)
                return new List<Dictionary<string, object>>();

            if (ratio < 0 || ratio > 1)
                throw new Exception("Ratio must be between 0 and 1");

            int count = data_units.Count;
            int interval = Convert.ToInt32(1 / ratio);

            return data_units.Where((item, index) => index % interval == 0).ToList();
        }
        public static Dictionary<string, object> Filter(Dictionary<string, object> data_unit, List<string> data_elements)
        {
            Dictionary<string, object> new_data_unit = new Dictionary<string, object>(); ;

            foreach(string key in data_unit.Keys)
            {
                if (data_elements.Contains(key))
                {
                    new_data_unit.Add(key, data_unit[key]);
                }
            }
           return new_data_unit;
        }
        public static List<Dictionary<string, object>> Filter(List<Dictionary<string, object>> data_units, List<string> data_elements)
        {
            List<Dictionary<string, object>> new_data_units = new List<Dictionary<string, object>>();

            foreach (Dictionary<string, object> unit in data_units)
            {
                new_data_units.Add(Filter(unit,data_elements));
            }

            return new_data_units;
        }
        public static BaseMeasure BaseMeasure(List<Dictionary<string, object>> data_units, string quality_rule, double rule_weight,
            Dictionary<string, object> reference_data = null, string reference_key = null)
        {

            if (rule_weight <= 0 || rule_weight > 1)
                throw new Exception("weight value must be between 0 and 1");

            if (reference_data != null)
                throw new NotImplementedException();

            double score = 0;
            foreach (Dictionary<string, object> unit in data_units)
            {
                string element = "";
                string key = Block(unit, quality_rule, ref element).ToString();

                if (key != "" && Evaluate(quality_rule, element, key))
                    score += 1;

            }

            return new BaseMeasure(quality_rule, rule_weight, score / data_units.Count);
        }
        public static DerivedMeasure DeriveMeasure(List<BaseMeasure> base_measures, QualityDimension dimension, double dimension_weight)
        {

            double score = base_measures.Sum(x => x.Score * x.Weight);

            return new DerivedMeasure(dimension, dimension_weight, score);

        }
        public static double Assess(List<DerivedMeasure> derived_measures)
        {
            return derived_measures.Sum(x => x.Score * x.Weight);
        }

        #endregion

        #region Enhancers
        private static List<Dictionary<string, object>> Sort(List<Dictionary<string, object>> list_of_units, string sorting_key)
        {
            return list_of_units.OrderBy(x => x[sorting_key]).ToList();
        }
        private static object Block(Dictionary<string, object> data_unit, string quality_rule, ref string element)
        {
            if (data_unit == null)
                return "";

            try
            {
                element = GetElement(quality_rule);
                return data_unit[element.TrimStart('[').TrimEnd(']')];
            }catch
            {
                return "";
            }

        }
        private static string GetElement(string quality_rule)
        {
            return Regex.Match(quality_rule, @"\[(.*?)\]").Value;
        }
        private static bool Evaluate(string quality_rule, string element,  string value)
        {
            string new_expression = quality_rule.Replace(element,"'" +  value.Replace("'","") + "'");
            var result = new DataTable().Compute(new_expression, "");
            return (bool)result;
        }
        public static List<Tuple<Dictionary<string, object>, Dictionary<string, object>>> CrossApply(List<Dictionary<string, object>> list1, List<Dictionary<string, object>> list2)
        {
            var query = from lst1 in list1
                        from lst2 in list2
                        select new Tuple<Dictionary<string, object>,
                            Dictionary<string, object>>(lst1, lst2);

            return query.ToList();
        }
        #endregion

    }
    public class BaseMeasure
    {
        public string QualityRule { get; set; }
        public double Weight { get; set; }
        public double Score { get; set; }

        public BaseMeasure(string rule, double weight, double score)
        {
            QualityRule = rule;

            if (weight <= 0 || weight > 1)
                throw new Exception("weight value must be between 0 and 1");

            Weight = weight;
            Score = score;
        }

    }
    public class DerivedMeasure
    {
        public DataQuality.QualityDimension Name { get; set; }
        public double Weight { get; set; }
        public double Score { get; set; }
        public DerivedMeasure(DataQuality.QualityDimension name, double weight, double score)
        {
            Name = name;

            if (weight <= 0 || weight > 1)
                throw new Exception("weight value must be between 0 and 1");

            Weight = weight;
            Score = score;
        }
    }
    public static class PreProcessing
    {
        public static List<Dictionary<string, object>> ConvertDataTableToDictionaryList(DataTable source)
        {
            return source.AsEnumerable().Select(
            row => source.Columns.Cast<DataColumn>().ToDictionary(
            column => column.ColumnName,    // Key
            column => row[column]  // Value
                )
            ).ToList();
        }
        public static List<Dictionary<string, object>> ImportFlatFile(string path, string delimiter, bool qualified)
        {
            string[] headers;
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            TextFieldParser tfpTxtParser = new TextFieldParser(path, System.Text.Encoding.Unicode);
            tfpTxtParser.TextFieldType = FieldType.Delimited;
            tfpTxtParser.SetDelimiters(delimiter);
            tfpTxtParser.HasFieldsEnclosedInQuotes = qualified;

            headers = tfpTxtParser.ReadFields();

            while (!tfpTxtParser.EndOfData)
            {
                try
                {
                    result.Add(ConvertArrayToDictionaryList(headers, tfpTxtParser.ReadFields()));
                }
                catch
                {

                }
                
            }

            return result;
        }
        private static Dictionary<string,object> ConvertArrayToDictionaryList(string[] header, string[] values)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            for (int i = 0; i < values.Length; i++)
            {
                result.Add(header[i], values[i]);
            }

            return result;
        }
    }
}