#!/usr/bin/env bash
#
# Waitlist rule 4. Joins a full class, pushes that class into the past, runs the Hangfire sweep and
# checks the credit reservation was released exactly once.
#
# Usage: ./scripts/waitlist-expiry-test.sh [base-url] [mysql-container]

set -uo pipefail

BASE_URL="${1:-http://localhost:3000}"
MYSQL="${2:-rezerv-mysql}"
EMAIL="kyaw.pyae.phyo@notgmail.com"
PASSWORD="Password123!"

sql() { docker exec "$MYSQL" mysql -uroot -proot -N -B rezerv -e "$1" 2>/dev/null; }

TOKEN=$(curl -fsS -X POST "$BASE_URL/api/auth/login" \
  -H 'Content-Type: application/json' \
  -d "{\"email\":\"$EMAIL\",\"password\":\"$PASSWORD\"}" | jq -r .token.accessToken)

SCHEDULE=$(curl -fsS "$BASE_URL/api/timetable" | jq -r 'map(select(.isFull)) | .[0]')
SCHEDULE_ID=$(echo "$SCHEDULE" | jq -r .scheduleId)
CLASS_NAME=$(echo "$SCHEDULE" | jq -r .className)
BUSINESS_ID=$(echo "$SCHEDULE" | jq -r .businessId)

PACKAGE_ID=$(curl -fsS "$BASE_URL/api/packages/mine" -H "Authorization: Bearer $TOKEN" \
  | jq -r --arg b "$BUSINESS_ID" 'map(select(.businessId == $b and .remainingCredits > .reservedCredits)) | .[0].id')

echo "==> $CLASS_NAME is full, joining the waitlist with package $PACKAGE_ID"

curl -fsS -X POST "$BASE_URL/api/waitlist" \
  -H 'Content-Type: application/json' -H "Authorization: Bearer $TOKEN" \
  -d "{\"scheduleId\":\"$SCHEDULE_ID\",\"customerPackageId\":\"$PACKAGE_ID\"}" > /dev/null

BEFORE=$(sql "SELECT CONCAT(RemainingCredits,'/',ReservedCredits) FROM customer_packages WHERE Id='$PACKAGE_ID';")
LEDGER_BEFORE=$(sql "SELECT COUNT(*) FROM credit_transactions WHERE CustomerPackageId='$PACKAGE_ID' AND Type=5;")
echo "    remaining/reserved = $BEFORE, release rows = $LEDGER_BEFORE"

echo "==> pushing $CLASS_NAME into the past"
sql "UPDATE timetable_schedules SET StartTime = UTC_TIMESTAMP() - INTERVAL 2 HOUR, EndTime = UTC_TIMESTAMP() - INTERVAL 1 HOUR WHERE Id='$SCHEDULE_ID';"

echo "==> triggering the sweep"
curl -fsS -X POST "$BASE_URL/hangfire/recurring/trigger" \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  --data-urlencode "jobs[]=waitlist-release-ended" > /dev/null

for _ in $(seq 1 30); do
  STATUS=$(sql "SELECT Status FROM waitlist_entries WHERE TimetableScheduleId='$SCHEDULE_ID' AND CustomerPackageId='$PACKAGE_ID';")
  [ "$STATUS" = "4" ] && break
  sleep 1
done

AFTER=$(sql "SELECT CONCAT(RemainingCredits,'/',ReservedCredits) FROM customer_packages WHERE Id='$PACKAGE_ID';")
LEDGER_AFTER=$(sql "SELECT COUNT(*) FROM credit_transactions WHERE CustomerPackageId='$PACKAGE_ID' AND Type=5;")
echo "    waitlist status = $STATUS (4 = Expired)"
echo "    remaining/reserved = $AFTER, release rows = $LEDGER_AFTER"

echo "==> triggering it a second time, nothing should move"
curl -fsS -X POST "$BASE_URL/hangfire/recurring/trigger" \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  --data-urlencode "jobs[]=waitlist-release-ended" > /dev/null
sleep 5

AGAIN=$(sql "SELECT CONCAT(RemainingCredits,'/',ReservedCredits) FROM customer_packages WHERE Id='$PACKAGE_ID';")
LEDGER_AGAIN=$(sql "SELECT COUNT(*) FROM credit_transactions WHERE CustomerPackageId='$PACKAGE_ID' AND Type=5;")
echo "    remaining/reserved = $AGAIN, release rows = $LEDGER_AGAIN"

echo
if [ "$STATUS" = "4" ] && [ "$AFTER" = "$AGAIN" ] && [ "$LEDGER_AFTER" = "$LEDGER_AGAIN" ]; then
  echo "PASS  reservation released once, second run changed nothing"
else
  echo "FAIL  status=$STATUS  after=$AFTER again=$AGAIN  ledger=$LEDGER_AFTER/$LEDGER_AGAIN"
  exit 1
fi
