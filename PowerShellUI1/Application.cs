namespace PowerShellUI1
{
    class Application
    {
        public string Abreviation { get; set; }
        public string Name { get; set; }

        public Application(string abreviation, string name)
        {
            Abreviation = abreviation;
            Name = name;
        }
    }
}
