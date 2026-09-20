using Contract.Bookings;
using Core.Primitives.Result;
using MediatR;

namespace Application.Features.Bookings.UseCases.BookClass;

public record BookClassCommand(Guid ScheduleId, Guid CustomerPackageId, bool JoinWaitlistIfFull)
    : IRequest<Result<BookClassResponse>>;
