# Since we can't use versioning easy on local deploys we need to deploy this way
# Create a tempdir, copy all deploy files there, change the buildid used by Azure Devops to use 'latest' instead
$registryHost = $env:REGISTRYHOST
$buildId = "latest"

$temp = [System.IO.Path]::GetTempPath()
$guid = [System.Guid]::NewGuid().ToString()
$tempDir = "$temp$guid"
	
New-Item -Name $guid -Path $temp -ItemType Directory
$tempDir

Copy-Item -Path .\deploy\* -Destination $tempDir -Recurse

$files = Get-ChildItem -Path $tempDir\*.yaml -Recurse -Force

foreach ($file in $files) {
	$content = Get-Content -Path $file
	$content = $content -replace '#{Build.BuildId}#', 'latest'
	$content | Set-Content -Path $file
}

#Remove All with Prometheus, Grafana, Loki, Nats and applications
#kubectl delete -k $tempDir/all

#Remove Applications only
#kubectl delete -k $tempDir/apps 

#Remove One by one
kubectl delete -k $tempDir/smhiextractor
kubectl delete -k $tempDir/smhiapi

#Deploy All with Prometheus, Grafana, Loki, Nats and applications
#kubectl apply -k $tempDir/all

#Deploy Applications only
#kubectl apply -k $tempDir/apps

#Deploy One by one
kubectl apply -k $tempDir/smhiapi
kubectl apply -k $tempDir/smhiextractor

#Clean up temp dir
Remove-Item $tempDir -recurse -force