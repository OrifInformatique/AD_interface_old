#requires -Version 4
#requires -Module CimCmdlets
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
Requires powershell at least version 4.
#>
Function Install-ADModule {
    [CmdletBinding()]
    Param()

    # Script needs internet
    If (!(Test-Connection 8.8.8.8 -Quiet)) {
        #throw [System.Exception] 'Internet is required for installation'
        Write-Warning "Internet est requis pour l'installation!"
        return
    }

    $caption = (Get-CimInstance Win32_OperatingSystem).Caption
    # Windows 10 has a specific file to run
    If($caption -like '*Windows 10*') {
        # From 1809 the file isn't needed to be downloaded anymore, just activate it
        If([int](Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion').ReleaseId -ge 1809) {
            Write-Host '---Installation du module AD'
            Add-WindowsCapability -Online -Name Rsat.ActiveDirectory.DS-LDS.Tools~~~~0.0.1.0
            Write-Host '---Module AD installé'
            return
        }
        $version = 'Windows 10'
        $targets = @('WindowsTH-KB2693643-x64.msu', 'WindowsTH-KB2693643-x86.msu')
        $RSAT = 'KB2693643'
    # Windows 7 has a different file to run
    } ElseIf($caption -like '*Windows 7*') {
        $version = 'Windows 7'
        $targets = @('Windows6.1-KB958830-x64-RefreshPkg.msu', 'Windows6.1-KB958830-x86-RefreshPkg.msu')
        $RSAT = 'KB958830'
    # Not Windows 7 or Windows 10, no file to run
    } Else {
        Write-Warning "Votre système d'exploitation n'est pas supporté"
        return
    }

    $install = !(Get-HotFix -Id $RSAT -ErrorAction SilentlyContinue)

    # Checks that RSAT is not installed, otherwise install it
    If ($install -and !!($version)) {
        Write-Host '---Téléchargement du module AD'

        $dl = $targets[!(Get-CimInstance Win32_ComputerSystem).SystemType -like 'x64*']

        # Downloads the RSAT
        $BaseURL = 'https://download.microsoft.com/download/'
        $BaseURL += Switch ($version) {
            'Windows 10' {'1/D/8/1D8B5022-5477-4B9A-8104-6A71FF9D98AB/'}
            'Windows 7' {'4/F/7/4F71806A-1C56-4EF2-9B4F-9870C4CFD2EE/'}
            default {
                Write-Warning 'Version inconnue'
                return;
            }
        }
        $URL = $BaseURL + $dl
        $Destination = Join-Path -Path $HOME -ChildPath "Downloads\$dl"
        $WebClient = New-Object System.Net.WebClient
        $WebClient.DownloadFile($URL,$Destination)
        $WebClient.Dispose()

        Write-Host '---Téléchargement terminé'

        # Installs the RSAT
        Write-Host '---Installation du module AD'
        wusa.exe $Destination /quiet /norestart /log:$home\Documents\RSAT.log

        # Until done installing, keep writing dots
        Write-Host 'Installation en cours' -NoNewLine
        do {
            # This part is extremely slow on Windows 7
            Write-Host '.' -NoNewLine
            Start-Sleep -Seconds 5
        } until (Get-HotFix -Id $RSAT -ErrorAction SilentlyContinue)
        Write-Host '.'
        Write-Host '---Installation terminée'
    }
    # Windows 7 needs to have RSAT activated afterward
    If($version -eq 'Windows 7') {
        Write-Host '---Activation du module AD'
        # This part does not work, so the user has to do it manually
        Dism.exe /online /get-features | Select-String -Pattern remote* | ForEach-Object {
            $Exec = "Dism.exe /online /enable-feature /featurename:RemoteServerAdministrationTools /featurename:" + ($_).ToString().Replace('Feature Name : ','')
            Invoke-expression $Exec
        }
        Write-Host '---Module AD activé'
    }

    # Download / Update the help for ActiveDirectory
    Write-Verbose "---Rafraichissement de l'aide pour le module AD"
    $isVerbose = $false
    If ($PSBoundParameters.ContainsKey('Verbose')) {
        $isVerbose = $PSBoundParameters.Get_Item('Verbose')
    }
    Update-Help -Module ActiveDirectory -Verbose:$isVerbose -Force
    Write-Verbose '---Rafraichissement terminé'
}

# Launch function
Install-ADModule