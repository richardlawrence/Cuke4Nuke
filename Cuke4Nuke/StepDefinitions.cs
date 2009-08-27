using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke
{
    public class StepDefinitions
    {
        StepDefinitionRepository _stepDefinitionRepository;
        public StepDefinitions()
        {
            _stepDefinitionRepository = new StepDefinitionRepository();
        }

        public abstract void DefineSteps();

        public void Given(string pattern, Expression<object[]> invocationExpression)
        {
            RegisterStepDefinition(pattern, invocationExpression);
        }

        private void RegisterStepDefinition(string pattern, Expression invocationExpression)
        {
            stepDefintionRepository.Add(new StepDefinition(pattern, invocationExpression));
        }
    }
}
