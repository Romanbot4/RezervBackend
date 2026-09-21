# Rezerv Backend

## Project Setup

### 1. Setup MySql

```
docker run -d --name rezerv-mysql -p 3306:3306 -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=rezerv mysql:8.4
```

### 2. Migrate and Seed Data

Change dir to Persistence folder

```
cd ./src/Persistence/
```

Migrate and seed data

```
dotnet ef database update
```

## 1. Clean Architecture Project Setup

My usual Project skeleton to startup the assignment

```
|--WebApi (Program.cs)
|----src/Application
|----src/Contract
|----src/Core
|----src/Domain
|----src/Infrastructure
|----src/Persistence
|    (I prefer to separate Persistence layer from Infrastructure)
|----src/Presentation
```

## 2. Added Entities

I read the business rules from given PDF file and added the entities.
I may need to change or add new properties while I was developing.
I will use Domain driven approach and will translate business logic methods to each entity later if required.

```
--src/Domain/Entities
----BookingEntity.cs
----BusinessEntity.cs
----CustomerEntity.cs
----CustomerPackageEntity.cs
----PackageEntity.cs
----TimeableScheduleEntity.cs
----WaitlistEntryEntity.cs
```

## 3. Added IHashPasswordService and Tests

- based on simple MD5
- used XUnit for testing and will use for other tests too

## 4. Define Entity Relationship

- read and review the business logic requirement and defined entity relationship in Domain Layer
- implemented the entity configurations inside Persistence Layer
- also added the seed to most fundamental entities
- focused on timetable data. will need to use these data to test different edge cases.
- I also added a CreditTransactionEntity which seems like a violation of Atomic design but I just thought there should be something to log the transactions

## 5. UseCases

- I first thought about doing CQRS pattern but due to minimal timeline,
  I choosed to combine Command and Query under UseCases but will code with that pattern in case I still have time before deadline

  ### 1. Booking And Waitlisting

  After reading the documentation, I find these two feature share some business logics.
  So, I decided to share some logic for schedule function.
  And ofc, I clearly need to handle the cancellation logics which are completly different.

  I also noticed from the start that cancelling from waitlist will release credit reservation under waitlist rules.
  So, I need to count the RemainingCredits - ReservedCredits to get the AvailableCredits

  I applied all those rules from across packages and bookings as follows
  - ensure that current logged in consumer own the consumer package
  - ensure that package can only be used for schedules under the same business
  - ensure expired packages cannot be used
  - ensure customer have sufficient credits
  - ensure users cannot book overlapping timetable schedules
  - booking deducts 1 package credit immediately.

  New Enums
  - I will need to define WaitlistStatus to track the state of waitlist.
  - CreditTransactionEnity will also need WaitlistStatus to define its type

## 6. Cancellation and Waitlist Promotion

Cancellation couple together with the waitlist promotion.

### 1. Cancellation rules

- 1 credit is refunded when cancelled more than 4 hours before the class starts
- I put the window on TimetableScheduleEntity as RefundWindow and bacause it also has the start time
- I decided not to refund onto an already expired package

### 2. Promotion

- when a booking is cancelled first person waiting get the slot, FIFO by JoinedAt and Id to break tie
- during promotion from waitlist, at first I only tried to update the first person.
  but then I notice that if first peson's package is exp, then next person get the slot.
  so, I updated WaitlistPromoter to loop the waitlist rather than only looking at the first person.
- promotion goes through BookingService so I am not writing the same business rules twice

### 3. Credit lifecycle

This is the part I thought about the most. The Doc says the credit is deducted on promotion, and
that the reservation is released if the class ends while the user is still waiting. It never
actually says a credit is reserved when joining the waitlist.
So, I decided to reserve on join anyway. So the flow is:

- join waitlist, ReservesCredits, the credit is reserved and not spent
- promoted, ConsumeReserveCredits, the reserved credits was actually deducted
- dropped or class ended, the hold is released and nothing is charged

AvailableCredits = RemainingCredits - ReservedCredits is used to check the actual available credits

### 4. Tests

- ./UnitTests/Entities contains the domain logic tests methods that I could think of
- the credit lifecycle, join then promote costs exactly 1 credit, a single credit cannot be held twice, promotion refuses when nothing was held
- the slot rules, capacity can never be exceeded limit, release cannot go negative, cancel followed by promotion leaves attendance unchanged
- the 4 hour refund boundary, tested on both cases

## 7. Seed Data

I added sample data to test the edge cases from the doc, so I seeded everything with HasData.

- 2 businesses, 11 customers, 6 packages, 13 customer packages, 8 schedules, 13 bookings, 2 waitlist
- Kyaw Pyae Phyo is account I used to test. I own a valid package, an expired package and a used package,

Minor Bug fixes,

- CustomerSeeder was not applied. I accidently added IHashPasswordService which wont work.
  Added a preprocessed MD% Hash instead to fix.
- HasData skips SaveChangesAsync so UpdateTimestamps never runs and all AddedAt became 0001-01-01.
  Defined SeedDateTime and attached the timestamp manually

## 8. Query Methods and Presentation Layer Setup and Connect with Application Layer

### 1. ICommand and IQuery

As I said eailier I usually use CQRS. So, in case I still have time. I would create a readonly DB instance and fully setup CQRS.
But for now I will use only the syntax of CQRS under UseCases.

- extends the commands onto ICommand and ICommandHandler and queries to IQuery and IQueryHandler

### 2. Routes

```
GET  /api/businesses      pdf dont have this route but wantted to see the business ids during testing
GET  /api/packages        ?businessId=
GET  /api/packages/mine   auth
GET  /api/timetable       ?businessId= &date=
GET  /api/bookings/mine   auth
```

## 9. Global Exception Handling

This is the pattern that I always use to to convert core exception thrown from anywhere to fomatted response.

- a CoreException maps through ToFailure then ToFailureResponse. Other exception else becomes an UnknownException
  so internal errors data never shown to users.
- every error has the same body. status, code, message, errors

## 10. Purchase Package

This is a mock package purchase route that I left while doing other routes.

```
POST /api/packages/purchase
```

- mock purchase only. no payment gateway as the doc said
- PurchaseFor Domain method on PackageEntity builds the CustomerPackage.
  credits come from the package and ExpiresAt is purchased time plus ValidityDays
- writes a Purchase credit transaction ledger to show where the credits came from
- an inactive package cannot be bought
- I tested that the credits that I bought are usable by booking a class with the new package after buying it

## 11. Concurrency

Booking count must never exceed the available slots even when many users book at the same time.
My booking code currently check try avoid double booking with race condition.
But on acutal prod server there are multiple process spawning and competing requests for same db row
There is a gap between read and write which can still leave an opening.
I will use the db transaction and commit to handle this. The doc say to use redis for both concurrency
and caching. Redis lock is a good idead but db transaction is also nice to have.

current issues

- The old code read the count, checked it, then wrote it back using unit of work pattern only.

```
if (!schedule.HasAvailableSlot()) // reads
schedule.ReserveSlot();           // make changes
await unitOfWork.SaveChangesAsync() // writes using UoW
```

With 5 slots and BookedCount 4, two requests can both read 4, both see a free slot and both write 5.
Two bookings for one seat. The gap between the read and the write is the problem.

### Conditional update

So I moved the check into the update itself. So, basically read and update at the same time
with no gap between read and write.

```
.Where(cp => cp.Id == id && cp.RemainingCredits - cp.ReservedCredits >= 1)
.ExecuteUpdateAsync(
    setters => setters.SetProperty(cp => cp.RemainingCredits, cp => cp.RemainingCredits - 1),
    cancellationToken
);
```

Changes

- added ITransaction and EfTransaction and BeginTransactionAsync on IUnitOfWork
- BookClass, JoinWaitlist and CancelBooking now wrap their writes in one transaction
- I trimmed one class down to 3 slots and fired 11 customers at it at the same time.

I did the same for every credit movement. TryConsumeCredit, TryReserveCredit,
TryConsumeReservedCredit, RefundCredit and ReleaseReservation.

## 12. Redis Lock

The conditional update already stops the overbooking so this is not what makes it correct.
I added the lock on top as an extra guard.

```
SET lock:schedule:{id} {random token} NX PX 5000
```

- If redis is down, service still work. I added a bypass

```
catch (RedisException exception)
{
    return new UnlockedHandle(key);
}
```

- the lock is taken before BeginTransactionAsync so the lock always covers the transaction
- if the wait times out the request gets 422 ScheduleBusy so the user can retry

## 13. Redis Caching

GET /api/timetable is readonly endpoint every user hits first and it changes rarely, so I will use it to showcase Redis skill

Keys

```
timetable:all:all
timetable:{businessId}:all
timetable:all:{date}
```

- one key per filter combination with a 60 second TTL
- booking and cancelling clear the whole timetable prefix after the commit, not before,
  so the new attendance is already visible when the cache is dropped
- like the concurrency lock if fails open. Every redis error is caught reads the database instead.

Measured on a fresh database

```
1st call  miss  0.0095s   creates timetable:all:all
2nd call  hit   0.0056s   no db query
```
