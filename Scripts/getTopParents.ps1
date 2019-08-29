#requires -Module ActiveDirectory

<#
.SYNOPSIS
Returns an array containing the parents of the group
.DESCRIPTION
Gets the groups which contain the input group
.NOTES
Requires the ActiveDirectory module
#>
Function Get-GroupParents {
    Param(
        # Name of the group to check
        [Parameter(Mandatory=$true)]
        [String]
        $GroupName
    )
    Import-Module ActiveDirectory

    $grouplist = [String[]] (Get-ADGroup -Identity $GroupName -Properties memberof).memberof

    # Remove the unused parts, returning only the group
    # Hopefully the group does not contain ',' or '='
    For($i = 0; $i -lt $grouplist.Length; $i++) {
        $grouplist[$i] = $grouplist[$i].Split(',')[0].Split('=')[1]
    }

    Return $grouplist
}

<#
.SYNOPSIS
Returns an array of the ad groups that do not have parents
.DESCRIPTION
Checks all ad groups for a parent, and returns them if they do not have any.
Takes a while to work.
#>
Function Get-TopParents {
    $groups = Get-ADGroup -Filter "name -like '*'" | ForEach-Object{$_.name}
    # Storage for the parentless groups
    $topgroups = @()

    Foreach($group in $groups) {
        # Converts 0 parents to $false, any to $true
        $hasParents = [bool] (Get-GroupParents -GroupName $group).Length
        If(-not $hasParents) {
            $topgroups += $group
        }
    }

    Return $topgroups
}

Get-TopParents | Out-String