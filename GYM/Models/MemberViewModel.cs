namespace GYM.Models

{

    public class MemberViewModel

    {

        public int Id { get; set; }

        public string Name { get; set; }



        public List<int> SelectedLessonIds { get; set; }

        public List<Lesson> AvailableLessons { get; set; }



    }

}
