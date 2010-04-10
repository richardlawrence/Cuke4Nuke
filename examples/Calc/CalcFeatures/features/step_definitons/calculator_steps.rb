if Cucumber::IRONRUBY # We only want this to run on IronRuby

require 'spec/expectations'
$:.unshift(File.dirname(__FILE__) + '/../../bin/Debug') # So we find the .dll
require 'Calc.dll'

Before do
  @calc = Calc::Calculator.new
end

Given "I have entered $n into the calculator" do |n|
  @calc.Push(n.to_f)
end

When /I press divide/ do
  @result = @calc.Divide
end

When /I press add/ do
  @result = @calc.Add
end

Then /the result should be (.*) on the screen/ do |result|
  @result.should == result.to_f
end

end