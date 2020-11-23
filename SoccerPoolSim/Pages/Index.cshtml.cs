using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SoccerPoolSim.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerPoolSim.Pages
{
    public class IndexModel : PageModel
    {
        private static List<Type> simulatorTypes = SoccerSimTools.FindAllDerivedTypes<SoccerPoolSimulator>();
        private static List<SoccerPoolSimulator> simulators = new List<SoccerPoolSimulator>();
        private static List<SelectListItem> simulatorSelects = new List<SelectListItem>();

        static IndexModel()
        {
            foreach (Type simulatorType in simulatorTypes)
                simulators.Add(SoccerSimTools.CreateInstanceOfType<SoccerPoolSimulator>(simulatorType));

            foreach (ISoccerPoolSimulator simulator in simulators)
            {
                simulatorSelects.Add(new SelectListItem { Text = simulator.Name, Value = simulator.GetType().Name });
            }
        }

        private readonly ILogger<IndexModel> _logger;

        public List<SelectListItem> SimulatorNames { get { return simulatorSelects; } }
        public SoccerPoolSimulator Simulator { get; set; } = new SoccerPoolSimulator.Algorithm1();
        public string SimulatorName => Simulator.Name;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            
            Pool = Core.Pool.GenerateEK88Group2();
            Pool.GenerateMatches();
        }

        public IPool Pool { get; }

        public void OnGet()
        {
            Simulator.Simulate(Pool);
            Pool.GenerateResults();
        }
        public void OnPost()
        {
            OnGet();
        }

    }
}
