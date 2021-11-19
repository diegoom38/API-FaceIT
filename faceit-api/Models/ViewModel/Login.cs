namespace faceitapi.Models.ViewModel
{
    public class LoginEntry
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public int? GoogleId { get; set; }
    }

    public class LoginRetun
    {
        public Pessoa pessoa { get; set; }
        public Token token { get; set; }
    }
}