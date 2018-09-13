namespace Simulation
{
    public class Register
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class SpinRequest
    {
        public int PlayerId { get; set; }
        public decimal Bet { get; set; }
        public string AuthToken { get; set; }
    }
}
