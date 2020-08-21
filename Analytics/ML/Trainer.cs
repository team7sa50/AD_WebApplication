using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.ML;
using Team7_StationeryStore.Analytics.ML.Base;
using Team7_StationeryStore.Analytics.ML.Objects;
using Team7_StationeryStore.Analytics.Common;

namespace Team7_StationeryStore.Analytics

{
    public class Trainer : BaseML
    {
        public void Train(string trainingFileName, string testFileName)
        {
            System.Diagnostics.Debug.WriteLine("Reached Train Method");
            //Check if training file exists
            if (!File.Exists(trainingFileName))
            {
                System.Diagnostics.Debug.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }
            //Check if test file exists 
            if (!File.Exists(testFileName))
            {
                System.Diagnostics.Debug.WriteLine($"Failed to find test data file ({testFileName}");

                return;
            }

            //Convert training file into IDataView object (ready for processing)
            var trainingDataView = MlContext.Data.LoadFromTextFile<CarInventory>(trainingFileName, ',', hasHeader: false);

            //Normalise Mean Variance on the inputted values
            IEstimator<ITransformer> dataProcessPipeline = MlContext.Transforms.Concatenate("Features",
                typeof(CarInventory).ToPropertyList<CarInventory>(nameof(CarInventory.Label)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(inputColumnName: "Features",
                    outputColumnName: "FeaturesNormalizedByMeanVar"));

            //Create a trainer object with the label from the car inventory class + normalised mean variance 
            var trainer = MlContext.BinaryClassification.Trainers.FastTree(labelColumnName: nameof(CarInventory.Label),
                featureColumnName: "FeaturesNormalizedByMeanVar",
                numberOfLeaves: 2,
                numberOfTrees: 800,
                minimumExampleCountPerLeaf: 1,
                learningRate: 0.2);
            //Append the trainer to the pipeline
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            //Save the model 
            var trainedModel = trainingPipeline.Fit(trainingDataView);
            MlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);

            //Evaluate the model like we trained it 
            var evaluationPipeline = trainedModel.Append(MlContext.Transforms
                .CalculateFeatureContribution(trainedModel.LastTransformer)
                .Fit(dataProcessPipeline.Fit(trainingDataView).Transform(trainingDataView)));

            var testDataView = MlContext.Data.LoadFromTextFile<CarInventory>(testFileName, ',', hasHeader: false);

            var testSetTransform = evaluationPipeline.Transform(testDataView);

            var modelMetrics = MlContext.BinaryClassification.Evaluate(data: testSetTransform,
                labelColumnName: nameof(CarInventory.Label),
                scoreColumnName: "Score");

            System.Diagnostics.Debug.WriteLine($"Accuracy: {modelMetrics.Accuracy:P2}");
            System.Diagnostics.Debug.WriteLine($"Area Under Curve: {modelMetrics.AreaUnderRocCurve:P2}");
            System.Diagnostics.Debug.WriteLine($"Area under Precision recall Curve: {modelMetrics.AreaUnderPrecisionRecallCurve:P2}");
            System.Diagnostics.Debug.WriteLine($"F1Score: {modelMetrics.F1Score:P2}");
            System.Diagnostics.Debug.WriteLine($"LogLoss: {modelMetrics.LogLoss:#.##}");
            System.Diagnostics.Debug.WriteLine($"LogLossReduction: {modelMetrics.LogLossReduction:#.##}");
            System.Diagnostics.Debug.WriteLine($"PositivePrecision: {modelMetrics.PositivePrecision:#.##}");
            System.Diagnostics.Debug.WriteLine($"PositiveRecall: {modelMetrics.PositiveRecall:#.##}");
            System.Diagnostics.Debug.WriteLine($"NegativePrecision: {modelMetrics.NegativePrecision:#.##}");
            System.Diagnostics.Debug.WriteLine($"NegativeRecall: {modelMetrics.NegativeRecall:P2}");
        }
    }
}