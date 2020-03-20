namespace ScooterBear.GTD.IntegrationTests.UserProject
{
    public class ProjectItem
    {
        public ProjectItem(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}