// See https://aka.ms/new-console-template for more information
using System.Xml.Linq;
using Xmasdev2022.DolcettoCarbone.Common;
using Xmasdev2022.DolcettoCarbone.Models;
using Xmasdev2022.DolcettoCarbone.Trainers;

//NormalizeMinMaxMulticolumn.Example();

//Trainer trainer = new Trainer();
var trainers = new List<ITrainerBase>
            {
                new LbfgsLogisticRegressionTrainer(),
                //new AveragedPerceptronTrainer(),
                //new PriorTrainer(),
                new SdcaLogisticRegressionTrainer()
                //new SdcaNonCalibratedTrainer(),
                //new SgdCalibratedTrainer(),
                //new SgdNonCalibratedTrainer()
            };

var newSample = new Xmasdev2022.DolcettoCarbone.Models.ModelInput()
{
    GiocattoliRotti = 12,
    MediaVoti = 4,
    Note = 1,
    Parolacce = 2,
    VisiteNonni = 122
};

trainers.ForEach(t => TrainEvaluatePredict(t, newSample));

static void TrainEvaluatePredict(ITrainerBase trainer, ModelInput newSample)
{
    Console.WriteLine("*******************************");
    Console.WriteLine($"{trainer.Name}");
    Console.WriteLine("*******************************");

    string path = $"{Directory.GetCurrentDirectory()}\\Data\\befana.csv";
    
    //Fit with training data
    trainer.Fit(path);
    //Evaluate
    var modelMetrics = trainer.Evaluate();

    Console.WriteLine($"Accuracy: {modelMetrics.Accuracy:0.##}{Environment.NewLine}" +
                      $"Precision: {modelMetrics.PositivePrecision:#.##}{Environment.NewLine}" +
                      $"Recall: {modelMetrics.PositiveRecall:#.##}{Environment.NewLine}" +
                      $"F1 Score: {modelMetrics.F1Score:#.##}{Environment.NewLine}" +
                      $"Area Under Roc Curve: {modelMetrics.AreaUnderRocCurve:#.##}{Environment.NewLine}");

    trainer.Save();

    //usi il modello nell'applicazione
    var predictor = new Predictor();
    var prediction = predictor.Predict(newSample);
    Console.WriteLine("------------------------------");
    Console.WriteLine($"Prediction: {prediction.PredictedLabel:#.##}");
    Console.WriteLine("------------------------------");
}

//trainer.Fit(path);

//var modelMetrics = trainer.Evaluate();

//Console.WriteLine($"Accuracy: {modelMetrics.Accuracy:0.##}{Environment.NewLine}" +
//                  $"F1 Score: {modelMetrics.F1Score:#.##}{Environment.NewLine}" +
//                  $"Area under the Curve: {modelMetrics.AreaUnderRocCurve:#.##}{Environment.NewLine}" +
//                  $"Area Under Precision Recall Curve: {modelMetrics.AreaUnderPrecisionRecallCurve:#.##}{Environment.NewLine}");


//trainer.Save();

//Predictor predictor = new Predictor();
////ModelOutput result = predictor.Predict(
////    new Xmasdev2022.DolcettoCarbone.Models.ModelInput()
////    {
////        GiocattoliRotti = 10,
////        MediaVoti = 2,
////        Note = 20,
////        Parolacce = 30,
////        VisiteNonni = 2
////    }
////    );

//ModelOutput result = predictor.Predict(
//    new Xmasdev2022.DolcettoCarbone.Models.ModelInput()
//    {
//        GiocattoliRotti = 1,
//        MediaVoti = 8,
//        Note = 1,
//        Parolacce = 2,
//        VisiteNonni = 12
//    }
//    );

//Console.WriteLine("Predicted: " + result.PredictedLabel + " Probability: " + result.Probability.ToString());