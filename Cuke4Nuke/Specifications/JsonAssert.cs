using LitJson;

using NUnit.Framework;

namespace Cuke4Nuke.Specifications
{
    public static class JsonAssert
    {
        public static void HasString(JsonData jsonData, string name, string value)
        {
            Assert.That(jsonData[name].IsString);
            Assert.That(jsonData[name].ToString(), Is.EqualTo(value));
        }

        public static void HasString(JsonData jsonData, string name)
        {
            Assert.That(jsonData[name].IsString);
        }

        public static void IsObject(JsonData jsonData)
        {
            Assert.That(jsonData.IsObject);
        }

        public static void IsArray(JsonData jsonData)
        {
            Assert.That(jsonData.IsArray);
        }
    }
}