task :default => :build

task :build do
  msbuild = 'c:\windows\microsoft.net\framework\v3.5\msbuild.exe'
  solution = File.expand_path(File.dirname(__FILE__) + '/Cuke4Nuke/Cuke4Nuke.sln')
  sh %{#{msbuild} "#{solution}" /p:configuration=Release}
end

task :test do
  nunit = 'C:\Program Files\NUnit 2.5.2\bin\net-2.0\nunit-console.exe'
  tests = File.expand_path(File.dirname(__FILE__) + '/Cuke4Nuke/Specifications/bin/Release/Cuke4Nuke.Specifications.dll')
  sh %{"#{nunit}" "#{tests}"}
end