#!/usr/bin/env sh
set -eu

java --add-opens=java.base/java.util=ALL-UNNAMED ${IJHTTP_JAVA_OPTS:-} -cp "/intellij-http-client/*" "com.intellij.httpClient.cli.HttpClientMain" "$@"
