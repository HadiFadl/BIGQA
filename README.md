# BIGQA: Declarative Big Data Quality Assessment

This is a C# console application used to demonstrate our research project BIGDQ. 

*This documentation is generated using [The following script]([Generates Markdown From VisualStudio XML documentation files (github.com)](https://gist.github.com/formix/515d3d11ee7c1c252f92))*

## Class: BaseMeasure

A class used to store the information of a base measurement operation

---------------------------


## Class: DataQuality

The Class library where all BIGDQ components (Wrappers and Enhancers) are defined


#### Remarks

 All Enhancers are defined using a private access modifiers since they are only used by Wrappers 


### M:BIGDQ.Assess(derived_measures)

The implementation of the Assess wrapper component.

| Name             | Description                                                  |
| ---------------- | ------------------------------------------------------------ |
| derived_measures | *System.Collections.Generic.List{BIGDQ.DerivedMeasure}*<br>The input derived measures used to calculate the overall quality score |


#### Returns

A quality score percentage that shows the data set usability in a specific context


### M:BIGDQ.BaseMeasure(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}},System.String,System.Double,System.Collections.Generic.Dictionary{System.String,System.Object},System.String)

A sequentiel implementation of the BaseMeasure wrapper component. This function loop over each data unit in the input data set and evaluate them over the input data quality rule

| Name           | Description                                                  |
| -------------- | ------------------------------------------------------------ |
| data_units     | *Unknown type*<br>The input list of data units               |
| quality_rule   | *Unknown type*<br>The quality rule for evaluation            |
| rule_weight    | *Unknown type*<br>The quality rule weight (between 0 and 1)  |
| reference_data | *Unknown type*<br>The refernce look up data set (Not implemented yet) |
| reference_key  | *Unknown type*<br>The reference key used to join the input data set with the reference data (Not implemented yet) |


#### Returns

A BaseMeasure class that contains the measurement metadata and the measured score


### M:BIGDQ.Block(System.Collections.Generic.Dictionary{System.String,System.Object},System.String,System.String@)

An implementation of the Block enhancer. A component used to get a blocking key value from a data unit based on a quality rule

| Name         | Description                                           |
| ------------ | ----------------------------------------------------- |
| data_unit    | *Unknown type*<br>Input data unit                     |
| quality_rule | *Unknown type*<br>Input Quality rule                  |
| element      | *Unknown type*<br>The element name (output parameter) |


#### Returns

The value of the element used for blocking


### M:BIGDQ.CrossApply(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}},System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}})

A implementation of the CrossApply enhancer. A component that is used to generate all data unit combination from two input data sets in order to evaluate a data qualiuty rule

| Name  | Description                       |
| ----- | --------------------------------- |
| list1 | *Unknown type*<br>First data set  |
| list2 | *Unknown type*<br>Second data set |


#### Returns

A list of data unit pairs


### M:BIGDQ.DeriveMeasure(base_measures, dimension, dimension_weight)

The implementation of the DerivedMeasure wrapper component.

| Name             | Description                                                  |
| ---------------- | ------------------------------------------------------------ |
| base_measures    | *System.Collections.Generic.List{BIGDQ.BaseMeasure}*<br>Input base measures used to calculate the derived measures |
| dimension        | *BIGDQ.DataQuality.QualityDimension*<br>Quality dimension to calculate |
| dimension_weight | *System.Double*<br>Quality dimension weight to be used in the overall quality assessment |


#### Returns

A DerivedMeasure class that contains all metadata of the derived measure and the measured score


### F:BIGDQ.dt

This DataTable is only used to evaluate data quality rules using DataTable.Compute() function


### M:BIGDQ.Evaluate(quality_rule, element, value)

A function used to evaluate a given data quality rule for a specific element value

| Name         | Description                                |
| ------------ | ------------------------------------------ |
| quality_rule | *System.String*<br>Input data quality rule |
| element      | *System.String*<br>Element name            |
| value        | *System.String*<br>Element value           |


#### Returns

A boolean that shows if the data quality rule is evaluated over the given element value


### M:BIGDQ.Filter(System.Collections.Generic.Dictionary{System.String,System.Object},System.Collections.Generic.List{System.String})

The Filter function is the implementation of the Filter() wrapper which is used to remove the data elements that are not needed from a data unit based on the user configuration

| Name          | Description                                                  |
| ------------- | ------------------------------------------------------------ |
| data_unit     | *Unknown type*<br>The input data unit (Key-Value pair list)  |
| data_elements | *Unknown type*<br>A list that contains all needed data elements |


#### Returns

A new list 


### M:BIGDQ.Filter(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}},System.Collections.Generic.List{System.String})

A sequential implementation of the filtering operation that takes a list of data units as input and loop over them to perform filtering using the Filter() wrapper

| Name          | Description                                                  |
| ------------- | ------------------------------------------------------------ |
| data_units    | *Unknown type*<br>The input data set (list of data units)    |
| data_elements | *Unknown type*<br>A list that contains all needed data elements |


#### Returns

Filtered list of data units


### M:BIGDQ.GetElement(quality_rule)

A function used to extract the data element used in a quality rule

| Name         | Description                           |
| ------------ | ------------------------------------- |
| quality_rule | *System.String*<br>Input quality rule |


#### Returns

Element name


### M:BIGDQ.ParallelBaseMeasure(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}},System.String,System.Double,System.Collections.Generic.Dictionary{System.String,System.Object},System.String)

A parallel implementation of the BaseMeasure wrapper component. This function loop in parallel over each data unit in the input data set and evaluate them over the input data quality rule

| Name           | Description                                                  |
| -------------- | ------------------------------------------------------------ |
| data_units     | *Unknown type*<br>The input list of data units               |
| quality_rule   | *System.String*<br>The quality rule for evaluation           |
| rule_weight    | *Unknown type*<br>The quality rule weight (between 0 and 1)  |
| reference_data | *Unknown type*<br>The refernce look up data set (Not implemented yet) |
| reference_key  | *Unknown type*<br>The reference key used to join the input data set with the reference data (Not implemented yet) |


#### Returns

A BaseMeasure class that contains the measurement metadata and the measured score


### M:BIGDQ.ParallelFilter(System.Collections.Generic.Dictionary{System.String,System.Object},System.Collections.Generic.List{System.String})

The ParallelFilter function is the parallel implementation of the Filter() wrapper which is used to remove the data elements that are not needed from a data unit based on the user configuration

| Name          | Description                                                  |
| ------------- | ------------------------------------------------------------ |
| data_unit     | *Unknown type*<br>The input data unit (Key-Value pair list)  |
| data_elements | *Unknown type*<br>A list that contains all needed data elements |


#### Returns

A new list 


### M:BIGDQ.ParallelFilter(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}},System.Collections.Generic.List{System.String})

A parallel implementation of the filtering operation that takes a sinle data units as input and loop in parallel over them to perform filtering using the parallel implementation of the Filter() wrapper

| Name          | Description                                                  |
| ------------- | ------------------------------------------------------------ |
| data_units    | *Unknown type*<br>The input data set (list of data units)    |
| data_elements | *Unknown type*<br>A list that contains all needed data elements |


#### Returns

Filtered list of data units


### M:BIGDQ.ParallelSample(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}},System.Double)

The ParallelSample function is the parallel implementation of the Sample() wrapper. (Useless in the first release)

| Name       | Description                                                  |
| ---------- | ------------------------------------------------------------ |
| data_units | *Unknown type*<br>The original data set (list of data units) |
| ratio      | *Unknown type*<br>The data sampling ratio (between 0 and 1)  |


#### Returns

It returns a subset of the original data set where the Subset Size = ratio * DataSet Size 


## DataQuality.QualityDimension

In this enumerator we list all supported data quality dimensions


### M:BIGDQ.DataQuality.Sample(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}},System.Double)

The Sample function is the implementation of the Sample() wrapper which is used to perform data sampling on a list of data units. We used a basic data sampling function that pick data units randomly from the original data set

| Name       | Description                                                  |
| ---------- | ------------------------------------------------------------ |
| data_units | *Unknown type*<br>The original data set (list of data units) |
| ratio      | *Unknown type*<br>The data sampling ratio (between 0 and 1)  |


#### Returns

It returns a subset of the original data set where the Subset Size = ratio * DataSet Size 


### M:BIGDQ.DataQuality.Sort(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}},System.String)

An Implementation of the Sort enhancer. A component used to sort an input data set by a given key

| Name          | Description                            |
| ------------- | -------------------------------------- |
| list_of_units | *Unknown type*<br>Input data set       |
| sorting_key   | *Unknown type*<br>Key used for sorting |


#### Returns

A sorted data set (list of lists of key-value pairs)

---------------------------------------------------------


## Class: DerivedMeasure

A class used to store the information of a derived measurement operation

---------------------------------------------

## Class: PreProcessing

A class the contains all preprocessing operations needed by BIGDQ


### M:BIGDQ.ConvertArrayToDictionaryList(header, values)

A function used to convert a array of string into a list of key-value pairs

| Name   | Description           |
| ------ | --------------------- |
| header | *System.String[]*<br> |
| values | *System.String[]*<br> |


#### Returns




### M:BIGDQ.ConvertDataTableToDictionaryList(source)

A function used to convert a data table to a list of dictionaries (key-value list)

| Name   | Description                 |
| ------ | --------------------------- |
| source | *System.Data.DataTable*<br> |


#### Returns




### M:BIGDQ.ImportFlatFile(path, delimiter, qualified)

A function used to import a flat file into a list of dictionaries (key-value list)

| Name      | Description          |
| --------- | -------------------- |
| path      | *System.String*<br>  |
| delimiter | *System.String*<br>  |
| qualified | *System.Boolean*<br> |

#### Returns


--------------------------------------

## Class: Program


### M:BIGDQ.Program.RunMultiThreaded(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}})

Multi-Threaded with sequentual loops (Par1)

| Name       | Description        |
| ---------- | ------------------ |
| data_units | *Unknown type*<br> |

### M:BIGDQ.Program.RunMultiThreadedParallel(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}})

Multi-Threaded with parallel loops (Par2)

| Name       | Description        |
| ---------- | ------------------ |
| data_units | *Unknown type*<br> |

### M:BIGDQ.Program.RunParallelLoops(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}})

Single threaded with parallel loops (Seq2)

| Name       | Description        |
| ---------- | ------------------ |
| data_units | *Unknown type*<br> |

### M:BIGDQ.Program.RunSingleThreaded(System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.String,System.Object}})

Single Threaded implementation (Seq1)

| Name       | Description        |
| ---------- | ------------------ |
| data_units | *Unknown type*<br> |

### F:BIGDQ.Program.Sampling_ratio

Data Sampling ratio (used by the Sample() operator) default value is 70%

