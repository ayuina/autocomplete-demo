
#$basepath = "http://35.190.231.31:9200"
#$basepath = "http://35.200.21.178:9200"
$basepath = "http://localhost:9200"

function Get-ElasticHealth()
{
    $url = "$basepath/_cat/health?pretty"
    Get-Simple $url
}

function Get-ElasticIndices()
{
    $url = "$basepath/_cat/indices?v"
    Get-Simple $url
}

function Get-Document($indexname, $typename, $id)
{
    $url = "$basepath/$indexname/$typename/$id" + "?pretty"
    Get-Simple $url
}

function Get-Mapping($indexname, $typename)
{
    $url = "$basepath/$indexname/$typename/_mapping" + "?pretty"
    Get-Simple $url
}

function Get-Simple($url)
{
    (Invoke-WebRequest -Uri $url -Method get -UseBasicParsing).RawContent 
}


function Search-Elastic($indexname, $typename, $query)
{
    $url = "$basepath/$indexname/$typename/_search"
    $body = $query | convertto-json
    write-host $body
    $ret = (Invoke-WebRequest -Uri $url -Method post -ContentType "application/json" -Body $body  -UseBasicParsing)
    write-host $ret.RawContent
    write-output (convertfrom-json $ret.Content)
}

function Create-ElasticQuery()
{
    $base = "{ 'query' : { 'match' : { 'name' : 'Xbox' } } }"
    return ConvertFrom-Json $base
}