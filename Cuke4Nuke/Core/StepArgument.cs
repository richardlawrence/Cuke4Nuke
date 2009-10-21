namespace Cuke4Nuke.Core
{
    class StepArgument
    {
        public string Val { get; private set; }
        public int Pos { get; private set; }

        public StepArgument(string val, int pos)
        {
            Val = val;
            Pos = pos;
        }
    }
}
