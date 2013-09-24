require 'bundler/setup'
require 'albacore'

@acceptance_assemblies = Dir["**/bin/Debug/*Acceptance*.dll"]
@test_assemblies = Dir["**/bin/Debug/*Test*.dll"] - @acceptance_assemblies

@deploy_target = "C:/Sites/FundTracker"

task :check do
	puts @test_assemblies
end

task :default => [:build]

msbuild :build do |msb|
	msb.properties = {:configuration => :Release }
	msb.solution = "FundTracker.sln"
end

nunit :test => :build do |nunit|
	nunit.command = "nunit-console.exe"
	nunit.assemblies = @test_assemblies
end

nunit :acceptance => :build do |nunit|
	nunit.command = "nunit-console.exe"
	nunit.assemblies = @acceptance_assemblies
end

