using Application.Abstractions.Database;

namespace Application.Abstractions.Repositories;

using Domain.Entities;

public interface IBookingRepository : IGenericRepository<BookingEntity> { }
