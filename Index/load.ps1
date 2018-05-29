


#$basepath = "http://35.190.231.31:9200"
$basepath = "http://localhost:9200"

function Main()
{
    #Start-Elastic
    Load-ProductData
}

function Load-ProductData()
{
    $indexname = "sample"
    $typename = "product"
    $products = Get-Content .\products.json -Raw | ConvertFrom-Json 

    $i = 0
    $products | foreach {
        $upc = $_.upc
        write-host ("{0} Inserting -- {1}:{2}" -f $i++, $upc, $_.name )
        $record = $_ | ConvertTo-Json

        $url = "$basepath/$indexname/$typename/$upc" + "?pretty"
        
        $ret = (Invoke-WebRequest $url -Method Put -ContentType "application/json" -Body $record -UseBasicParsing).Content
    }
}

function Create-Index($indexname)
{
    $url = "$basepath/$indexname" + "?pretty"
     (Invoke-WebRequest $url -Method Put).RawContent
}

function Start-Elastic()
{
    Write-Host "starting container"
    Start-Process "docker" -ArgumentList "run", "-it", "-d", "-p 9200:9200", "myelastic" -Wait -NoNewWindow
    $wait = $true
    while($wait)
    {
        try {
            $result = Invoke-WebRequest -Method get -UseBasicParsing  "$basepath/_cat/health?pretty"
            if($result -ne $null)
            {
                $wait = $false
                Write-Host $result.Content
            }
        }
        catch {
            Write-Host "Waiting for idle ..."
            Start-Sleep -Seconds 2
        }
   }
}

Main