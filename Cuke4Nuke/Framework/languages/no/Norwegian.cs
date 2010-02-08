namespace Cuke4Nuke.Framework.Languages.Norwegian
{
    public class GittAttribute : GivenAttribute
    {
        public GittAttribute(string pattern) : base(pattern)
        {
        }
    }

    public class NårAttribute : WhenAttribute
    {
        public NårAttribute(string pattern): base(pattern)
        {
        }
    }

    public class SåAttribute : ThenAttribute
    {
        public SåAttribute(string pattern): base(pattern)
        {
        }
    }
}
