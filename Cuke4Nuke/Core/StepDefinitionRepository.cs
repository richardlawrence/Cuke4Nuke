using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace Cuke4Nuke.Core
{
    public class StepDefinitionRepository
    {
        List<StepDefinition> _stepDefinitions = new List<StepDefinition>();

        public void AddStepDefinition(StepDefinition stepDefinition)
        {
            _stepDefinitions.Add(stepDefinition);
        }

        public List<StepDefinition> StepDefinitions
        {
            get
            {
                return _stepDefinitions;
            }
        }

        public string ListStepDefinitionsAsJson()
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.WriteArrayStart();
            foreach (StepDefinition sd in _stepDefinitions)
            {
                writer.WriteObjectStart();
                writer.WritePropertyName("id");
                writer.Write(sd.Id);
                writer.WritePropertyName("regexp");
                writer.Write(sd.Pattern);
                writer.WriteObjectEnd();
            }
            writer.WriteArrayEnd();
            return sb.ToString();
        }
    }
}
