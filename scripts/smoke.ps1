. "$PSScriptRoot/common.ps1"
Import-AidaEnv
 
Invoke-InAidaRepoRoot {
    Assert-DockerAvailable
    Remove-LegacyContainers
    Remove-PortCollisions

    Invoke-Compose up -d --build ijhttp
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

    $requests = @(
        Get-ChildItem -Path 'http' -Filter '*.http' -File
    ) |
    Sort-Object FullName |
    ForEach-Object { $_.FullName.Substring($PWD.Path.Length + 1).Replace('\\', '/') }

    foreach ($request in $requests) {
        Invoke-Compose exec -T ijhttp /bin/sh /workspace/scripts/run-ijhttp.sh --env-file $env:AIDA_HTTP_ENV_FILE --env $env:AIDA_HTTP_ENV $request
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }
}
