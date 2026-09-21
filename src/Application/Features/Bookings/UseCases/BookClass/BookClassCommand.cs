using Application.Abstractions.Messaging;
using Contract.Bookings;

namespace Application.Features.Bookings.UseCases.BookClass;

public record BookClassCommand(Guid ScheduleId, Guid CustomerPackageId, bool JoinWaitlistIfFull)
    : ICommand<BookClassResponse>;
