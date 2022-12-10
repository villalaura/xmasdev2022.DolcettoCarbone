using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xmasdev2022.DolcettoCarbone.Common;
using Xmasdev2022.DolcettoCarbone.Models;
using Xmasdev2022.DolcettoCarbone.Trainers;

namespace Xmasdev2022.DolcettoCarbone.Blazor.BusinessLayer.Services
{
    public class DolcettoCarboneService : IDolcettoCarboneService
    {
        private readonly Predictor predictor;
        
        public DolcettoCarboneService()
        {
            predictor = new Predictor();
        }

        public Task<ModelOutput> PredictAsync(string filePath, ModelInput modelInput)
        {
            var prediction = predictor.Predict(filePath, modelInput);

            return Task.FromResult(prediction);
        } 
    }
}
