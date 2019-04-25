$result = @()

foreach ($data in Get-ADUser -Filter "{FilterSelection} -like '{part}'") {
    $obj = New-Object psobject
    $properties = [string[]] $data.PropertyNames
    for ($i = 0; $i -lt $properties.Length; $i++) {
        if ($properties[$i] -like 'Write*' -or $properties[$i] -like 'Object*') {
            continue
        }
        Add-Member -InputObject $obj -MemberType NoteProperty -Name $properties[$i] -Value ([string] $data[$properties[$i]])
    }
    $result += $obj
}
$result | Out-String