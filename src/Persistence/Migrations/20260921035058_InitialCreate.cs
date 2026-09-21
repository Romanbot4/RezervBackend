using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "businesses",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        Name = table
                            .Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsActive = table.Column<bool>(
                            type: "tinyint(1)",
                            nullable: false,
                            defaultValue: true
                        ),
                        AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_businesses", x => x.Id);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "customers",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        Name = table
                            .Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Email = table
                            .Column<string>(type: "varchar(320)", maxLength: 320, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        PasswordHash = table
                            .Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_customers", x => x.Id);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "packages",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        BusinessId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        Name = table
                            .Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Credits = table.Column<int>(type: "int", nullable: false),
                        ValidityDays = table.Column<int>(type: "int", nullable: false),
                        Price = table.Column<decimal>(
                            type: "decimal(10,2)",
                            precision: 10,
                            scale: 2,
                            nullable: false
                        ),
                        IsActive = table.Column<bool>(
                            type: "tinyint(1)",
                            nullable: false,
                            defaultValue: true
                        ),
                        AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_packages", x => x.Id);
                        table.ForeignKey(
                            name: "FK_packages_businesses_BusinessId",
                            column: x => x.BusinessId,
                            principalTable: "businesses",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "timetable_schedules",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        BusinessId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        ClassName = table
                            .Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        InstructorName = table
                            .Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        AvailableSlots = table.Column<int>(type: "int", nullable: false),
                        BookedCount = table.Column<int>(
                            type: "int",
                            nullable: false,
                            defaultValue: 0
                        ),
                        AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_timetable_schedules", x => x.Id);
                        table.ForeignKey(
                            name: "FK_timetable_schedules_businesses_BusinessId",
                            column: x => x.BusinessId,
                            principalTable: "businesses",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "customer_packages",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        CustomerId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        PackageId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        BusinessId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        TotalCredits = table.Column<int>(type: "int", nullable: false),
                        RemainingCredits = table.Column<int>(type: "int", nullable: false),
                        ReservedCredits = table.Column<int>(
                            type: "int",
                            nullable: false,
                            defaultValue: 0
                        ),
                        PurchasedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_customer_packages", x => x.Id);
                        table.ForeignKey(
                            name: "FK_customer_packages_businesses_BusinessId",
                            column: x => x.BusinessId,
                            principalTable: "businesses",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                        table.ForeignKey(
                            name: "FK_customer_packages_customers_CustomerId",
                            column: x => x.CustomerId,
                            principalTable: "customers",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                        table.ForeignKey(
                            name: "FK_customer_packages_packages_PackageId",
                            column: x => x.PackageId,
                            principalTable: "packages",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "bookings",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        CustomerId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        TimetableScheduleId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        CustomerPackageId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        BookedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        Status = table.Column<int>(type: "int", nullable: false),
                        CancelledAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                        RefundApplied = table.Column<bool>(
                            type: "tinyint(1)",
                            nullable: false,
                            defaultValue: false
                        ),
                        AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_bookings", x => x.Id);
                        table.ForeignKey(
                            name: "FK_bookings_customer_packages_CustomerPackageId",
                            column: x => x.CustomerPackageId,
                            principalTable: "customer_packages",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                        table.ForeignKey(
                            name: "FK_bookings_customers_CustomerId",
                            column: x => x.CustomerId,
                            principalTable: "customers",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                        table.ForeignKey(
                            name: "FK_bookings_timetable_schedules_TimetableScheduleId",
                            column: x => x.TimetableScheduleId,
                            principalTable: "timetable_schedules",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "credit_transactions",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        CustomerPackageId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        Amount = table.Column<int>(type: "int", nullable: false),
                        Type = table.Column<int>(type: "int", nullable: false),
                        Reason = table
                            .Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        OccurredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        BookingId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: true,
                            collation: "ascii_general_ci"
                        ),
                        WaitlistEntryId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: true,
                            collation: "ascii_general_ci"
                        ),
                        AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_credit_transactions", x => x.Id);
                        table.ForeignKey(
                            name: "FK_credit_transactions_customer_packages_CustomerPackageId",
                            column: x => x.CustomerPackageId,
                            principalTable: "customer_packages",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "waitlist_entries",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        TimetableScheduleId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        CustomerId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        CustomerPackageId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: false,
                            collation: "ascii_general_ci"
                        ),
                        JoinedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        Status = table.Column<int>(type: "int", nullable: false),
                        PromotedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                        BookingId = table.Column<Guid>(
                            type: "char(36)",
                            nullable: true,
                            collation: "ascii_general_ci"
                        ),
                        AddedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_waitlist_entries", x => x.Id);
                        table.ForeignKey(
                            name: "FK_waitlist_entries_customer_packages_CustomerPackageId",
                            column: x => x.CustomerPackageId,
                            principalTable: "customer_packages",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                        table.ForeignKey(
                            name: "FK_waitlist_entries_customers_CustomerId",
                            column: x => x.CustomerId,
                            principalTable: "customers",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                        table.ForeignKey(
                            name: "FK_waitlist_entries_timetable_schedules_TimetableScheduleId",
                            column: x => x.TimetableScheduleId,
                            principalTable: "timetable_schedules",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "businesses",
                columns: new[] { "Id", "AddedAt", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    {
                        new Guid("0d0238a1-a72d-7624-1a8f-99d6aee4c02a"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        true,
                        "Rhino Yoga Studio",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        true,
                        "Rezerv Fitness",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "Id", "AddedAt", "Email", "Name", "PasswordHash", "UpdatedAt" },
                values: new object[,]
                {
                    {
                        new Guid("00484cfb-27be-2548-1aa6-a349c22204ac"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "thor.odinson@notgmail.com",
                        "Thor Odinson",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("0ad7fa28-5f1f-6c53-2c96-8b774601dc73"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "zheng.yu@notgmail.com",
                        "Zheng Yu",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("1e151057-ccf6-5a19-2d99-5b5041ac81f2"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "kim.jong.un@notgmail.com",
                        "Kim Jong Un",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("2aedd076-ca5a-2959-f239-bf24255dd176"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "tony.stark@notgmail.com",
                        "Tony Stark",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("2ca94917-7c1c-c44c-f798-5bca6920529a"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "yan.naing.kyaw@notgmail.com",
                        "Yan Naing Kyaw",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("417f56ea-60be-a7d9-d576-69740730a6b4"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "steve.roger@notgmail.com",
                        "Steve Roger",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("737feb5f-959d-7fc4-7be5-ad8d1aa70d38"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "zaw.myo.tun@notgmail.com",
                        "Zaw Myo Tun",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("936557d8-b4ab-3fc4-54c8-bec7a61391de"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "kyaw.pyae.phyo@notgmail.com",
                        "Kyaw Pyae Phyo",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("a9103b4a-e548-6bcd-b239-f7c71c179881"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "donald.trump@notgmail.com",
                        "Donald Trump",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("e1f5adc4-4394-a3ae-edfa-f9ed517cb2cb"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "bruce.will@notgmail.com",
                        "Bruce Will",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("fe6bc868-82b7-ffaf-306a-606b1c751280"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "eaint.pan@notgmail.com",
                        "Eaint Pan",
                        "LBA/LE7R5ZwLTi4Bghdw+g==",
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "packages",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "BusinessId",
                    "Credits",
                    "IsActive",
                    "Name",
                    "Price",
                    "UpdatedAt",
                    "ValidityDays",
                },
                values: new object[,]
                {
                    {
                        new Guid("20f9dcfc-422b-d1ea-1af7-d5f51b1a72d7"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("0d0238a1-a72d-7624-1a8f-99d6aee4c02a"),
                        20,
                        true,
                        "Yoga Pro",
                        149.00m,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        90,
                    },
                    {
                        new Guid("57a730c5-f254-c240-6cb5-0fb6abe09aff"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("0d0238a1-a72d-7624-1a8f-99d6aee4c02a"),
                        10,
                        true,
                        "Yoga Standard",
                        85.00m,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        60,
                    },
                    {
                        new Guid("7fb4b589-1518-7793-5ff2-fb89e1c6e852"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("0d0238a1-a72d-7624-1a8f-99d6aee4c02a"),
                        5,
                        true,
                        "Yoga Starter",
                        45.00m,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        30,
                    },
                    {
                        new Guid("875d6f79-65ce-4ccf-0dbf-12605df3e09d"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        10,
                        true,
                        "Fitness Standard",
                        89.00m,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        60,
                    },
                    {
                        new Guid("a26dfc2b-0795-f479-c6f1-e939f171b16b"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        5,
                        true,
                        "Fitness Starter",
                        49.00m,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        30,
                    },
                    {
                        new Guid("c02d394a-97bf-c060-8bf8-64fd888cc57d"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        20,
                        true,
                        "Fitness Pro",
                        159.00m,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        90,
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "timetable_schedules",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "AvailableSlots",
                    "BookedCount",
                    "BusinessId",
                    "ClassName",
                    "EndTime",
                    "InstructorName",
                    "StartTime",
                    "UpdatedAt",
                },
                values: new object[,]
                {
                    {
                        new Guid("03400720-bc13-511c-930e-0605aafa1bcf"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        8,
                        1,
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        "Early Bird Cardio",
                        new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Tom Holland",
                        new DateTime(2026, 9, 23, 23, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("29430c46-33b6-e642-5431-7df2fc70a73e"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        20,
                        3,
                        new Guid("0d0238a1-a72d-7624-1a8f-99d6aee4c02a"),
                        "Zendaya Yoga",
                        new DateTime(2026, 9, 22, 1, 15, 0, 0, DateTimeKind.Utc),
                        "Zendaya",
                        new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "timetable_schedules",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "AvailableSlots",
                    "BusinessId",
                    "ClassName",
                    "EndTime",
                    "InstructorName",
                    "StartTime",
                    "UpdatedAt",
                },
                values: new object[]
                {
                    new Guid("71cf8a55-66de-661c-e284-7b0100fed16c"),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    10,
                    new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                    "Sunrise Bootcamp",
                    new DateTime(2026, 9, 21, 1, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    "Chris Hemsworth",
                    new DateTime(2026, 9, 21, 0, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                }
            );

            migrationBuilder.InsertData(
                table: "timetable_schedules",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "AvailableSlots",
                    "BookedCount",
                    "BusinessId",
                    "ClassName",
                    "EndTime",
                    "InstructorName",
                    "StartTime",
                    "UpdatedAt",
                },
                values: new object[]
                {
                    new Guid("881ee867-8186-f02e-da88-63950875320e"),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    15,
                    3,
                    new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                    "Morning Upper Body Workout",
                    new DateTime(2026, 9, 22, 2, 0, 0, 0, DateTimeKind.Utc),
                    "Brad Pitt",
                    new DateTime(2026, 9, 22, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                }
            );

            migrationBuilder.InsertData(
                table: "timetable_schedules",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "AvailableSlots",
                    "BusinessId",
                    "ClassName",
                    "EndTime",
                    "InstructorName",
                    "StartTime",
                    "UpdatedAt",
                },
                values: new object[]
                {
                    new Guid("9b19dfb0-d397-d788-6589-e26716010aee"),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    10,
                    new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                    "Lunchtime Upper Body Workout",
                    new DateTime(2026, 9, 22, 3, 0, 0, 0, DateTimeKind.Utc),
                    "Taylor Swift",
                    new DateTime(2026, 9, 22, 2, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                }
            );

            migrationBuilder.InsertData(
                table: "timetable_schedules",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "AvailableSlots",
                    "BookedCount",
                    "BusinessId",
                    "ClassName",
                    "EndTime",
                    "InstructorName",
                    "StartTime",
                    "UpdatedAt",
                },
                values: new object[,]
                {
                    {
                        new Guid("c35a8c92-23bb-7c32-4e38-b8f537a40571"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        5,
                        5,
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        "Strength Foundations",
                        new DateTime(2026, 9, 23, 11, 0, 0, 0, DateTimeKind.Utc),
                        "Bradd Pitt",
                        new DateTime(2026, 9, 23, 10, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("c52fa314-f98e-013b-b5d1-f85c7f303eb4"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        10,
                        1,
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        "Lunchtime Pilates",
                        new DateTime(2026, 9, 21, 6, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        "Sofia Rossi",
                        new DateTime(2026, 9, 21, 5, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "timetable_schedules",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "AvailableSlots",
                    "BusinessId",
                    "ClassName",
                    "EndTime",
                    "InstructorName",
                    "StartTime",
                    "UpdatedAt",
                },
                values: new object[]
                {
                    new Guid("d7a37bf5-57c5-50f3-380e-c504e9bf207d"),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    12,
                    new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                    "Morning Cardio Class",
                    new DateTime(2026, 9, 22, 2, 30, 0, 0, DateTimeKind.Utc),
                    "Justin Bieber",
                    new DateTime(2026, 9, 22, 1, 30, 0, 0, DateTimeKind.Utc),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                }
            );

            migrationBuilder.InsertData(
                table: "customer_packages",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "BusinessId",
                    "CustomerId",
                    "ExpiresAt",
                    "PackageId",
                    "PurchasedAt",
                    "RemainingCredits",
                    "TotalCredits",
                    "UpdatedAt",
                },
                values: new object[,]
                {
                    {
                        new Guid("23ebed7a-af0f-0861-235e-31cedec9ef5a"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("0d0238a1-a72d-7624-1a8f-99d6aee4c02a"),
                        new Guid("00484cfb-27be-2548-1aa6-a349c22204ac"),
                        new DateTime(2026, 11, 17, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("57a730c5-f254-c240-6cb5-0fb6abe09aff"),
                        new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        9,
                        10,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("2fb0d3c6-1438-3255-f7ef-87871541eb5b"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        new Guid("e1f5adc4-4394-a3ae-edfa-f9ed517cb2cb"),
                        new DateTime(2026, 10, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("a26dfc2b-0795-f479-c6f1-e939f171b16b"),
                        new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        4,
                        5,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("4a64c343-d30f-8448-ef96-f73ff11df675"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        new Guid("0ad7fa28-5f1f-6c53-2c96-8b774601dc73"),
                        new DateTime(2026, 11, 17, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("875d6f79-65ce-4ccf-0dbf-12605df3e09d"),
                        new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        8,
                        10,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("5fc76cf4-e225-43a7-141a-5e59cbb18f9e"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        new Guid("737feb5f-959d-7fc4-7be5-ad8d1aa70d38"),
                        new DateTime(2026, 11, 17, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("875d6f79-65ce-4ccf-0dbf-12605df3e09d"),
                        new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        8,
                        10,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("66918386-aea8-7f33-3718-e71fc1f12f15"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        new Guid("2ca94917-7c1c-c44c-f798-5bca6920529a"),
                        new DateTime(2026, 11, 17, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("875d6f79-65ce-4ccf-0dbf-12605df3e09d"),
                        new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        8,
                        10,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("85476e89-a7ff-0fde-07a2-033e3752c313"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        new Guid("936557d8-b4ab-3fc4-54c8-bec7a61391de"),
                        new DateTime(2026, 11, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("875d6f79-65ce-4ccf-0dbf-12605df3e09d"),
                        new DateTime(2026, 9, 19, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        8,
                        10,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("92c7d3cb-cd53-775b-e9e9-3b2b1c41cc6c"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("0d0238a1-a72d-7624-1a8f-99d6aee4c02a"),
                        new Guid("2aedd076-ca5a-2959-f239-bf24255dd176"),
                        new DateTime(2026, 11, 17, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("57a730c5-f254-c240-6cb5-0fb6abe09aff"),
                        new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        9,
                        10,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("95648397-c42c-7474-58f8-201d841312ee"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        new Guid("936557d8-b4ab-3fc4-54c8-bec7a61391de"),
                        new DateTime(2026, 9, 16, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("a26dfc2b-0795-f479-c6f1-e939f171b16b"),
                        new DateTime(2026, 8, 17, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        5,
                        5,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("b856b0b8-3ec5-cf36-79a9-fb7b3b265e9d"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        new Guid("936557d8-b4ab-3fc4-54c8-bec7a61391de"),
                        new DateTime(2026, 10, 11, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("a26dfc2b-0795-f479-c6f1-e939f171b16b"),
                        new DateTime(2026, 9, 11, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        0,
                        5,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "customer_packages",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "BusinessId",
                    "CustomerId",
                    "ExpiresAt",
                    "PackageId",
                    "PurchasedAt",
                    "RemainingCredits",
                    "ReservedCredits",
                    "TotalCredits",
                    "UpdatedAt",
                },
                values: new object[]
                {
                    new Guid("b87db777-4820-3355-9948-041a8afe5d4b"),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                    new Guid("1e151057-ccf6-5a19-2d99-5b5041ac81f2"),
                    new DateTime(2026, 11, 17, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    new Guid("875d6f79-65ce-4ccf-0dbf-12605df3e09d"),
                    new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    10,
                    1,
                    10,
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                }
            );

            migrationBuilder.InsertData(
                table: "customer_packages",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "BusinessId",
                    "CustomerId",
                    "ExpiresAt",
                    "PackageId",
                    "PurchasedAt",
                    "RemainingCredits",
                    "TotalCredits",
                    "UpdatedAt",
                },
                values: new object[,]
                {
                    {
                        new Guid("c4ead12f-8460-c8cf-cd9f-4fed44975dc1"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("0d0238a1-a72d-7624-1a8f-99d6aee4c02a"),
                        new Guid("417f56ea-60be-a7d9-d576-69740730a6b4"),
                        new DateTime(2026, 11, 17, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("57a730c5-f254-c240-6cb5-0fb6abe09aff"),
                        new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        9,
                        10,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("e9b9025a-4fdb-2cb4-adee-804887d3e3be"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                        new Guid("fe6bc868-82b7-ffaf-306a-606b1c751280"),
                        new DateTime(2026, 10, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new Guid("a26dfc2b-0795-f479-c6f1-e939f171b16b"),
                        new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        4,
                        5,
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "customer_packages",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "BusinessId",
                    "CustomerId",
                    "ExpiresAt",
                    "PackageId",
                    "PurchasedAt",
                    "RemainingCredits",
                    "ReservedCredits",
                    "TotalCredits",
                    "UpdatedAt",
                },
                values: new object[]
                {
                    new Guid("ea178241-7dc7-ee60-12c3-4693dd2b3dfb"),
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    new Guid("cf24fc77-facf-8d6e-9654-5b07b6cc8ad6"),
                    new Guid("a9103b4a-e548-6bcd-b239-f7c71c179881"),
                    new DateTime(2026, 11, 17, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    new Guid("875d6f79-65ce-4ccf-0dbf-12605df3e09d"),
                    new DateTime(2026, 9, 18, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    10,
                    1,
                    10,
                    new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                }
            );

            migrationBuilder.InsertData(
                table: "bookings",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "BookedAt",
                    "CancelledAt",
                    "CustomerId",
                    "CustomerPackageId",
                    "Status",
                    "TimetableScheduleId",
                    "UpdatedAt",
                },
                values: new object[,]
                {
                    {
                        new Guid("01c2a39c-3b93-108b-b9db-aaf8848cdb53"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("0ad7fa28-5f1f-6c53-2c96-8b774601dc73"),
                        new Guid("4a64c343-d30f-8448-ef96-f73ff11df675"),
                        1,
                        new Guid("881ee867-8186-f02e-da88-63950875320e"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("1b5a3004-97ed-8de4-c6c5-560b4f0ac7a6"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("0ad7fa28-5f1f-6c53-2c96-8b774601dc73"),
                        new Guid("4a64c343-d30f-8448-ef96-f73ff11df675"),
                        1,
                        new Guid("c35a8c92-23bb-7c32-4e38-b8f537a40571"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("262061de-17d5-d76c-c698-c51447e1f7ee"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("e1f5adc4-4394-a3ae-edfa-f9ed517cb2cb"),
                        new Guid("2fb0d3c6-1438-3255-f7ef-87871541eb5b"),
                        1,
                        new Guid("c35a8c92-23bb-7c32-4e38-b8f537a40571"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("338f054c-1ae9-5f44-c291-130ac15caa37"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("417f56ea-60be-a7d9-d576-69740730a6b4"),
                        new Guid("c4ead12f-8460-c8cf-cd9f-4fed44975dc1"),
                        1,
                        new Guid("29430c46-33b6-e642-5431-7df2fc70a73e"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("3557bc85-adf6-4773-7520-438fb5cb9b0d"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("737feb5f-959d-7fc4-7be5-ad8d1aa70d38"),
                        new Guid("5fc76cf4-e225-43a7-141a-5e59cbb18f9e"),
                        1,
                        new Guid("c35a8c92-23bb-7c32-4e38-b8f537a40571"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("4040b00e-0f05-8d60-01b6-9a18ad56689b"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("00484cfb-27be-2548-1aa6-a349c22204ac"),
                        new Guid("23ebed7a-af0f-0861-235e-31cedec9ef5a"),
                        1,
                        new Guid("29430c46-33b6-e642-5431-7df2fc70a73e"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("9dc4baac-f241-50fd-b33b-126523827ee4"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("936557d8-b4ab-3fc4-54c8-bec7a61391de"),
                        new Guid("85476e89-a7ff-0fde-07a2-033e3752c313"),
                        1,
                        new Guid("c52fa314-f98e-013b-b5d1-f85c7f303eb4"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("bf528796-efdd-250a-a6ce-9e810197597c"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("2ca94917-7c1c-c44c-f798-5bca6920529a"),
                        new Guid("66918386-aea8-7f33-3718-e71fc1f12f15"),
                        1,
                        new Guid("c35a8c92-23bb-7c32-4e38-b8f537a40571"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("c3f94e50-59f2-471b-49ca-a6ba2dff7aab"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("936557d8-b4ab-3fc4-54c8-bec7a61391de"),
                        new Guid("85476e89-a7ff-0fde-07a2-033e3752c313"),
                        1,
                        new Guid("03400720-bc13-511c-930e-0605aafa1bcf"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("c51a8972-3fa4-0c37-70a8-7f06b6187677"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("2ca94917-7c1c-c44c-f798-5bca6920529a"),
                        new Guid("66918386-aea8-7f33-3718-e71fc1f12f15"),
                        1,
                        new Guid("881ee867-8186-f02e-da88-63950875320e"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("ce3e4a7c-d048-c0fd-bd82-40bb08584353"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("fe6bc868-82b7-ffaf-306a-606b1c751280"),
                        new Guid("e9b9025a-4fdb-2cb4-adee-804887d3e3be"),
                        1,
                        new Guid("c35a8c92-23bb-7c32-4e38-b8f537a40571"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("e51e626a-b093-7038-a117-abe40eaa6c21"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("737feb5f-959d-7fc4-7be5-ad8d1aa70d38"),
                        new Guid("5fc76cf4-e225-43a7-141a-5e59cbb18f9e"),
                        1,
                        new Guid("881ee867-8186-f02e-da88-63950875320e"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("fdf3c499-b6b3-e22c-9b98-c789d0ece471"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        new DateTime(2026, 9, 20, 21, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("2aedd076-ca5a-2959-f239-bf24255dd176"),
                        new Guid("92c7d3cb-cd53-775b-e9e9-3b2b1c41cc6c"),
                        1,
                        new Guid("29430c46-33b6-e642-5431-7df2fc70a73e"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "waitlist_entries",
                columns: new[]
                {
                    "Id",
                    "AddedAt",
                    "BookingId",
                    "CustomerId",
                    "CustomerPackageId",
                    "JoinedAt",
                    "PromotedAt",
                    "Status",
                    "TimetableScheduleId",
                    "UpdatedAt",
                },
                values: new object[,]
                {
                    {
                        new Guid("2d43f82e-e338-ec7f-3209-461ce3ebf3d0"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("a9103b4a-e548-6bcd-b239-f7c71c179881"),
                        new Guid("ea178241-7dc7-ee60-12c3-4693dd2b3dfb"),
                        new DateTime(2026, 9, 21, 2, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        1,
                        new Guid("c35a8c92-23bb-7c32-4e38-b8f537a40571"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                    {
                        new Guid("5bbbda12-434c-cf03-964b-95a1e4108297"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        new Guid("1e151057-ccf6-5a19-2d99-5b5041ac81f2"),
                        new Guid("b87db777-4820-3355-9948-041a8afe5d4b"),
                        new DateTime(2026, 9, 21, 1, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                        null,
                        1,
                        new Guid("c35a8c92-23bb-7c32-4e38-b8f537a40571"),
                        new DateTime(2026, 9, 21, 3, 50, 57, 573, DateTimeKind.Utc).AddTicks(8990),
                    },
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_bookings_CustomerId",
                table: "bookings",
                column: "CustomerId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_bookings_CustomerPackageId",
                table: "bookings",
                column: "CustomerPackageId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_bookings_TimetableScheduleId",
                table: "bookings",
                column: "TimetableScheduleId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_credit_transactions_CustomerPackageId",
                table: "credit_transactions",
                column: "CustomerPackageId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_customer_packages_BusinessId",
                table: "customer_packages",
                column: "BusinessId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_customer_packages_CustomerId",
                table: "customer_packages",
                column: "CustomerId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_customer_packages_PackageId",
                table: "customer_packages",
                column: "PackageId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_customers_Email",
                table: "customers",
                column: "Email",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_packages_BusinessId",
                table: "packages",
                column: "BusinessId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_timetable_schedules_BusinessId_StartTime",
                table: "timetable_schedules",
                columns: new[] { "BusinessId", "StartTime" }
            );

            migrationBuilder.CreateIndex(
                name: "IX_timetable_schedules_EndTime",
                table: "timetable_schedules",
                column: "EndTime"
            );

            migrationBuilder.CreateIndex(
                name: "IX_waitlist_entries_CustomerId",
                table: "waitlist_entries",
                column: "CustomerId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_waitlist_entries_CustomerPackageId",
                table: "waitlist_entries",
                column: "CustomerPackageId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_waitlist_entries_TimetableScheduleId",
                table: "waitlist_entries",
                column: "TimetableScheduleId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "bookings");

            migrationBuilder.DropTable(name: "credit_transactions");

            migrationBuilder.DropTable(name: "waitlist_entries");

            migrationBuilder.DropTable(name: "customer_packages");

            migrationBuilder.DropTable(name: "timetable_schedules");

            migrationBuilder.DropTable(name: "customers");

            migrationBuilder.DropTable(name: "packages");

            migrationBuilder.DropTable(name: "businesses");
        }
    }
}
