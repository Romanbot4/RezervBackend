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
