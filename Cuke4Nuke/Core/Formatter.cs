using System;
using System.Collections.Generic;
using System.Text;

using LitJson;

namespace Cuke4Nuke.Core
{
    public class Formatter
    {
        public string Format(IEnumerable<StepDefinition> stepDefinitions)
        {
            return Format(writer => Write(writer, stepDefinitions));
        }

        public string Format(StepDefinition stepDefinition)
        {
            return Format(writer => Write(writer, stepDefinition));
        }

        public string Format(Exception exception)
        {
            return "FAIL:" + Format(writer => Write(writer, exception));
        }

        public string Format(string failedMessage)
        {
            return "FAIL:" + Format(writer => Write(writer, failedMessage));
        }

        static string Format(Action<JsonWriter> write)
        {
            var sb = new StringBuilder();
            write(new JsonWriter(sb));
            return sb.ToString();
        }

        static void Write(JsonWriter writer, IEnumerable<StepDefinition> stepDefinitions)
        {
            WriteArray(writer, () =>
                {
                    foreach (var stepDefinition in stepDefinitions)
                        Write(writer, stepDefinition);
                });
        }

        static void Write(JsonWriter writer, StepDefinition stepDefinition)
        {
            WriteObject(writer, () =>
                {
                    WriteProperty(writer, "id", stepDefinition.Id);
                    WriteProperty(writer, "regexp", stepDefinition.Pattern);
                });
        }

        static void Write(JsonWriter writer, Exception exception)
        {
            WriteObject(writer, () =>
                {
                    WriteProperty(writer, "exception", exception.GetType().ToString());
                    WriteProperty(writer, "message", exception.Message);
                    WriteProperty(writer, "backtrace", exception.StackTrace);
                });
        }

        static void Write(JsonWriter writer, string failedMessage)
        {
            WriteObject(writer, () =>
                {
                    WriteProperty(writer, "message", failedMessage);
                });
        }

        static void WriteArray(JsonWriter writer, Action writeArrayDetails)
        {
            writer.WriteArrayStart();
            writeArrayDetails();
            writer.WriteArrayEnd();
        }

        static void WriteObject(JsonWriter writer, Action writeObjectDetails)
        {
            writer.WriteObjectStart();
            writeObjectDetails();
            writer.WriteObjectEnd();
        }

        static void WriteProperty(JsonWriter writer, string name, string value)
        {
            writer.WritePropertyName(name);
            writer.Write(value);
        }
    }
}