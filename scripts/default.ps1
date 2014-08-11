properties {
    #$BuildScriptsPath = Resolve-Path .
    $base_dir = Resolve-Path ..
    $packages = "$base_dir\packages"
    $build_dir = "$base_dir\build"
    $scripts_dir = "$base_dir\scripts"
    $sln_file = "$base_dir\src\Applified.NET.sln"
    $src_dir = "$base_dir\src"

    $environment = ""   
	$revision = "12"    
	$version = "1.0.0." 
	 
	$platform = "x64"
	$config = "Release"

}

Framework "4.5.1x64"

task default -depends CleanUp, Version, Compile, Zip


Task Zip -Description "Package the output" {
    . "$scripts_dir\zip.ps1"
    out-zip $build_dir
}

Task Version -Description "Version the assemblies" {
    . "$scripts_dir\versioning.ps1"

	Update-AssemblyInfoFiles $src_dir $version$revision
}


task CleanUp -Description "Clean the build directory" {
    @($build_dir) | Where-Object { Test-Path $_ } | ForEach-Object {
        Write-Host "Cleaning folder $_..."
        Remove-Item $_ -Recurse -Force -ErrorAction Stop
    }
}
 
task Compile -Description "Build the solution" {
    Write-Host $build_dir
    if (-not ($build_dir.EndsWith("\"))) { 
        $build_dir = $build_dir + '\'
    } 
  
    if ($build_dir.Contains(" ")) { 
        $build_dir = """$($build_dir)\""" #read comment from Johannes Rudolph here: http://www.markhneedham.com/blog/2008/08/14/msbuild-use-outputpath-instead-of-outdir/ 
    } 

    Write-Host "Compiling $sln_file in $BuildConfiguration mode to $build_dir"
    Exec { & msbuild """$sln_file"" /t:Clean /t:Build /p:Configuration=$config /m /nr:false /v:q /nologo /p:OutDir=$build_dir" }
}


