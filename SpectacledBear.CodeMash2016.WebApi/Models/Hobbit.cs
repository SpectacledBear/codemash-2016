namespace SpectacledBear.CodeMash2016.WebApi.Models
{
    public class Hobbit
    {
        public string Name { get; }
        public string FamilyName { get; }
        public int BirthYear { get; }
        public int DeathYear { get; }
        public long Id { get; }

        public Hobbit (string name, string familyName, int birthYear, int deathYear, long id)
        {
            Name = name;
            FamilyName = familyName;
            BirthYear = birthYear;
            DeathYear = deathYear;
            Id = id;
        }
    }
}
