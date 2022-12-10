using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xmasdev2022.DolcettoCarbone.Models;

namespace Xmasdev2022.DolcettoCarbone.Blazor.BusinessLayer.Services
{
    public interface IDolcettoCarboneService
    {
        Task<ModelOutput> PredictAsync(string filePath, ModelInput modelInput);
    }
}
