if($env:Path -notmatch "chocolatey")
{
	iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
	# setx PATH "%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"
}
else
{
	choco upgrade chocolatey
}

choco feature enable -n allowGlobalConfirmation
refreshenv

# 7-zip is basic for choco
choco install -force 7zip
refreshenv

# Two great terminals, conemu is excellent when working with powershell and different cli's
choco install -force conemu
choco install -force microsoft-windows-terminal
refreshenv

# Poshgit brings git closer to powershell and tortoisegit gives explorer interactivity with git
choco install -force poshgit
choco install -force tortoisegit
refreshenv

# Various support for working against Azure
choco install -force azure-cli
choco install -force microsoftazurestorageexplorer
choco install -force servicebusexplorer
choco install -force azure-pipelines-agent
choco install -force vsts-cli
refreshenv

# Amazon cli
# choco install awscli

# Docker desktop for windows
choco install -force docker-desktop

# Various Kubernetes tools
choco install -force kubernetes-cli
choco install -force kubernetes-helm
choco install -force k9s
choco install -force kustomize
choco install -force k3d

# Good to have when working against web api's etc
choco install -force curl
