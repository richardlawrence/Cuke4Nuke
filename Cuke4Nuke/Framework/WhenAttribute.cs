namespace Cuke4Nuke.Framework
{
    public class WhenAttribute : StepDefinitionAttribute
    {
        public WhenAttribute(string pattern) : base(pattern)
        {
        }
    }
}