using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        readonly ObjectFactory _objectFactory;

        public Processor(Loader loader, ObjectFactory objectFactory)
        {
            _loader = loader;
            _objectFactory = objectFactory;
            _stepDefinitions = _loader.Load();
        }

        public string Process(string request)
        {
            try
            {
                JsonData requestObject = JsonMapper.ToObject(request);
                String command = requestObject[0].ToString();
                if (command.Equals("step_matches"))
                {
                    return StepMatches(requestObject[1]["name_to_match"].ToString());
                }

                if (command.Equals("invoke"))
                {
                    JsonData jsonArgs = requestObject[1]["args"];
                    string[] args = new string[jsonArgs.Count];
                    for (int i = 0; i < args.Length; ++i)
                    {
                        args[i] = jsonArgs[i].ToString();
                    }
                    return Invoke(requestObject[1]["id"].ToString(), args);
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

        string StepMatches(string stepName)
        {
            JsonData matches = new JsonData();
            matches.SetJsonType(JsonType.Array);
            foreach (var sd in _stepDefinitions)
            {
                List<StepArgument> args = sd.ArgumentsFrom(stepName);
                if(args != null)
                {
                    JsonData stepMatch = new JsonData();
                    stepMatch["id"] = sd.Id;
                    JsonData jsonArgs = new JsonData();
                    jsonArgs.SetJsonType(JsonType.Array);
                    foreach (var arg in args)
                    {
                        JsonData jsonArg = new JsonData();
                        jsonArg["val"] = arg.Val;
                        jsonArg["pos"] = arg.Pos;
                        jsonArgs.Add(jsonArg);
                    }
                    stepMatch["args"] = jsonArgs;
                    matches.Add(stepMatch);
                }
            }
            JsonData response = new JsonData();
            response.Add("step_matches");
            response.Add(matches);
            return JsonMapper.ToJson(response);
        }

        string Invoke(string id, string[] args)
        {
            try
            {
                var stepDefinition = GetStepDefinition(id);

                if (stepDefinition == null)
                {
                    return _formatter.Format("Could not find step with id '" + id + "'");
                }

                ParameterInfo [] parameters = stepDefinition.Method.GetParameters();
                if (parameters.Length != args.Length)
                {
                    throw new ArgumentException("Expected " + parameters.Length + " argument(s); got " + args.Length);
                }
                var typedArgs = new object[args.Length];
                for (int i = 0; i < args.Length; ++i)
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(parameters[i].ParameterType);
                    typedArgs[i] = converter.ConvertFromString(args[i]);
                }

                stepDefinition.Invoke(_objectFactory, typedArgs);
                JsonData response = new JsonData();
                response.Add("OK");
                return JsonMapper.ToJson(response);
            }
            catch (KeyNotFoundException x)
            {
                return _formatter.Format("Missing '" + x.ToString() + "' in request");
            }
            catch (TargetInvocationException x)
            {
                return _formatter.Format(x.InnerException);
            }
        }

        StepDefinition GetStepDefinition(string id)
        {
            if(_stepDefinitions == null )
            {
                _stepDefinitions = _loader.Load();
            }
            return _stepDefinitions.Find(s => s.Id == id);
        }
    }
}
