using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke.Core
{
    public class Repository
    {
        public Repository()
        {
            StepDefinitions = new List<StepDefinition>();
            BeforeHooks = new List<BeforeHook>();
            AfterHooks = new List<AfterHook>();
        }

        public Repository(List<StepDefinition> stepDefinitions, List<BeforeHook> beforeHooks, List<AfterHook> afterHooks)
        {
            StepDefinitions = stepDefinitions;
            BeforeHooks = beforeHooks;
            AfterHooks = afterHooks;
        }

        public List<StepDefinition> StepDefinitions { get; private set; }
        public List<BeforeHook> BeforeHooks { get; private set; }
        public List<AfterHook> AfterHooks { get; private set; }
    }
}
