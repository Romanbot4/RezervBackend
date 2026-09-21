using Domain.Entities;
using Persistence.Common.Helpers;

namespace Persistence.Seeders;

public static class CustomerSeeder
{
    public const string DefaultPassword = "Password123!";

    /// output of Md5HashPasswordService("Password123!")
    private const string DefaultPasswordHash = "LBA/LE7R5ZwLTi4Bghdw+g==";

    public const string KyawPyaePhyo = "Kyaw Pyae Phyo";
    public const string ZawMyoTun = "Zaw Myo Tun";
    public const string YanNaingKyaw = "Yan Naing Kyaw";
    public const string ZhengYu = "Zheng Yu";
    public const string EaintPan = "Eaint Pan";
    public const string BruceWill = "Bruce Will";
    public const string KimJongUn = "Kim Jong Un";
    public const string DonaldTrump = "Donald Trump";
    public const string SteveRoger = "Steve Roger";
    public const string TonyStark = "Tony Stark";
    public const string ThorOdinson = "Thor Odinson";

    public static readonly string[] Names =
    [
        KyawPyaePhyo,
        ZawMyoTun,
        YanNaingKyaw,
        ZhengYu,
        EaintPan,
        BruceWill,
        KimJongUn,
        DonaldTrump,
        SteveRoger,
        TonyStark,
        ThorOdinson,
    ];


    public static ICollection<CustomerEntity> GetCustomers()
    {
        return
        [
            .. Names.Select(name => new CustomerEntity(
                IdFor(name),
                name,
                EmailFor(name),
                DefaultPasswordHash
            )
            {
                AddedAt = SeedClock.Now,
                UpdatedAt = SeedClock.Now,
            }),
        ];
    }

    public static string EmailFor(string name) =>
        $"{name.ToLowerInvariant().Replace(' ', '.')}@notgmail.com";

    public static Guid IdFor(string name) => DeterministicGuid.From($"customer_{EmailFor(name)}");
}
