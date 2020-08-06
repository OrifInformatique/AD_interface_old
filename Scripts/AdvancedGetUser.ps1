# Result to display
$result = @()
# Filter used 
$criteria = '{FilterSelection}'
$search = '{Part}'

foreach ($data in Get-ADUser -Filter "$criteria -like '$search'" -Properties *) {
    $obj = New-Object psobject
    $properties = [string[]] $data.PropertyNames
    foreach ($property in $properties) {
        Add-Member -InputObject $obj -MemberType NoteProperty -Name $property -Value ([string] $data[$property])
    }
    $result += $obj
}
$result | Out-String