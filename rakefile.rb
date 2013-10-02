require 'bundler/setup'
require 'albacore'

@acceptance_assemblies = Dir["**/bin/Debug/*Acceptance*.dll"]
@test_assemblies = Dir["**/bin/Debug/*Test*.dll"] - @acceptance_assemblies

task :default => [:build, :test]

task :test => ["test:run_all"]
task :setup => ["setup:deploy"]

msbuild :build do |msb|
	msb.properties = {:configuration => :Release }
	msb.solution = "FundTracker.sln"
end

namespace "test" do
	task :run_all => [:unit, :acceptance]
	nunit :unit => :build do |nunit|
		nunit.command = "nunit-console.exe"
		nunit.assemblies = @test_assemblies
	end

	nunit :acceptance => :build do |nunit|
		nunit.command = "nunit-console.exe"
		nunit.assemblies = @acceptance_assemblies
	end
end

namespace "setup" do
	@appcmd = "c:/windows/system32/inetsrv/appcmd.exe"
	
	task :deploy => :build do
		site_name = "FundTracker.local"
		site_location = "\"#{File.expand_path("./FundTracker.Web")}\""
		set_up_site(site_name, site_location)
	end
	
	def set_up_site(site_name, site_location)
		puts "setting up IIS for site #{site_name}"
		
		puts "trying to delete site"
		system "#{@appcmd} delete site #{site_name}"
		
		puts "adding site pointing to #{site_location}"
		system "#{@appcmd} add site /name:#{site_name} /bindings:http/*:80: /physicalPath:#{site_location}"
		
		puts "configuring app pool for site"
		system "#{@appcmd} set app #{site_name}/ /applicationPool:\"ASP.NET v4.0\""
		
		puts "stopping default site"
		system "#{@appcmd} stop site \"Default Web Site\""
		
		puts "starting site"
		system "#{@appcmd} start site #{site_name}"
	end
end

