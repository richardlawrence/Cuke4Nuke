using System;
using System.Collections.Generic;
using System.Reflection;

using LitJson;

namespace Cuke4Nuke.Core
{
    public interface IProcessor
    {
        string Process(string request);
    }

    public class Processor : IProcessor
    {
        readonly Loader _loader;
        List<StepDefinition> _stepDefinitions;
        readonly Formatter _formatter = new Formatter();

        public Processor(Loader loader)
        {
            _loader = loader;
        }

        public string Process(string request)
        {
            _stepDefinitions = _loader.Load();

            try
            {
                if (request == "list_step_definitions")
                {
                    return _formatter.Format(_stepDefinitions);
                }

                if (request.StartsWith("invoke:"))
                {
                    return Invoke(request);
                }

                return _formatter.Format("Invalid request '" + request + "'");
            }
            catch (JsonException x)
            {
                return _formatter.Format("Invalid json in request '" + request + "': " + x.Message);
            }
            catch (Exception x)
            {
                return _formatter.Format(x);
            }
        }

        string Invoke(string request)
        {
            try
            {
                var id = GetStepDefinitionId(request.Substring(7));
                var stepDefinition = GetStepDefinition(id);

                if (stepDefinition == null)
                {
                    return _formatter.Format("Could not find step with id '" + id + "'");
                }

                stepDefinition.Invoke();
                return "OK";
            }
            catch (KeyNotFoundException)
            {
                return _formatter.Format("Missing 'id' in request");
            }
            catch (TargetInvocationException x)
            {
                return _formatter.Format(x.InnerException);
            }
        }

        static string GetStepDefinitionId(string jsonDetails)
        {
            return JsonMapper.ToObject(jsonDetails)["id"].ToString();
        }

        StepDefinition GetStepDefinition(string id)
        {
            return _stepDefinitions.Find(s => s.Id == id);
        }
    }
}
