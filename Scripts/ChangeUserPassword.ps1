Set-ADAccountPassword -Identity '{part}' -Credential $credential -Reset -NewPassword (ConvertTo-SecureString -AsPlainText '{newPassword}' -Force)
Write-Host $?