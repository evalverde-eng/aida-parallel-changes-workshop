#!/usr/bin/env bash
set -euo pipefail

source "$(dirname "$0")/common.sh"

ensure_repo_root
ensure_docker_available
remove_legacy_containers
remove_port_collisions

shopt -s nullglob
requests=(http/*.http)
shopt -u nullglob

for request in "${requests[@]}"; do
  compose_cmd run --rm ijhttp --env-file "$AIDA_HTTP_ENV_FILE" --env "$AIDA_HTTP_ENV" "$request"
done
