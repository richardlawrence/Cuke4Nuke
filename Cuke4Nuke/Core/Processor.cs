using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Cuke4Nuke.Framework;

namespace Cuke4Nuke.Core
{
    public interface IProcessor
    {
        string Process(string request);
    }

    public class Processor : IProcessor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        readonly Loader _loader;
        readonly Repository _repository;
        readonly Formatter _formatter = new Formatter();
        readonly ObjectFactory _objectFactory;

        public Processor(Loader loader, ObjectFactory objectFactory)
        {
            _loader = loader;
            _objectFactory = objectFactory;
            _repository = _loader.Load();

            // Register TypeConverter for Cuke4Nuke.Framework.Table
            TypeConverterAttribute attr = new TypeConverterAttribute(typeof(TableConverter));
            TypeDescriptor.AddAttributes(typeof(Table), new Attribute[] { attr });
        }

        public string Process(string request)
        {
            try
            {
                JArray requestObject = JArray.Parse(request);
                string command = requestObject[0].Value<string>();
                switch (command)
                {
                    case "begin_scenario":
                        _objectFactory.CreateObjects();
                        _repository.BeforeHooks.ForEach(hook => hook.Invoke(_objectFactory));
                        return SuccessResponse();
                    case "end_scenario":
                        _repository.AfterHooks.ForEach(hook => hook.Invoke(_objectFactory));
                        _objectFactory.DisposeObjects();
                        return SuccessResponse(); 
                    case "step_matches":
                        return StepMatches(((JObject)requestObject[1])["name_to_match"].ToString());
                    case "invoke":
                        JArray jsonArgs = (JArray)((JObject)requestObject[1])["args"];
                        string[] args = new string[jsonArgs.Count];
                        for (int i = 0; i < args.Length; ++i)
                        {
                            args[i] = jsonArgs[i].ToString();
                        }
                        return Invoke(requestObject[1]["id"].ToString(), args);
                    default:
                        return _formatter.Format("Invalid request '" + request + "'");
                }
            }
            //catch (JsonException x)
            //{
            //    return _formatter.Format("Invalid json in request '" + request + "': " + x.Message);
            //}
            catch (Exception x)
            {
                return _formatter.Format(x);
            }
        }

        string SuccessResponse()
        {
            return "[\"success\",null]";
        }

        string StepMatches(string stepName)
        {
            log.DebugFormat("Looking for step matches for step name: {0}", stepName);

            var matches = new JArray();
            foreach (var sd in _repository.StepDefinitions)
            {
                List<StepArgument> args = sd.ArgumentsFrom(stepName);
                if(args != null)
                {
                    var stepMatch = new JObject();
                    stepMatch.Add("id", sd.Id);
                    var jsonArgs = new JArray();
                    foreach (var arg in args)
                    {
                        var jsonArg = new JObject();
                        jsonArg.Add("val", arg.Val);
                        jsonArg.Add("pos", arg.Pos);
                        jsonArgs.Add(jsonArg);
                    }
                    stepMatch["args"] = jsonArgs;
                    matches.Add(stepMatch);
                }
            }
            var response = new JArray();
            response.Add("step_matches");
            response.Add(matches);
            return response.ToString(Formatting.None);
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

                stepDefinition.Invoke(_objectFactory, args);
                return SuccessResponse();
            }
            catch (TargetInvocationException x)
            {
                return _formatter.Format(x.InnerException);
            }
        }

        StepDefinition GetStepDefinition(string id)
        {
            return _repository.StepDefinitions.Find(s => s.Id == id);
        }
    }
}
