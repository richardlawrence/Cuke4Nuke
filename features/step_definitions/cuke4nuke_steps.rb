Given /^Cuke4Nuke started with no step definition assemblies$/ do
  run_in_background cuke4nuke_server_exe
end

Given /^Cuke4Nuke started with a step definition assembly containing:$/ do |contents|
  assembly_path = build_step_definitions(contents)
  run_in_background %{#{cuke4nuke_server_exe} -a "#{assembly_path}"}
end

Given /^a step definition assembly containing:$/ do |contents|
  @last_assembly_path = build_step_definitions(contents)
end

When /^I run the cuke4nuke wrapper$/ do
  ruby_path = Cucumber::RUBY_BINARY.gsub('/', '\\')
  run %{"#{ruby_path}" "#{cuke4nuke_wrapper_path}" "#{@last_assembly_path}" -b --no-color -f progress features}
end