param([string]$productId,
[string]$release,
[string]$build,
[string]$filename)

Invoke-WebRequest -Uri "http://sc-css-wb-02-vm.amat.com/www1/cmweb/services/cmweb.srvc.mconfig-spec.php?optimize=true&ServiceCMD=QuickGetForBuild&productId=$productId&release=$release&build=$build" -UseDefaultCredentials -OutFile $filename