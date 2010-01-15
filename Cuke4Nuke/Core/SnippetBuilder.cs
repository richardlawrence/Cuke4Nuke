using System.Text;
using System.Text.RegularExpressions;
using System;

namespace Cuke4Nuke.Core
{
    public class SnippetBuilder
    {
        public string GenerateSnippet(string keyword, string stepName)
        {
            string multilineArgClass = String.Empty;
            return GenerateSnippet(keyword, stepName, multilineArgClass);
        }

        public string GenerateSnippet(string keyword, string stepName, string multilineArgClass)
        {
            string parameter = "";
            if (multilineArgClass == "Cucumber::Ast::Table")
            {
                parameter = "Table table";
            }
            else if (multilineArgClass == "Cucumber::Ast::PyString")
            {
                parameter = "string str";
            }

            string methodName = StepNameToMethodName(stepName);
            stepName = stepName.Replace("\"", "\"\"");
            StringBuilder sb = new StringBuilder();
            sb.Append("[Pending]\n");
            sb.AppendFormat("[{0}(@\"^{1}$\")]\n", keyword, stepName);
            sb.AppendFormat("public void {0}({1})\n", methodName, parameter);
            sb.Append("{\n");
            sb.Append("}");
            string snippet = sb.ToString();
            return snippet;
        }

        public string StepNameToMethodName(string stepName)
        {
            string s = stepName;
            s = Regex.Replace(s, @"[^A-Za-z0-9\s]", "");
            s = Regex.Replace(s, @"\s+", " ");
            s = s.Trim();
            string[] words = s.Split(' ');
            StringBuilder methodNameBuilder = new StringBuilder();
            foreach (string word in words)
            {
                methodNameBuilder.Append(word.Substring(0, 1).ToUpper());
                if (word.Length > 1)
                {
                    methodNameBuilder.Append(word.Substring(1).ToLower());
                }
            }
            return methodNameBuilder.ToString();
        }
    }
}
