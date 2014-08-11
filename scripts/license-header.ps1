
function Ensure-LicenseHeader($sourceDirectory, $filter, $headerFile) {
    $items = Get-ChildItem $sourceDirectory -Recurse -Filter $filter -ErrorAction SilentlyContinue -ErrorVariable err
 
    foreach ($errorRecord in $err)
    {
        if ($errorRecord.Exception -is [System.IO.PathTooLongException])
        {
            Write-Warning "Path too long in directory '$($errorRecord.TargetObject)'."
        }
        else
        {
            Write-Error -ErrorRecord $errorRecord
        }
    }

    $header = Get-Content -LiteralPath $headerFile
    
    foreach ($item in $items) 
    {
        $content = Get-Content $item
        $prependHeader = $true;

        foreach ($line in $content) 
        {
        
            if ($line.StartsWith($header[0]) -eq $true) 
            {
                $prependHeader = $false;
                break;
            }
        }
        
        if ($prependHeader -eq $true) 
        {
            Set-Content $item $header 
            Add-Content $item $content

            Write-Host "Added license header to file $item"
        }
        else
        {
            Write-Warning "Skipping file $item"
        }
    }
}

Ensure-LicenseHeader -sourceDirectory "" -filter "*.cs" -headerFile "..\header-agpl.txt"