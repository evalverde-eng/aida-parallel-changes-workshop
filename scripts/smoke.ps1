. "$PSScriptRoot/common.ps1"
Import-AidaEnv
 
Invoke-InAidaRepoRoot {
    Assert-DockerAvailable
    Remove-LegacyContainers
    Remove-PortCollisions

    $requests = @(
        Get-ChildItem -Path 'http' -Filter '*.http' -File
    ) |
    Sort-Object FullName |
    ForEach-Object { $_.FullName.Substring($PWD.Path.Length + 1).Replace('\\', '/') }

    foreach ($request in $requests) {
        Invoke-Compose run --rm ijhttp --env-file $env:AIDA_HTTP_ENV_FILE --env $env:AIDA_HTTP_ENV $request
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }
}
