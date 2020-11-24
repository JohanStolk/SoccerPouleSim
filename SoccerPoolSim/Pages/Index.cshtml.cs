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
    /// <summary>
    /// the page model for our index page
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// a static collection of all known simulator names
        /// </summary>
        private static readonly List<SelectListItem> simulatorSelects = new();

        /// <summary>
        /// static ctor filling the simulatorSelects collection once
        /// </summary>
        static IndexModel()
        {
            foreach (KeyValuePair<string, SoccerPoolSimulator> simulatorKvp in SoccerPoolSimulator.Simulators)
                simulatorSelects.Add(new SelectListItem { Text = simulatorKvp.Key, Value = simulatorKvp.Key });
        }

        /// <summary>
        /// the logger object achieved thru DI
        /// </summary>
        private readonly ILogger<IndexModel> _logger;
        /// <summary>
        /// the IWebHostEnvironment object achieved thru DI
        /// </summary>
        private readonly IWebHostEnvironment _env;
        /// <summary>
        /// the path in which pool results .json files are loaded/saved
        /// </summary>
        /// <returns></returns>
        private string SimulationsPath => Path.Combine(_env.WebRootPath, "simulations");
        /// <summary>
        /// the Pool object we use to display & simulate
        /// </summary>
        public Pool Pool { get; set; } = new Pool();
        /// <summary>
        /// the SavedNames collection shown as a html select to the client (json file names from SimulationsPath)
        /// </summary>
        public List<SelectListItem> SavedNames { get; } = new();
        /// <summary>
        /// the currently selected SavedName
        /// </summary>
        public string SavedName { get; set; } = string.Empty;
        /// <summary>
        /// the SavedNames collection shown as a html select to the client
        /// </summary>
        public List<SelectListItem> SimulatorNames { get { return simulatorSelects; } }
        /// <summary>
        /// the currently selected SimulatorName
        /// </summary>
        public string SimulatorName { get; set; } = string.Empty;

        /// <summary>
        /// ctor setting defaults and initializing logger & WebHostEnvironment thru dependency injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="env"></param>
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;

            if (SimulatorNames.Count > 0)
                SimulatorName = SimulatorNames[0].Text;
            FillSavedNames();

            // code below can be used to generate an initial state json files
#if GENERATE_CURRENT_JSON
            Pool = Pool.GenerateEK88Group2();
            Pool.GenerateMatches();
            Simulate();
            Pool.Save("current.json");
#endif
#if GENERATE_GROUP2_JSON
            Pool = Pool.GenerateEK88Group2WithMatches();
            Pool.GenerateResults();
            Pool.Save("EK-1988-Group2.json");
#endif
#if GENERATE_GROUP1_JSON
            Pool = Pool.GenerateEK88Group1();
            //Pool.GenerateResults();
            Pool.Save("EK-1988-Group1.json");
#endif
        }

        /// <summary>
        /// fill the SavedNames collection from disk, shown as a html select to the client
        /// </summary>
        private void FillSavedNames()
        {
            SavedNames.Clear();
            DirectoryInfo directoryInfo = new DirectoryInfo(SimulationsPath);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.json"))
                SavedNames.Add(new SelectListItem { Text = fileInfo.Name, Value = fileInfo.Name });
        }

        /// <summary>
        /// the HTTP get entry point
        /// </summary>
        /// <param name="action"></param>
        /// <param name="simulatorName"></param>
        /// <param name="savedName"></param>
        public void OnGet(string action = "", string simulatorName = "", string savedName = "")
        {
            Pool = Pool.Load("current.json"); // this is how we keep state so info is kept accross sessions

            if (!string.IsNullOrEmpty(simulatorName))
                SimulatorName = simulatorName;

            if (!string.IsNullOrEmpty(savedName))
                SavedName = savedName;

            if (!string.IsNullOrEmpty(action))
            {
                switch (action)
                {
                    case "simulate":
                        Simulate(SoccerPoolSimulator.Simulators[SimulatorName], Pool);
                        break;
                    case "save":
                        SaveSimulation();
                        break;
                    case "load":
                        LoadSimulation(SavedName);
                        break;
                    default:
                        throw new SoccerPoolSimException("unknown action " + action);
                }
            }
            Pool.Save("current.json"); // update our state
        }

        /// <summary>
        /// the HTTP POST entry point, redirect to get for this simple website
        /// </summary>
        /// <param name="action"></param>
        public void OnPost(string action = "", string simulatorName = "", string savedName = "")
        {
            OnGet(action, simulatorName, savedName);
        }

        /// <summary>
        /// simulate using the given simulator and pool
        /// </summary>
        private void Simulate(SoccerPoolSimulator simulator, Pool pool)
        {
            try
            {
                pool.GenerateMatches();
                simulator.Simulate(pool);
                pool.GenerateResults();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Simulate failed");
            }
        }

        /// <summary>
        /// save the current simulation state to disk
        /// </summary>
        private void SaveSimulation()
        {
            try
            {
                DateTime date = DateTime.Now;
                string fileName = string.Format("{0}-{1}-{2}.json", Pool.Name, SimulatorName, date.ToString("yyyyMMddHHmmss"));
                string path = Path.Combine(SimulationsPath, fileName);
                Pool.Save(path);

                FillSavedNames();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SaveSimulation failed");
            }
        }

        /// <summary>
        /// load a simulation from disk given the SavedName
        /// </summary>
        private void LoadSimulation(string name)
        {
            try
            {
                string path = Path.Combine(SimulationsPath, name);
                Pool = Pool.Load(path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LoadSimulation failed");
            }
        }
    }
}
