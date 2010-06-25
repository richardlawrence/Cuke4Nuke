namespace Cuke4Nuke.Framework
{
    public class AfterAttribute : HookAttribute
    {
        public AfterAttribute()
            : base()
        {
        }

        public AfterAttribute(string tag)
            : base(tag)
        {
        }
    }
}