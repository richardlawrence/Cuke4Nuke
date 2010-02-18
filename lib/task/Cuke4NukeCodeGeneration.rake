

namespace :generate do
  desc "Generateing cuke4nuke attributes for all i18n supported languages in Gherkin"
  task :attributes do
    require "gherkin"
    Gherkin::CodeGenerator.new("lib/code_template/C#_template.cs.erb").write_to_file("Cuke4Nuke/Framework/languages/steps.cs")
  end
end
