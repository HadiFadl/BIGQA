using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System;

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
            Understandability
        }
        #region Wrappers

        public static List<Dictionary<string, object>> Sample(List<Dictionary<string, object>> data_units, double ratio)
        {
            int count = data_units.Count;
            int interval = Convert.ToInt32(count * ratio);

            return data_units.Where((item, index) => index % interval == 0).ToList();   
        }
        public static Dictionary<string, object> Filter(Dictionary<string, object> data_unit, List<string> data_elements)
        {
            Dictionary<string, object> new_data_unit = data_unit;

            foreach (string element in data_elements)
            {
                new_data_unit.Remove(element);
            }

            return new_data_unit;
        }

        public static double BaseMeasure(List<Dictionary<string, object>> data_units, string quality_rule, 
            Dictionary<string, object> reference_data = null, string reference_key = null)
        {

            if (reference_data != null)
                throw new NotImplementedException();

            int score = 0;
            foreach(Dictionary<string, object> unit in data_units)
            {
                string key = Block(unit, quality_rule).ToString();

                if (Evaluate(quality_rule, key))
                    score += 1;

            }

            return score / data_units.Count;
        }

        public static double DeriveMeasure(List<Dictionary<string,decimal>> base_measures, QualityDimension dimension)
        {
            switch (dimension)
            {
                case QualityDimension.Accuracy:

                    break;
                case QualityDimension.Completeness:

                    break;
                case QualityDimension.Compliance:

                    break;
                case QualityDimension.Consistency:

                    break;
                case QualityDimension.Credibility:

                    break;
                case QualityDimension.Understandability:

                    break;
                case QualityDimension.Uniqueness:

                    break;
                default:
                    throw new NotImplementedException();



            }

            return 0.0;
        }


        public static double Assess(Dictionary<string, decimal> derived_measures)
        {
            return Convert.ToDouble(derived_measures.Values.Average());
        }
        #endregion

        #region Enhancers
        private static List<Dictionary<string, object>> Sort(List<Dictionary<string, object>> list_of_units, string sorting_key)
        {
            return list_of_units.OrderBy(x => x[sorting_key]).ToList();
        }
        private static object Block(Dictionary<string, object> data_unit, string quality_rule)
        {
            string element = GetElement(quality_rule);
            return data_unit[element];
        }
        private static string GetElement(string quality_rule)
        {
            return Regex.Match(quality_rule, @"\[(.*?)\]").Value;
        }
        private static bool Evaluate(string quality_rule, string value)
        {
            string new_expression = Regex.Replace(quality_rule, @"\[(.*?)\]", value);
            var result = new DataTable().Compute(new_expression, "");
            return (bool)result;
        }
        public static List<Tuple<Dictionary<string, object>, Dictionary<string, object>>> CrossApply(List<Dictionary<string, object>> list1, List<Dictionary<string, object>> list2)
        {
            var query = from lst1 in list1
            from lst2 in list2
            select new Tuple<Dictionary<string, object>,
                Dictionary<string, object>>(lst1, lst2) ;

            return query.ToList();
        }
        #endregion

    }
}
