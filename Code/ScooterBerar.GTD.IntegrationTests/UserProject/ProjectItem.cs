namespace ScooterBear.GTD.IntegrationTests.UserProject
{
    public class ProjectItem
    {
        public string Id { get; }
        public string Name { get; }

        public ProjectItem(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
