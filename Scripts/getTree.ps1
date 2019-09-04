#requires -Module ActiveDirectory

<#
.SYNOPSIS
Returns an array containing the parents of the group
.DESCRIPTION
Gets the groups which contain the input group
.NOTES
Requires the ActiveDirectory module
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
.OUTPUTS
An array containing only groups without parents
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

<#
.SYNOPSIS
Returns a tree of self and children
.DESCRIPTION
Returns "[depth] name" for itself and appends its children's
values through this function.
If there is no child, it will just return its own value.
.PARAMETER <GroupName>
The name of the parent group
.PARAMETER <Depth>
The current depth in the tree,
increments by one for children
.OUTPUTS
A string containing the "[depth] name" of itself and its children
#>
Function Show-GroupChildren {
    Param(
        # Name of the group to check
        [Parameter(Mandatory=$true)]
        [String]
        $GroupName,
        # Current depth of checking
        [Parameter(Mandatory=$false)]
        [Int32]
        $Depth = 0,
        # List of all groups already done
        [Parameter(Mandatory=$false)]
        [System.Array]
        $Done = @()
    )

    If($Done.IndexOf($GroupName) -ne -1) {
        $result = "[$Depth...] $GroupName"
        Return $result
    }

    $Done += $GroupName

    Try {
        $children = Get-ADGroupMember -Identity $GroupName
    } Catch {
        Return ""
    }

    $result = "[$Depth] $GroupName"

    If($children -is [System.Array]) {
        Foreach($child in $children) {
            If($child.objectGUID -ne [GUID]::parse('00000000-0000-0000-0000-000000000000')) {
                $ts = Show-GroupChildren -GroupName $child.name -Depth ($Depth+1)
                If($ts.Length -gt 0) {
                    $result += "`r`n$ts"
                }
            }
        }
    }

    Return $result
}

<#
.SYNOPSIS
Returns a string of the tree
.DESCRIPTION
Returns a string representing the active directory's groups tree.
.OUTPUTS
A string representing the tree.
#>
Function Get-AllGroupChildren {
    $result = ""

    Get-TopParents | ForEach-Object {$result += (Show-GroupChildren -GroupName $_) + "`r`n"}

    return $result
}

Get-AllGroupChildren