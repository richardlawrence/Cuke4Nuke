namespace Cuke4Nuke.Framework
{
    public class BeforeAttribute : HookAttribute
    {
        public BeforeAttribute()
            : base()
        {
        }

        public BeforeAttribute(string tag)
            : base(tag)
        {
        }
    }
}
