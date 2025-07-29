using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFTest.MVVM.Model.Exercise;

namespace WPFTest.MVVM.Model.Subject
{
    public class Subject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Year { get; set; }
        public ICollection<LightExercise>? Exercises { get; set; }
    }
}
