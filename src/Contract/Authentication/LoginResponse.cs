using Contract.Customer;

namespace Contract.Authentication;

public record LoginResponse(CustomerResponse Customer, TokenResponse Token);
