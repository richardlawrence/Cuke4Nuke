using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cuke4Nuke.Framework;
using System.Reflection;
using Cuke4Nuke.Core;

namespace Cuke4Nuke.Specifications.Core
{
    [TestFixture]
    public class Hook_TagLogic_Specification
    {
        [Test]
        public void OrLogicMatchingFirst()
        {
            var scenarioTags = new string[] { "@my_tag" };
            var method = Reflection.GetMethod(typeof(TaggedHooks), "BeforeWithMultipleTagsThrowsException_Or");
            var hook = new Hook(method);
            Assert.That(hook.MatchesTags(scenarioTags), Is.True);
        }

        [Test]
        public void OrLogicMatchingSecond()
        {
            var scenarioTags = new string[] { "@another_tag" };
            var method = Reflection.GetMethod(typeof(TaggedHooks), "BeforeWithMultipleTagsThrowsException_Or");
            var hook = new Hook(method);
            Assert.That(hook.MatchesTags(scenarioTags), Is.True);
        }

        [Test]
        public void OrLogicNonMatching()
        {
            var scenarioTags = new string[] { "@something_else" };
            var method = Reflection.GetMethod(typeof(TaggedHooks), "BeforeWithMultipleTagsThrowsException_Or");
            var hook = new Hook(method);
            Assert.That(hook.MatchesTags(scenarioTags), Is.False);
        }

        public class TaggedHooks
        {
            [Before("@my_tag,@another_tag")]
            public static void BeforeWithMultipleTagsThrowsException_Or()
            {
                throw new Exception();
            }

            [Before("@my_tag", "@another_tag", "a_third_tag")]
            public static void BeforeWithMultipleTagsThrowsException_And()
            {
                throw new Exception();
            }

            [Before("@my_tag,@another_tag", "a_third_tag")]
            public static void BeforeWithMultipleTagsThrowsException_OrAnd()
            {
                throw new Exception();
            }

            [Before("@my_tag")]
            private void BeforeWithTagThrowsException()
            {
                throw new Exception();
            }
        }
    }
}
