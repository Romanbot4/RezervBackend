using Application.Abstractions.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

// Please upload the source code to a public GitHub repository and submit the GitHub URL
// to:
// zawmyohtun@, yannaingkyaw@, CC: zhengyu@ and
// eaintpan@

public class CustomerSeeder(IHashPasswordService hashPasswordService)
    : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        string[] names =
        [
            "Kyaw Pyae Phyo",
            "Zaw Myo Tun",
            "Yan Naing Kyaw",
            "Zheng Yu",
            "Eaint Pan",
            "Bruce Will",
            "Kim Jong Un",
            "Donald Trump",
            "Steve Roger",
            "Tony Stark",
            "Thor Odinson",
        ];

        string defaultPass = "Password123!";

        var hash = hashPasswordService.Hash(defaultPass);

        var customers = names.Select(name =>
        {
            var email = $"{name.ToLowerInvariant().Replace(' ', '.')}@notgmail.com";

            return new CustomerEntity(
                DeterministicGuid.From($"customer_{email}"),
                name,
                email,
                hash
            );
        });

        builder.HasData(customers);
    }
}
