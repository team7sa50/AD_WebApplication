using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Microsoft.ML;
using Team7_StationeryStore.Analytics.ML.Base;
using Team7_StationeryStore.Analytics.ML.Objects;



namespace Team7_StationeryStore.Analytics.ML
{
    public class Predictor : BaseML
    {
        public CarInventoryPrediction Predict(string inputDataFile)
        {
            /*
            if (!File.Exists(ModelPath))
            {
                System.Diagnostics.Debug.WriteLine($"Failed to find model at {ModelPath}");

                return null;
            }

            if (!File.Exists(inputDataFile))
            {
                System.Diagnostics.Debug.WriteLine($"Failed to find input data at {inputDataFile}");

                return null;
            }*/
            
            ITransformer mlModel;

            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mlModel = MlContext.Model.Load(stream, out _);
            }

            if (mlModel == null)
            {
                Console.WriteLine("Failed to load model");

                return null;
            }

            var predictionEngine = MlContext.Model.CreatePredictionEngine<CarInventory, CarInventoryPrediction>(mlModel);
            
            /*var json = File.ReadAllText(inputDataFile);*/

            var json = inputDataFile;

            CarInventoryPrediction prediction = predictionEngine.Predict(JsonConvert.DeserializeObject<CarInventory>(json));

            System.Diagnostics.Debug.WriteLine(
                                $"Based on input json:{System.Environment.NewLine}" +
                                $"{json}{System.Environment.NewLine}" +
                                $"It is time to {(prediction.PredictedLabel ? "restock" : "to stay put")} deal, with a {prediction.Probability:P0} confidence");
            return prediction;
        }
    }
}
