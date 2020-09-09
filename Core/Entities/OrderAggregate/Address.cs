namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }

        public Address() { }
        public Address(string firstName, string lastName, string city, string state, string street, string zipcode)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            State = state;
            Street = street;
            Zipcode = zipcode;
        }
    }
}