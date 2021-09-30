$baseUrl = "http://$($env:registryhost)/v2"

$imageRepo = Invoke-RestMethod -Uri "$baseUrl/_catalog"

foreach($repository in $imageRepo.repositories)
{
    
    $images = Invoke-RestMethod -Uri "$baseUrl/$repository/tags/list"
    "Image: $($images.name)"
    foreach($tag in $images.tags)
    {
        "  Tag: $tag"
        $manifests = Invoke-RestMethod -Uri "$($baseUrl)/$($repository)/manifests/$($tag)"
        
        foreach($item in $manifests.signatures)
        {
                "    Signature: $($item.signature)"
        }
    }
}