Cuke4Nuke
=========

Cuke4Nuke is a project to allow [Cucumber](http://cukes.info/) to support step definitions written in .NET. It uses a simple wire protocol for Cucumber to issue commands to a .NET server that knows how to read and invoke .NET step definitions. 

The goal: to make Cucumber usable for .NET teams who can't or don't want to work directly with Ruby.

**WARNING** - Cuke4Nuke is very much in an alpha state. Anything could change.

See [https://rspec.lighthouseapp.com/projects/16211-cucumber/tickets/428-wire-protocol](https://rspec.lighthouseapp.com/projects/16211-cucumber/tickets/428-wire-protocol) for more information about the development of the Cucumber wire protocol. 

## Why "Cuke4Nuke"?

The native Java step definition support for Cucumber is called Cuke4Duke, after the Java mascot, Duke. When we started work on this project at AA-FTT 2009, we had to call it something. .NET versions of Java tools tend to get an N added somewhere, hence Cuke4Nuke. That's all.

## How to Use It

There are two sides to using Cuke4Nuke: the Cucumber side and the .NET side.

The Cucumber side has a directory structure like this:
    MyProject/features/
    MyProject/features/some_feature.feature
    MyProject/features/another_feature.feature
    MyProject/features/step_definitions/
    MyProject/features/step_definitions/cucumber.wire

The directories and the *.feature files are normal Cucumber stuff. The interesting bit is the .wire file. This tells Cucumber to go look for step definitions using the wire protocol instead of using the normal Ruby files. The .wire file has only two lines:
    host: localhost
    port: 3901
which specify the host and port to find the wire server with the Cucumber step definitions.
    
You'll need a version of Cucumber that includes the wire protocol language, such as this one [http://github.com/mattwynne/cucumber/tree/wire_protocol](http://github.com/mattwynne/cucumber/tree/wire_protocol).

On the .NET side you need the Cuke4Nuke server and at least one DLL with step definitions. Step definitions are static methods with one of the Cuke4Nuke.Framework step definition attributes (Given, When, Then, And, or But). For example:
    [Then("^it should pass.$")]
    public static void ItShouldPass()
    {
        Assert.Pass();
    }
(The assertion is from NUnit, which is the only test framework Cuke4Nuke currently understands how to work with.)

To use this step definition, you'd start the Cuke4Nuke server as follows:
    > Cuke4Nuke.Server.exe -p 3901 -a MyStepDefinitionLibrary.dll

Assuming you have a feature file that includes
    Then it should pass.
when you run
    > cucumber
it should cause Cuke4Nuke to invoke your ItShouldPass() method and show a successful test.

Currently, Cuke4Nuke can't handle step parameters, hooks, or shared state between steps, so you can't do anything very interesting with it yet. But soon...

## Contributing

Of course, you're welcome to fork this repository, make changes, and send a pull request. But if you want to coordinate your work with the others contributing to the project, speak up on the [cukes mailing list](http://groups.google.com/group/cukes) with what you intend to work on, and I'll update the [Issues](http://github.com/richardlawrence/Cuke4Nuke/issues) list accordingly.
