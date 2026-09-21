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

- ./UnitTests/Entities contains the domain logic tests methods that I could think of
- the credit lifecycle, join then promote costs exactly 1 credit, a single credit cannot be held twice, promotion refuses when nothing was held
- the slot rules, capacity can never be exceeded limit, release cannot go negative, cancel followed by promotion leaves attendance unchanged
- the 4 hour refund boundary, tested on both cases
