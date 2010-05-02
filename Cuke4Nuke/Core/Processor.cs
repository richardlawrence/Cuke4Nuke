using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Cuke4Nuke.Framework;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

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
                        string nameToMatch = ((JObject)requestObject[1])["name_to_match"].Value<string>();
                        return StepMatches(nameToMatch);
                    case "snippet_text":
                        string keyword = ((JObject)requestObject[1])["step_keyword"].Value<string>();
                        string stepName = ((JObject)requestObject[1])["step_name"].Value<string>();
                        string multilineArgClass = ((JObject)requestObject[1])["multiline_arg_class"].Value<string>();
                        return SnippetResponse(keyword, stepName, multilineArgClass);
                    case "invoke":
                        JArray jsonArgs = (JArray)((JObject)requestObject[1])["args"];
                        string[] args = new string[jsonArgs.Count];
                        for (int i = 0; i < args.Length; ++i)
                        {
                            if (jsonArgs[i] is JArray)
                            {
                                args[i] = jsonArgs[i].ToString(Formatting.None);
                            }
                            else
                            {
                                args[i] = jsonArgs[i].Value<string>();
                            }
                        }
                        return Invoke(requestObject[1]["id"].Value<string>(), args);
                    default:
                        return FailResponse("Invalid request '" + request + "'");
                }
            }
            catch (Exception x)
            {
                log.Error("Unable to process request '" + request + "': " + x.Message);
                return FailResponse(x);
            }
        }

        private string SnippetResponse(string keyword, string stepName, string multilineArgClass)
        {
            SnippetBuilder snb = new SnippetBuilder();
            string snippet = snb.GenerateSnippet(keyword, stepName, multilineArgClass);

            StringBuilder stb = new StringBuilder();
            StringWriter sw = new StringWriter(stb);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.None;
                jsonWriter.WriteStartArray();
                jsonWriter.WriteValue("snippet_text");
                jsonWriter.WriteValue(snippet);
                jsonWriter.WriteEndArray();
            }
            return sw.ToString();
        }

        string SuccessResponse()
        {
            return "[\"success\",null]";
        }

        string PendingResponse()
        {
            return "[\"pending\",null]";
        }

        string FailResponse(string message)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.None;
                jsonWriter.WriteStartArray();
                jsonWriter.WriteValue("fail");
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("message");
                jsonWriter.WriteValue(message);
                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndArray();
            }
            return sw.ToString();
        }

        string FailResponse(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.None;
                jsonWriter.WriteStartArray();
                jsonWriter.WriteValue("fail");
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("exception");
                jsonWriter.WriteValue(ex.GetType().ToString());
                jsonWriter.WritePropertyName("message");
                jsonWriter.WriteValue(ex.Message);
                jsonWriter.WritePropertyName("backtrace");
                jsonWriter.WriteValue(ex.StackTrace);
                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndArray();
            }
            return sw.ToString();
        }

        string StepMatches(string stepName)
        {
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
                    return FailResponse("Could not find step with id '" + id + "'");
                }

                if (stepDefinition.Pending)
                {
                    return PendingResponse();
                }

                stepDefinition.Invoke(_objectFactory, args);
                return SuccessResponse();
            }
            catch (TargetInvocationException x)
            {
                if (x.InnerException is TableAssertionException)
                {
                    TableAssertionException ex = (TableAssertionException) x.InnerException;
                    return TableDiffResponse(ex.Expected, ex.Actual);
                }
                return FailResponse(x.InnerException);
            }
        }

        private string TableDiffResponse(Table expectedTable, Table actualTable)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Table));
            string expectedTableJson = (string)converter.ConvertToString(expectedTable);
            string actualTableJson = (string)converter.ConvertToString(actualTable);
            return String.Format("[\"diff!\", [{0},{1}]]", expectedTableJson, actualTableJson);
        }

        StepDefinition GetStepDefinition(string id)
        {
            return _repository.StepDefinitions.Find(s => s.Id == id);
        }
    }
}
