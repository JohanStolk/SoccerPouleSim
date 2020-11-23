using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SoccerPoolSim.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SoccerPoolSim.Pages
{
    public class IndexModel : PageModel
    {
        private static List<Type> simulatorTypes = SoccerSimTools.FindAllDerivedTypes<SoccerPoolSimulator>();
        private static Dictionary<string, SoccerPoolSimulator> simulators = new();
        private static List<SelectListItem> simulatorSelects = new();

        static IndexModel()
        {
            foreach (Type simulatorType in simulatorTypes)
                simulators[simulatorType.Name] = SoccerSimTools.CreateInstanceOfType<SoccerPoolSimulator>(simulatorType);

            foreach (KeyValuePair<string, SoccerPoolSimulator> simulatorKvp in simulators)
                simulatorSelects.Add(new SelectListItem { Text = simulatorKvp.Key, Value = simulatorKvp.Key });
        }

        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _env;

        public Pool Pool { get; set; }
        public List<SelectListItem> SavedNames { get; } = new();
        public string SavedName { get; set; } = string.Empty;
        public List<SelectListItem> SimulatorNames { get { return simulatorSelects; } }
        public SoccerPoolSimulator Simulator { get; set; } = new SoccerPoolSimulator.Algorithm1();
        public string SimulatorName { get; set; } = string.Empty;
        public IndexModel(ILogger<IndexModel> logger,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;

            SimulatorName = Simulator.Name;
            FillSavedNames();

            //Pool = Pool.GenerateEK88Group2();
            //Pool.GenerateMatches();
            //Simulate();
            //Pool.Save("current.json");
        }

        private void FillSavedNames()
        {
            SavedNames.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSimulationsPath());
            foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.json"))
                SavedNames.Add(new SelectListItem { Text = fileInfo.Name, Value = fileInfo.Name });
        }

        public void OnGet(string action = "", string simulatorName = "", string savedName = "")
        {
            Pool = Pool.Load("current.json");

            if (!string.IsNullOrEmpty(simulatorName))
                SimulatorName = simulatorName;

            if (!string.IsNullOrEmpty(savedName))
                SavedName = savedName;

            switch (action)
            {
                case "simulate":
                    Simulate();
                    break;
                case "save":
                    SaveSimulation();
                    break;
                case "load":
                    LoadSimulation();
                    break;
            }
            Pool.Save("current.json");
        }

        private void Simulate()
        {
            try
            {
                Simulator = simulators[SimulatorName];
                Simulator.Simulate(Pool);
                Pool.GenerateResults();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Simulate failed");
            }
        }

        public void OnPost(string action = "")
        {
            OnGet(action);
        }
        private string GetSimulationsPath() => Path.Combine(_env.WebRootPath, "simulations");

        private void SaveSimulation()
        {
            try
            {
                DateTime date = DateTime.Now;
                string fileName = string.Format("{0}-{1}.json", SimulatorName, date.ToString("yyyyMMddHHmmss"));
                string path = Path.Combine(GetSimulationsPath(), fileName);
                Pool.Save(path);

                FillSavedNames();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SaveSimulation failed");
            }
        }
        private void LoadSimulation()
        {
            try
            {
                string path = Path.Combine(GetSimulationsPath(), SavedName);                
                Pool = Pool.Load(path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LoadSimulation failed");
            }
        }
    }
}
