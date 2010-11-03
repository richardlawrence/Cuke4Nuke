using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using Cuke4Nuke.Framework;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cuke4Nuke.Core
{
    public interface IProcessor
    {
        string Process(string request);
    }

    public class Processor : IProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Loader _loader;
        private readonly ObjectFactory _objectFactory;
        private readonly Repository _repository;

        public Processor(Loader loader, ObjectFactory objectFactory)
        {
            _loader = loader;
            _objectFactory = objectFactory;
            _repository = _loader.Load();

            // Register TypeConverter for Cuke4Nuke.Framework.Table
            var attr = new TypeConverterAttribute(typeof (TableConverter));
            TypeDescriptor.AddAttributes(typeof (Table), new Attribute[] {attr});
        }

        #region IProcessor Members

        public string Process(string request)
        {
            try
            {
                JArray requestObject = JArray.Parse(request);
                var command = requestObject[0].Value<string>();
                switch (command)
                {
                    case "begin_scenario":
                        _objectFactory.CreateObjects();
                        var scenarioTags = GetScenarioTags(requestObject);
                        _repository.BeforeHooks.ForEach(hook => hook.Invoke(_objectFactory, scenarioTags));
                        return SuccessResponse();
                    case "end_scenario":
                        _repository.AfterHooks.ForEach(hook => hook.Invoke(_objectFactory, GetScenarioTags(requestObject)));
                        _objectFactory.DisposeObjects();
                        return SuccessResponse();
                    case "step_matches":
                        var nameToMatch = ((JObject) requestObject[1])["name_to_match"].Value<string>();
                        return StepMatches(nameToMatch);
                    case "snippet_text":
                        var keyword = ((JObject) requestObject[1])["step_keyword"].Value<string>();
                        var stepName = ((JObject) requestObject[1])["step_name"].Value<string>();
                        var multilineArgClass = ((JObject) requestObject[1])["multiline_arg_class"].Value<string>();
                        return SnippetResponse(keyword, stepName, multilineArgClass);
                    case "invoke":
                        var jsonArgs = (JArray) ((JObject) requestObject[1])["args"];
                        return Invoke(requestObject[1]["id"].Value<string>(), ToStringArray(jsonArgs));
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

        private string[] GetScenarioTags(JArray requestObject)
        {
            string[] scenarioTags = new string[0];
            if (requestObject.Count > 1)
            {
                scenarioTags = ToStringArray((JArray)((JObject)requestObject[1])["tags"]);
            }
            return scenarioTags;
        }


        #endregion

        private string[] ToStringArray(JArray jsonArray)
        {
            var stringArray = new string[jsonArray.Count];
            for (int i = 0; i < stringArray.Length; ++i)
            {
                if (jsonArray[i] is JArray)
                {
                    stringArray[i] = jsonArray[i].ToString(Formatting.None);
                }
                else
                {
                    stringArray[i] = jsonArray[i].Value<string>();
                }
            }
            return stringArray;
        }

        private string SnippetResponse(string keyword, string stepName, string multilineArgClass)
        {
            var snb = new SnippetBuilder();
            string snippet = snb.GenerateSnippet(keyword, stepName, multilineArgClass);

            var stb = new StringBuilder();
            var sw = new StringWriter(stb);
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

        private string SuccessResponse()
        {
            return "[\"success\",null]";
        }

        private string PendingResponse()
        {
            return "[\"pending\",null]";
        }

        private string FailResponse(string message)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
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

        private string FailResponse(Exception ex)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
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

        private string StepMatches(string stepName)
        {
            var matches = new JArray();
            foreach (StepDefinition sd in _repository.StepDefinitions)
            {
                List<StepArgument> args = sd.ArgumentsFrom(stepName);
                if (args != null)
                {
                    var stepMatch = new JObject();
                    stepMatch.Add("id", sd.Id);
                    var jsonArgs = new JArray();
                    foreach (StepArgument arg in args)
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

        private string Invoke(string id, string[] args)
        {
            try
            {
                StepDefinition stepDefinition = GetStepDefinition(id);

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
                    var ex = (TableAssertionException) x.InnerException;
                    return TableDiffResponse(ex.Expected, ex.Actual);
                }
                return FailResponse(x.InnerException);
            }
        }

        private string TableDiffResponse(Table expectedTable, Table actualTable)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof (Table));
            string expectedTableJson = converter.ConvertToString(expectedTable);
            string actualTableJson = converter.ConvertToString(actualTable);
            return String.Format("[\"diff!\", [{0},{1}]]", expectedTableJson, actualTableJson);
        }

        private StepDefinition GetStepDefinition(string id)
        {
            return _repository.StepDefinitions.Find(s => s.Id == id);
        }
    }
}