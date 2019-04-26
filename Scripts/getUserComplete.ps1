Param([string]$samid=$env:username)

$searcher=New-Object DirectoryServices.DirectorySearcher
$searcher.Filter="(&(objectcategory=person)(objectclass=user)(sAMAccountname=$samid))"
$user=$searcher.FindOne()
if ($user -ne $null ){
    return $user.getdirectoryentry()
}

return $user