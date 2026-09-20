# Rezerv Backend

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
  So, I need to count the remainin

  I applied all those rules from across packages and bookings as follows
  - ensure that current logged in consumer own the consumer package
  - ensure that package can only be used for schedules under the same business
  - ensure expired packages cannot be used
  - ensure customer have sufficient credits
  - ensure users cannot book overlapping timetable schedules
  - booking deducts 1 package credit immediately.
