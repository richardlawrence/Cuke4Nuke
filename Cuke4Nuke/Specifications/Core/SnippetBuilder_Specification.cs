using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cuke4Nuke.Core;

namespace Cuke4Nuke.Specifications.Core
{
    public class SnippetBuilder_Specification
    {
        [Test]
        public void StepNameToMethodName_ShouldReturnLegalMethodName()
        {
            SnippetBuilder sb = new SnippetBuilder();
            string stepName = "we're all wired";
            Assert.That(sb.StepNameToMethodName(stepName), Is.EqualTo("WereAllWired"));
        }

        [Test]
        public void StepNameToMethodName_ShouldReturnLegalMethodName_TrailingComma()
        {
            SnippetBuilder sb = new SnippetBuilder();
            string stepName = "the separator is ,";
            Assert.That(sb.StepNameToMethodName(stepName), Is.EqualTo("TheSeparatorIs"));
        }
    }
}
