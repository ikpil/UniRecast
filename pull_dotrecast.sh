#!/bin/bash

# DotRecast 소스 경로를 입력받습니다.
DOTRECAST_SRC="$1"

# 입력 확인
if [ -z "$DOTRECAST_SRC" ]; then
    echo "Usage: $0 <DotRecast-Source-Path>"
    echo "Example: $0 ../DotRecast"
    exit 1
fi

# 소스 디렉토리 존재 확인
if [ ! -d "$DOTRECAST_SRC" ]; then
    echo "Error: Directory '$DOTRECAST_SRC' not found."
    exit 1
fi

# 복사 대상 모듈 목록
MODULES=(
    "DotRecast.Core"
    "DotRecast.Detour"
    "DotRecast.Detour.Crowd"
    "DotRecast.Detour.Dynamic"
    "DotRecast.Detour.Extras"
    "DotRecast.Detour.TileCache"
    "DotRecast.Recast"
    "DotRecast.Recast.Toolset"
)

DEST_ROOT="./Runtime"

echo "Starting sync from '$DOTRECAST_SRC' to '$DEST_ROOT'..."

for MODULE in "${MODULES[@]}"; do
    # 소스 경로 찾기 (src/ 아래에 있는지 루트에 있는지 확인)
    if [ -d "$DOTRECAST_SRC/src/$MODULE" ]; then
        SRC_PATH="$DOTRECAST_SRC/src/$MODULE"
    elif [ -d "$DOTRECAST_SRC/$MODULE" ]; then
        SRC_PATH="$DOTRECAST_SRC/$MODULE"
    else
        echo "Warning: Module '$MODULE' not found in source path. Skipping."
        continue
    fi

    DEST_PATH="$DEST_ROOT/$MODULE"

    echo "Syncing $MODULE..."
    
    # rsync를 사용하여 동기화
    # -a: 아카이브 모드 (재귀, 권한 유지 등)
    # -v: 상세 출력
    # --delete: 소스에 없는 파일은 대상에서 삭제 (폴더 구조 일치)
    # --exclude: .meta 파일, bin, obj, 숨김 파일 등은 제외
    rsync -av --delete \
        --exclude='*.meta' \
        --exclude='*.asmdef' \
        --exclude='*.csproj' \
        --exclude='bin/' \
        --exclude='obj/' \
        --exclude='.*' \
        "$SRC_PATH/" "$DEST_PATH/"
done

echo "Sync completed."
