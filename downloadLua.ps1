$client = new-object System.Net.Webclient
$client.DownloadFile("https://netix.dl.sourceforge.net/project/luabinaries/5.3.5/Tools%20Executables/lua-5.3.5_Win32_bin.zip", "lua-5.3.5_Win32_bin.zip")
Write-Host "Hello World from $Env:AGENT_NAME."
Write-Host "My ID is $Env:AGENT_ID."
Write-Host "AGENT_WORKFOLDER contents:"
gci $Env:AGENT_WORKFOLDER
Write-Host "AGENT_BUILDDIRECTORY contents:"
gci $Env:AGENT_BUILDDIRECTORY
Write-Host "BUILD_SOURCESDIRECTORY contents:"
gci $Env:BUILD_SOURCESDIRECTORY
Write-Host "Over and out."
