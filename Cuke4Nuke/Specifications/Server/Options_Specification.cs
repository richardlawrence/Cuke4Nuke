using System;
using System.IO;

using Cuke4Nuke.Server;

using NUnit.Framework;

namespace Cuke4Nuke.Specifications.Server
{
    [TestFixture]
    public class Options_Specification
    {
        [Test]
        public void Port_should_default_correctly()
        {
            var options = new Options("");
            Assert.That(options.Port, Is.EqualTo(Options.DefaultPort));
        }

        [Test]
        public void Should_parse_p_into_port()
        {
            var options = new Options("-p=1234");
            Assert.That(options.Port, Is.EqualTo(1234));
        }

        [Test]
        public void Should_parse_port_into_port()
        {
            var options = new Options("-port=1234");
            Assert.That(options.Port, Is.EqualTo(1234));
        }

        [Test]
        public void ShowHelp_should_default_to_false()
        {
            var options = new Options("");
            Assert.That(options.ShowHelp, Is.False);
        }

        [Test]
        public void Should_parse_h_into_ShowHelp()
        {
            var options = new Options("-h");
            Assert.That(options.ShowHelp, Is.True);
        }

        [Test]
        public void Should_parse_question_mark_into_ShowHelp()
        {
            var options = new Options("-h");
            Assert.That(options.ShowHelp, Is.True);
        }

        [Test]
        public void Should_parse_help_into_ShowHelp()
        {
            var options = new Options("-help");
            Assert.That(options.ShowHelp, Is.True);
        }

        [Test]
        public void AssemblyPaths_should_default_to_empty_collection()
        {
            var options = new Options("");
            Assert.That(options.AssemblyPaths.Count, Is.EqualTo(0));
        }

        [Test]
        public void Should_parse_a_into_AssemblyPaths()
        {
            var options = new Options("-a=foo");
            Assert.That(options.AssemblyPaths.Count, Is.EqualTo(1));
            Assert.That(options.AssemblyPaths.Contains("foo"));
        }

        [Test]
        public void Should_parse_assembly_into_AssemblyPaths()
        {
            var options = new Options("-assembly=foo");
            Assert.That(options.AssemblyPaths.Count, Is.EqualTo(1));
            Assert.That(options.AssemblyPaths.Contains("foo"));
        }

        [Test]
        public void Should_parse_multiple_assembly_options_into_AssemblyPaths()
        {
            var options = new Options("-a=foo", "-a=bar");
            Assert.That(options.AssemblyPaths.Count, Is.EqualTo(2));
            Assert.That(options.AssemblyPaths.Contains("foo"));
            Assert.That(options.AssemblyPaths.Contains("bar"));
        }

        [Test]
        public void Write_should_write_options_label_line()
        {
            var output = new StringWriter();
            var options = new Options();
            options.Write(output);
            Assert.That(output.ToString().StartsWith("Options:" + Environment.NewLine));
        }

        [Test]
        [Explicit("Supports exploratory testing of options")]
        public void Write_should_write_options_label_lin()
        {
            var options = new Options();
            options.Write(Console.Out);
        }
    }
}