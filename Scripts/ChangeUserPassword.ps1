$password = ConvertTo-SecureString -AsPlainText '{newPassword}' -Force
$credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList '{part}', $password
Set-ADAccountPassword -Identity '{part}' -Credential $credential -Reset -NewPassword $password
Write-Host $?