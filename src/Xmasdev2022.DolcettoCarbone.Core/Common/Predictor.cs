using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xmasdev2022.DolcettoCarbone.Models;

namespace Xmasdev2022.DolcettoCarbone.Common
{
    
    public class Predictor
    {
        protected static string ModelPath => Path.Combine(AppContext.BaseDirectory, "classification.mdl");
        private readonly MLContext _mlContext;

        private ITransformer _model;

        public Predictor()
        {
            _mlContext = new MLContext();
        }
        public ModelOutput Predict(ModelInput newSample)
        {
            LoadModel();

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_model);
            
            return predictionEngine.Predict(newSample);
        }
        private void LoadModel()
        {
            if (!File.Exists(ModelPath))
            {
                throw new FileNotFoundException($"File {ModelPath} doesn't exist.");
            }

            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _model = _mlContext.Model.Load(stream, out _);
            }

            if (_model == null)
            {
                throw new Exception($"Failed to load Model");
            }
        }
    }
}
