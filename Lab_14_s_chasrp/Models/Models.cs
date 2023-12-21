namespace Lab_14_s_chasrp.Models
{
    public class Models
    {

        public List<NationalityModel> Nationalities { get; }
        public List<GroupModel> Groups { get; }

        

        public Models(List<NationalityModel> countryes, List<GroupModel> continents)
        {
            Nationalities = countryes;
            Groups = continents;
        }
    }
}
