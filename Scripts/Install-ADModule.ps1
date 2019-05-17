#requires -RunAsAdministrator
<#
.SYNOPSIS
Installs the ActiveDirectory module in Powershell.
.DESCRIPTION
Does the following tasks:
- Checks that the OS is Windows 10
- Installs RSAT if not already installed
- Updates help for ActiveDirectory
.NOTES
Requires Windows 10.
Requires an elevated host.
Requires an internet connection.

I have no idea if this works.
#>
Function Install-ADModule {
    [CmdletBinding()]
    Param()

    # Script needs internet
    If (!(Test-Connection 8.8.8.8 -Quiet)) {
        #throw [System.Exception] "Internet is required for installation"
        Write-Warning "Internet is required for installation!"
        break
    }

    # Script only works in Windows 10, make sure it it the current OS
    If ((Get-CimInstance Win32_OperatingSystem).Caption -notlike "*Windows 10*") {
        #throw [System.Exception] "OS must be Windows 10"
        Write-Warning 'OS must be Windows 10!'
        break
    }

    # Checks that RSAT is not installed, otherwise install it
    If (Get-HotFix -Id KB2693643 -ErrorAction SilentlyContinue) {
        Write-Verbose "---RSAT already installed"
    } Else {
        Write-Verbose "---Downloading RSAT"

        # Checks the architecture and selects the correct version of RSAT
        If ((Get-CimInstance Win32_ComputerSystem).SystemType -like "x64*") {
            $dl = 'WindowsTH-KB2693643-x64.msu'
        } Else {
            $dl = 'WindowsTH-KB2693643-x86.msu'
        }

        Write-Verbose "---Download finished"

        # Downloads the RSAT
        $BaseURL = 'https://download.microsoft.com/download/1/D/8/1D8B5022-5477-4B9A-8104-6A71FF9D98AB/'
        $URL = $BaseURL + $dl
        $Destination = Join-Path -Path $HOME -ChildPath "Downloads\$dl"
        $WebClient = New-Object System.Net.WebClient
        $WebClient.DownloadFile($URL,$Destination)
        $WebClient.Dispose()

        # Installs the RSAT
        Write-Verbose '---Installing RSAT'
        wusa.exe $Destination /quiet /norestart /log:$home\Documents\RSAT.log

        # Until done installing, keep writing dots
        Write-Host "Installing" -NoNewLine
        do {
            Write-Host "." -NoNewLine
            Start-Sleep -Seconds 5
        } until (Get-HotFix -Id KB2693643 -ErrorAction SilentlyContinue)
        Write-Host "."
        Write-Verbose "---Installation finished"
    }

    # Since enabling the RSAT was not working in the original, there is no reason to keep it

    # Download / Update the help for ActiveDirectory
    Write-Verbose "---Updating help for ActiveDirectory"
    $isVerbose = $false
    If ($PSBoundParameters.ContainsKey('Verbose')) {
        $isVerbose = $PsBoundParameters.Get_Item('Verbose')
    }
    Update-Help -Module ActiveDirectory -Verbose:$isVerbose -Force

}

# Launch function
Install-ADModule -Verbose