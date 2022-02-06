import enum
import string
import pandas as pd
from dask import dataframe as dd
import re
from datetime import datetime

 


class QualityDimension(enum.Enum):
    Completeness = 1
    Uniqueness = 2
    Accuracy = 3 
    Consistency = 4
    Credibility = 5
    Compliance = 6
    Understandability = 7
    Currentness = 8

class BaseMeasure():
    
    def __init__(self,quality_rule: string,weight: float, score: float = 0.0, time: int = 0, filename: string = "" ):
        self.quality_rule = quality_rule
        if(weight < 0 or weight > 1):
            raise Exception("weight value must be between 0 and 1")
        self.filename = filename
        self.weight = weight
        self.score = score
        self.time = time

    def combine(self, base_measure):
        if(self.quality_rule != base_measure.quality_rule):
            raise Exception("Cannot combine base measures having different rules")
        
        if(isinstance(base_measure, BaseMeasure)):
            self.score = (self.score + base_measure.score) / 2
            self.time += base_measure.time



class DerivedMeasure():
    def __init__(self, dimension: QualityDimension,weight: float,score: float):
        self.dimension = dimension
        if(weight < 0 or weight > 1):
            raise Exception("weight value must be between 0 and 1")

        self.weight = weight
        self.score = score       

class DataQuality:

    @staticmethod
    def GetElement(rule: string) -> string:
        p = re.compile("\[(.*?)\]")
        result = p.search(rule)
        return result.group(1)

    @staticmethod
    def Filter(data: pd.DataFrame, cols: list) -> pd.DataFrame :
        result = data.drop(columns=cols)
        return result

    @staticmethod
    def Sample(data: pd.DataFrame, ratio) -> pd.DataFrame :
        result = data.sample(frac=ratio)
        return result

    @staticmethod
    def BaseMeasure(data: pd.DataFrame,rule: string, weight: float, filename = "") -> BaseMeasure :
        date1 = datetime.now()
        rowcount = len(data.index)
        rulecount = data.eval(rule).sum()
        time = datetime.now() - date1
        result = BaseMeasure(rule,weight,rulecount / rowcount,time.total_seconds(),filename)
        return result

    @staticmethod
    def DeriveMeasure(base_measures: list, dimension: QualityDimension, weigth: float) -> DerivedMeasure:
        score = 0.0
        for measure in base_measures:
            if(isinstance(measure,BaseMeasure)):
                score += measure.score * measure.weight
        result = DerivedMeasure(dimension,weigth,score)
        return result

    @staticmethod 
    def ExtractValues(data: pd.DataFrame, columns):
        if(isinstance(columns,str)):
            return data[columns].to_frame(columns)
        else:
            return data[columns]

    @staticmethod
    def GroupByDistinctCount(data: pd.DataFrame, group_by, columns) -> pd.DataFrame:
        return data.groupby(group_by)[columns].nunique().to_frame(name='uniquecount')

    @staticmethod
    def Assess(derived_measures: list) -> float:
        score = 0.0
        for measure in derived_measures:
            if(isinstance(measure,DerivedMeasure)):
                score += measure.score * measure.weight
        return score


    #Dask methods
    @staticmethod
    def DaskFilter(data: dd.DataFrame, cols: list) -> dd.DataFrame :
        result = data.drop(columns=cols)
        return result

    @staticmethod
    def DaskSample(data: dd.DataFrame, ratio) -> dd.DataFrame :
        result = data.sample(frac=ratio)
        return result

    @staticmethod
    def DaskBaseMeasure(data: dd.DataFrame,rule: string, weight: float, filename: string = "") -> BaseMeasure :
        date1 = datetime.now()
        rowcount = len(data.index)
        rulecount = data.eval(rule).sum()
        time = datetime.now() - date1
        result = BaseMeasure(rule,weight,rulecount / rowcount,time.total_seconds(),filename)
        return result

    @staticmethod 
    def DaskExtractValues(data: dd.DataFrame, columns):
        if(isinstance(columns,str)):
            return data[columns].to_frame(columns)
        else:
            return data[columns]

    @staticmethod
    def DaskGroupByDistinctCount(data: dd.DataFrame, group_by, columns) -> pd.DataFrame:
        return data.groupby(group_by)[columns].nunique().to_frame(name='uniquecount')                       