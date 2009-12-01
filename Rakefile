task :default => :build

task :build do
  msbuild = 'c:\windows\microsoft.net\framework\v3.5\msbuild.exe'
  solution = File.expand_path(File.dirname(__FILE__) + '/Cuke4Nuke/Cuke4Nuke.sln')
  sh %{#{msbuild} "#{solution}" /p:configuration=Release}
end