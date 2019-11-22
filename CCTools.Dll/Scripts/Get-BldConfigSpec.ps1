param([string]$productId, [string]$release, [string]$build)

Invoke-WebRequest -Uri "http://sc-css-wb-02-vm.amat.com/www1/cmweb/services/cmweb.srvc.mconfig-spec.php?ServiceCMD=QuickGetForBuild&productId=$productId&release=$release&build=$build&ignoreBTV=true&useStorageSrv=true" -UseDefaultCredentials | Write-Host

