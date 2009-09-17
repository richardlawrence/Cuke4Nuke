using System.Collections.Generic;

namespace Cuke4Nuke.Core
{
    public class Factory
    {
        public Options Options { get; private set; }

        public Factory(Options options)
        {
            Options = options;
        }

        public virtual Loader GetLoader()
        {
            return new Loader(Options);
        }

        public virtual Processor GetProcessor(List<StepDefinition> stepDefinitions)
        {
            return new Processor(stepDefinitions);
        }

        public virtual Listener GetListener(Processor processor)
        {
            return new Listener(processor, Options);
        }
    }
}