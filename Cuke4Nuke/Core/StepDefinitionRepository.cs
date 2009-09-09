using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using System.Reflection;
using Cuke4Nuke.Framework;
using NUnit.Framework;

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

        public void Load(string assemblyPath)
        {
            Assembly asm = Assembly.LoadFrom(assemblyPath);
            foreach (Type type in asm.GetTypes())
            {
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    object[] attributes = method.GetCustomAttributes(typeof(StepDefinitionAttribute), true);
                    if (attributes.Length == 1)
                    {
                        StepDefinitionAttribute attribute = attributes[0] as StepDefinitionAttribute;
                        StepDefinition sd = new StepDefinition(attribute.Pattern, method);
                        this.AddStepDefinition(sd);
                    }
                }
            }
        }

        public string InvokeStep(string invocationDetails)
        {
            string stepId = JsonMapper.ToObject(invocationDetails)["id"].ToString();
            StepDefinition sd = _stepDefinitions.Find(s => s.Id == stepId);

            try
            {
                sd.Method.Invoke(null, null);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException.GetType() == typeof(SuccessException))
                {
                    return "OK";
                }
                else
                {
                    Exception e = ex.InnerException;
                    string response = @"FAIL:{ ""message"" : """ + e.GetType().ToString() + ": " + e.Message + @""" }";
                    return response;
                }
            }
            catch (Exception ex)
            {
                string response = @"FAIL:{ ""message"" : """ + ex.GetType().ToString() + ": " + ex.Message + @""" }";
                return response;
            }
            return "OK";
        }
    }
}
