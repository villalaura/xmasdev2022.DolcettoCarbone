using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xmasdev2022.DolcettoCarbone.Models;

namespace Xmasdev2022.DolcettoCarbone.Common
{
    public class Trainer<TParameters> : ITrainerBase
                where TParameters : class
    {
        protected readonly MLContext MlContext;
        protected DataOperationsCatalog.TrainTestData _dataSplit;

        protected ITransformer _trainedModel;
         protected ITrainerEstimator<BinaryPredictionTransformer<TParameters>, TParameters> _model;

        protected static string ModelPath => Path
                  .Combine(AppContext.BaseDirectory, "classification.mdl");

        public string Name { get; protected set; }

        public Trainer()
        {
            MlContext = new MLContext();
        }

        //Fit with training data
        public void Fit(string trainingFileName)
        {
            if (!File.Exists(trainingFileName))
            {
                throw new FileNotFoundException($"File {trainingFileName} doesn't exist.");
            }

            _dataSplit = LoadAndPrepareData(trainingFileName);

            //build Data Processing Pipeline
            var dataProcessPipeline = BuildDataProcessingPipeline();


            ////set LbfgsLogisticRegressionBinaryTrainer with options
            //var options = new LbfgsLogisticRegressionBinaryTrainer.Options()
            //{
            //    MaximumNumberOfIterations = 100,
            //    OptimizationTolerance = 1e-8f,
            //    L2Regularization = 0.01f
            //};

            //append trainer to processing pipeline
            //var trainingPipeline = dataProcessPipeline
            //    .Append(MlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: "Label", featureColumnName: "NormalizedFeatures"));
            
             var trainingPipeline = dataProcessPipeline.Append(_model);

            //train model
            _trainedModel = trainingPipeline.Fit(_dataSplit.TrainSet);

            //debug normalized trainset
            var x = _trainedModel.Transform(_dataSplit.TrainSet).Preview();
        }
        public void Save()
        {
            MlContext.Model.Save(_trainedModel, _dataSplit.TrainSet.Schema, ModelPath);
        }
        public BinaryClassificationMetrics Evaluate()
        {
            var testSetTransform = _trainedModel.Transform(_dataSplit.TestSet);
            var tested = testSetTransform.Preview();

            return MlContext.BinaryClassification.EvaluateNonCalibrated(testSetTransform);
        }
        private EstimatorChain<NormalizingTransformer> BuildDataProcessingPipeline()
        {
            var dataProcessPipeline = MlContext.Transforms.Concatenate("Features",
                                               nameof(ModelInput.Note),
                                               nameof(ModelInput.GiocattoliRotti),
                                               nameof(ModelInput.Parolacce),
                                               nameof(ModelInput.VisiteNonni)
                                               )
               .Append(MlContext.Transforms.NormalizeMinMax("NormalizedFeatures", "Features"))
               .AppendCacheCheckpoint(MlContext);

            return dataProcessPipeline;
        }

        private DataOperationsCatalog.TrainTestData LoadAndPrepareData(string trainingFileName)
        {
            var trainingDataView = MlContext.Data
                                    .LoadFromTextFile<ModelInput>
                                      (trainingFileName, hasHeader: true, separatorChar: ';');
            
            return MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.3);
        }
    }
}
