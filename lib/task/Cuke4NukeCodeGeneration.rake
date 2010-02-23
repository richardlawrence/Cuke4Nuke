

namespace :generate do
  desc "Generateing cuke4nuke attributes for all i18n supported languages in Gherkin"
  task :attributes do
	require 'gherkin/i18n'
	require 'erb'

	csharp = ERB.new(IO.read(File.dirname(__FILE__) + '/../code_template/C#_template.cs.erb'), nil, '-')
	File.open('Cuke4Nuke/Framework/languages/steps.cs', 'wb') do |io|
		Gherkin::I18n.all.each do |i18n_language|
		  language = i18n_language.sanitized_key.upcase
		  keywords = i18n_language.gwt_keywords.reject{|kw| kw =~ /\*/ }.map do |key|
			key.gsub(/[ |']/, "")
		  end
		  
		  io.write(csharp.result(binding))
		end
	end

  end
end
