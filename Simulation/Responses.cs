using Newtonsoft.Json;

namespace Simulation
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class RegistrationResponse : BaseResponse
    {
        public int PlayerId { get; set; }
    }

    public class AuthResponse
    {
        public string token_type { get; } = "Bearer";
        
        public string access_token { get; set; }

        public int expires_in { get; } = 1800;
    }

    public class SpinResponse : BaseResponse
    {
        public string Transaction { get; set; }
        public decimal Balance { get; set; }
    }

    public class WinAmount : BaseResponse
    {
        public decimal Amount { get; set; }
    }

    public class PayLineStat
    {
        public int Id { get; set; }

        public int Stat { get; set; } = 0;
    }

    public class SymbolStat
    {
        public int Id { get; set; }

        public string Symbol { get; set; }

        public int FiveKind { get; set; } = 0;

        public int FourKind { get; set; } = 0;

        public int ThreeKind { get; set; } = 0;
    }

}
