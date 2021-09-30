# Docker desktop for windows
#choco install -force docker-desktop

# Add any other tools that will be needed
#choco install -force k3d
#choco install -force curl
#choco install -force kubernetes-cli
#choco install -force kubernetes-helm
#choco install -force kustomize
#choco install -force k9s

# K3d is a tool to generate a K3s environment in Docker.
# Create local kubernetes cluster, registry and loadbalancer containers, publish 8081 externally in loadbalancer
k3d cluster create k3slocal --volume C:\Data\SQLite:/tmp/shared@server[0] --kubeconfig-update-default --kubeconfig-switch-context --registry-create -p 8081:80@loadbalancer -p 4222:4222@server[0] -p 8222:8222@server[0] --api-port=16443 --wait --timeout=60s

# Check what host port your registry publish and save it in environment:
# From Powershell you can access it as $env:REGISTRYHOST
# From CMD-files you can access it as %REGISTRYHOST%
$port = (docker port k3d-k3slocal-registry).Split(':')[1]
$registryhost = "k3d-k3slocal-registry:$($port)"
SETX /M REGISTRYHOST $registryhost
$env:registryhost=$registryhost

# Pull, retag and push NATS/Jetstream to your repository
# docker pull synadia/jsm:latest
# docker tag synadia/jsm:latest $registryhost/synadia/jsm:latest
# docker push $registryhost/synadia/jsm:latest

# Verify that you see the NATS image here:
curl http://$registryhost/v2/_catalog

#Setup good to have deployments:
# kubectl apply -k Deploy/prometheus
# kubectl apply -k Deploy/grafana
# kubectl apply -k Deploy/nats

#Using Helm to install loki
# helm repo add grafana https://grafana.github.io/helm-charts
# helm repo update
# helm upgrade -n monitoring --install loki grafana/loki
#helm uninstall loki
# kubectl apply -k Deploy/loki


"Add following to C:\Windows\System32\drivers\etc\hosts: "
"127.0.0.1 k3d-k3slocal-registry"
"127.0.0.1 k3d-k3slocal-registry.local"
"127.0.0.1 smhiapi"
"127.0.0.1 smhiapi.local"
"127.0.0.1 prometheus"
"127.0.0.1 prometheus.local"
"127.0.0.1 grafana"
"127.0.0.1 grafana.local"
"127.0.0.1 loki"
"127.0.0.1 loki.local"
""
"To remove everything regarding cluster, loadbalancer and registry:"
"k3d cluster delete k3slocal"
""
"You should now be able to surf to:"
" http://prometheus.local:8081"
" http://grafana.local:8081/login"