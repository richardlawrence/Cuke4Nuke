Feature: Load config file for step definition DLL

  Background:
    Given a standard Cucumber project directory structure
    And a file named "features/appconfig.feature" with:
      """
        Scenario: Load config
          Then the config file should load

      """
    And a file named "features/step_definitions/some_remote_place.wire" with:
      """
      host: localhost
      port: 3901

      """
    And a file named "bin/GeneratedStepDefinitions.dll.config" with:
      """
      <?xml version="1.0"?>
      <configuration>
        <appSettings>
          <add key="helloMessage" value="Hello Cuke4Nuke!" />
        </appSettings>
      </configuration>
      """

    Scenario: A passing step
      Given a step definition assembly containing:
        """
        public class GeneratedSteps
        {
          [Then("^the config file should load$")]
          public static void ConfigFileShouldLoad()
          {
            string expectedValue = "Hello Cuke4Nuke!";
            string actualValue = System.Configuration.ConfigurationManager.AppSettings["helloMessage"];
            if (actualValue != expectedValue)
            {
              throw new Exception(
                String.Format(
                  "Config setting missing or incorrect. Expected <{0}>, got <{1}>.",
                  expectedValue,
                  actualValue
                )
              );
            }
          }
        }
        """
      When I run the cuke4nuke wrapper
      Then STDERR should be empty
      And it should pass with
        """
        .

        1 scenario (1 passed)
        1 step (1 passed)

        """