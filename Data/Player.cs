namespace stortromme.Data
{
    public class Player
    {
        public string Name { get; set; }
        public bool IsConnected { get; set; }
        public bool IsModerator { get; set; }

        public bool IsNameEqual(string name)
        {
            return Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}