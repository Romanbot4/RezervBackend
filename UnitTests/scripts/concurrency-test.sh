#!/usr/bin/env bash
#
# Fires every seeded customer at one under capacity class at the same moment and checks that the
# booking count never goes past the capacity.
#
# Usage: ./scripts/concurrency-test.sh [base-url] [class-name]

set -uo pipefail

BASE_URL="${1:-http://localhost:3000}"
CLASS_NAME="${2:-Lunchtime Upper Body Workout}"
PASSWORD="Password123!"

CUSTOMERS=(
  kyaw.pyae.phyo zaw.myo.tun yan.naing.kyaw zheng.yu eaint.pan bruce.will
  kim.jong.un donald.trump steve.roger tony.stark thor.odinson
)

SCHEDULE=$(curl -fsS "$BASE_URL/api/timetable" | jq -r --arg n "$CLASS_NAME" 'map(select(.className == $n)) | .[0]')
SCHEDULE_ID=$(echo "$SCHEDULE" | jq -r .scheduleId)
BUSINESS_ID=$(echo "$SCHEDULE" | jq -r .businessId)
CAPACITY=$(echo "$SCHEDULE" | jq -r .availableSlots)
BEFORE=$(echo "$SCHEDULE" | jq -r .attendanceCount)

echo "==> $CLASS_NAME  capacity=$CAPACITY  attendance=$BEFORE  contenders=${#CUSTOMERS[@]}"

WORK=$(mktemp -d)
mkdir -p "$WORK/result" "$WORK/body"
trap 'rm -rf "$WORK"' EXIT

for name in "${CUSTOMERS[@]}"; do
(
  # Log in and pick a package BEFORE the barrier, so the contended call is all that is timed.
  token=$(curl -fsS -X POST "$BASE_URL/api/auth/login" \
    -H 'Content-Type: application/json' \
    -d "{\"email\":\"$name@notgmail.com\",\"password\":\"$PASSWORD\"}" | jq -r .token.accessToken)

  package=$(curl -fsS "$BASE_URL/api/packages/mine" -H "Authorization: Bearer $token" \
    | jq -r --arg b "$BUSINESS_ID" \
      'map(select(.businessId == $b and (.remainingCredits - .reservedCredits) > 0)) | .[0].id')

  if [ -z "$package" ] || [ "$package" = "null" ]; then
    printf 'SKIP  %-18s no usable package\n' "$name" > "$WORK/result/$name"
    exit
  fi

  # Every worker spins here, so they all fire within milliseconds of each other.
  while [ ! -f "$WORK/go" ]; do sleep 0.01; done

  status=$(curl -sS -o "$WORK/body/$name" -w '%{http_code}' -X POST "$BASE_URL/api/bookings" \
    -H "Authorization: Bearer $token" \
    -H 'Content-Type: application/json' \
    -d "{\"scheduleId\":\"$SCHEDULE_ID\",\"customerPackageId\":\"$package\",\"joinWaitlistIfFull\":false}")

  if [ "$status" = "201" ]; then
    printf 'OK    %-18s booked\n' "$name" > "$WORK/result/$name"
  else
    printf '%-5s %-18s %s\n' "$status" "$name" \
      "$(jq -r '.code // "?"' < "$WORK/body/$name" 2>/dev/null)" > "$WORK/result/$name"
  fi
) &
done

sleep 4
touch "$WORK/go"
wait

echo
cat "$WORK"/result/* | sort
ACCEPTED=$(cat "$WORK"/result/* | grep -c '^OK' || true)

AFTER=$(curl -fsS "$BASE_URL/api/timetable" \
  | jq -r --arg id "$SCHEDULE_ID" 'map(select(.scheduleId == $id)) | .[0].attendanceCount')

echo
echo "==> accepted=$ACCEPTED  attendance=$BEFORE -> $AFTER  capacity=$CAPACITY"

if [ "$AFTER" -gt "$CAPACITY" ]; then
  echo "FAIL: OVERBOOKED by $((AFTER - CAPACITY))"
  exit 1
fi

echo "PASS: attendance never exceeded capacity"
