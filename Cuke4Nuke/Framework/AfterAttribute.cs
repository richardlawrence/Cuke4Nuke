namespace Cuke4Nuke.Framework
{
    public class AfterAttribute : HookAttribute
    {
        public AfterAttribute()
            : base()
        {
        }

        public AfterAttribute(params string[] tags)
            : base(tags)
        {
        }
    }
}