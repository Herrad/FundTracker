require 'bundler/setup'
require 'albacore'

task :default => [:build, :deploy]

msbuild :build do |msb|
	msb.properties = {:configuration => :Debug }
	msb.solution = "FundTracker.sln"
end

task :deploy do
	create_web_site("FundTracker", "FundTracker.Web", "80")
end

def create_web_site(site_name, site_location, site_port)
  delete_command = "c:/windows/system32/inetsrv delete site #{site_name}"
  result = system delete_command
  puts "Failed to delete site on IIS: #{$?}" unless result

  add_command = "c:/windows/system32/inetsrv add site /name:#{site_name} /bindings:http/*:#{site_port}: /physicalPath:#{site_location}"
  result = system add_command
  raise "Failed to add site on IIS: #{$?}" unless result

  set_app_pool_command = "c:/windows/system32/inetsrv set app #{site_name}/ /applicationPool:\"ASP.NET v4.0\""
  result = system set_app_pool_command
  raise "Failed to bind site to .net 4 app pool on IIS: #{$?}" unless result

  start_site_command = "c:/windows/system32/inetsrv start site #{site_name}"
  result = system start_site_command
  raise "Failed to start site on IIS: #{$?}" unless result
end