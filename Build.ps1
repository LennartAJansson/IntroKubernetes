docker build -f .\WeatherApi\Dockerfile --force-rm -t weatherapi .
docker build -f .\SmhiApi\Dockerfile --force-rm -t smhiapi .
docker build -f .\SmhiExtractor\Dockerfile --force-rm -t smhiextractor .

docker tag weatherapi:latest $env:registryhost/weatherapi:latest
docker tag smhiapi:latest $env:registryhost/smhiapi:latest
docker tag smhiextractor:latest $env:registryhost/smhiextractor:latest

docker push $env:registryhost/weatherapi:latest
docker push $env:registryhost/smhiapi:latest
docker push $env:registryhost/smhiextractor:latest

#& .\Deploy.ps1