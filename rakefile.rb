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

task :deploy => [:build, :test] do
	
	FileUtils.rm_rf("#{@deploy_target}/*") if Dir.exists?(@deploy_target)
	
	FileUtils.mkdir(@deploy_target) unless Dir.exists?(@deploy_target)
	FileUtils.chmod("a=rwx", @deploy_target)

	puts "Deploying to #{@deploy_target}"

	Dir.glob("FundTracker.Web/*").each do |file|
		dir, filename = File.dirname(file), File.basename(file)
		dest = File.join(@deploy_target, dir)
		FileUtils.mkdir(dest) unless Dir.exists?(dest)
		FileUtils.cp_r(file, File.join(dest,filename))
	end
	
	puts "Deployed"
end

