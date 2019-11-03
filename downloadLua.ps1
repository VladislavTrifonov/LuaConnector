$client = new-object System.Net.Webclient
$client.DownloadFile("https://netix.dl.sourceforge.net/project/luabinaries/5.3.5/Tools%20Executables/lua-5.3.5_Win32_bin.zip", "$(System.DefaultWorkingDirectory)/lua-5.3.5_Win32_bin.zip")
Write-Host "Xer"
Write-Host "$(System.DefaultWorkingDirectory)"
Write-Host "Xer"
