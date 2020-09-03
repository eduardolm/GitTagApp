using System.Collections.Generic;

namespace GitTagApp.Entities
{
    public class Models
    {
        public class FakeDb
        {
            public List<Pet> Pets { get; set; } // StarredRepos
            public List<Color> Colors { get; set; }
            public List<PetColor> PetColors { get; set; }
        }
        public class Pet
        {
            public int PetId { get; set; }
            public string Name { get; set; }
        }

        public class Color
        {
            public int ColorId { get; set; }
            public string ColorName { get; set; }
        }

        public class PetColor
        {
            public int PetId { get; set; }
            public int ColorId { get; set; }
        }
    }
}