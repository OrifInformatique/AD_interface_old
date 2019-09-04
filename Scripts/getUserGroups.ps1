<#
.SYNOPSIS
Returns an array containing the parents of the group
.DESCRIPTION
Gets the groups which contain the input group
.NOTES
Requires the ActiveDirectory module
.PARAMETER <GroupName>
The name of the parent group
.OUTPUTS
An array containing all parent groups
#>
Function Get-GroupParents {
    Param(
        # Name of the group to check
        [Parameter(Mandatory=$true)]
        [String]
        $GroupName
    )
    Import-Module ActiveDirectory

    Try {
        $grouplist = [String[]] (Get-ADGroup -Identity $GroupName -Properties memberof).memberof
    } Catch {
        Return @()
    }

    # Remove the unused parts, returning only the group
    # Hopefully the group does not contain ',' or '='
    For ($i = 0; $i -lt $grouplist.Length; $i++) {
        $grouplist[$i] = $grouplist[$i].Split(',')[0].Split('=')[1]
    }

    Return $grouplist
}

<#
.SYNOPSIS
Returns all the groups containing the user
.DESCRIPTION
Gets all the groups which contain the user,
including the groups containing groups containing the user.
.PARAMETER <UserName>
The name of the user
.OUTPUTS
An array of all the groups containing the user
#>
Function Get-UserGroups {
    Param(
        # Name of the user to get
        [Parameter(Mandatory=$true)]
        [String]
        $UserName
    )

    $groups = (Get-ADPrincipalGroupMembership -Identity $UserName | Foreach-Object{$_.name})
    $userGroups = $groups | Select-Object -Unique

    For ($i = 0; $i -lt $userGroups.Count; $i++) {
        $group = $userGroups[$i]
        $userGroups += Get-GroupParents -GroupName $group
        $userGroups = $userGroups | Select-Object -Unique
    }

    Return $userGroups
}