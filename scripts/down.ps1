$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/common.ps1"
Import-AidaEnv

Invoke-InAidaRepoRoot {
    Invoke-ComposeDownCompatible
}
