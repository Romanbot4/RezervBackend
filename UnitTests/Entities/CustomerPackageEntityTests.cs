using System;
using Core.Exception.NetworkException;
using Domain.Entities;
using Xunit;

namespace UnitTests.Entities;

public class CustomerPackageEntityTests
{
    private static readonly DateTime Now = new(2026, 6, 10, 9, 0, 0, DateTimeKind.Utc);

    private static CustomerPackageEntity Package(int remaining = 10, int reserved = 0) =>
        new(
            id: Guid.NewGuid(),
            customerId: Guid.NewGuid(),
            packageId: Guid.NewGuid(),
            businessId: Guid.NewGuid(),
            totalCredits: 10,
            remainingCredits: remaining,
            reservedCredits: reserved,
            purchasedAt: Now.AddDays(-1),
            expiresAt: Now.AddDays(59)
        );

    [Fact]
    public void ReservesCredits_holds_the_credit_and_makes_it_unavailable()
    {
        var sut = Package(remaining: 10);

        sut.ReservesCredits(1, Now);

        Assert.Equal(1, sut.ReservedCredits);
        // The credit is held, not spent.
        Assert.Equal(10, sut.RemainingCredits);
        // And it can no longer be used for anything else.
        Assert.Equal(9, sut.AvailableCredits);
    }

    [Fact]
    public void ReservesCredits_cannot_hold_more_than_is_available()
    {
        var sut = Package(remaining: 2, reserved: 2);

        Assert.Throws<ForbiddenException>(() => sut.ReservesCredits(1, Now));
    }

    [Fact]
    public void ConsumeReserveCredits_turns_the_hold_into_a_deduction()
    {
        var sut = Package(remaining: 10);
        sut.ReservesCredits(1, Now);

        sut.ConsumeReserveCredits(1, Now);

        // "1 credit should be deducted when promoted to booked": the balance must actually drop.
        Assert.Equal(9, sut.RemainingCredits);
        Assert.Equal(0, sut.ReservedCredits);
        Assert.Equal(9, sut.AvailableCredits);
    }

    [Fact]
    public void ConsumeReserveCredits_refuses_when_nothing_was_held()
    {
        var sut = Package(remaining: 10);

        Assert.Throws<ForbiddenException>(() => sut.ConsumeReserveCredits(1, Now));
    }

    [Fact]
    public void Joining_a_waitlist_then_being_promoted_costs_exactly_one_credit()
    {
        var sut = Package(remaining: 10);

        sut.ReservesCredits(1, Now);
        sut.ConsumeReserveCredits(1, Now);

        Assert.Equal(9, sut.RemainingCredits);
        Assert.Equal(0, sut.ReservedCredits);
    }

    [Fact]
    public void A_single_credit_cannot_be_held_twice()
    {
        var sut = Package(remaining: 1);

        sut.ReservesCredits(1, Now);

        // Without the hold, this second waitlist entry would look affordable and both promotions
        // would later succeed against one credit.
        Assert.Throws<ForbiddenException>(() => sut.ReservesCredits(1, Now));
    }
}
