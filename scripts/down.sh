#!/usr/bin/env bash
set -euo pipefail

source "$(dirname "$0")/common.sh"

ensure_repo_root
compose_down_compatible
