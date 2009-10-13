Given /^Cuke4Nuke started with no step definition assemblies$/ do
  run_in_background cuke4nuke_server_exe
end

Given /^Cuke4Nuke started with a step definition assembly containing:$/ do |contents|
  assembly_path = build_step_definitions(contents)
  run_in_background "#{cuke4nuke_server_exe} -a #{assembly_path}"
end