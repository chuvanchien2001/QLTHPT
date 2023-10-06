using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DTO.SubModels
{
    public class StudentNameComparer:IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            List<string> x_names = x.StudentName.Split(" ").Reverse().ToList();
            List<string> y_names = y.StudentName.Split(" ").Reverse().ToList();
            for(int i=0;i<x_names.Count;i++)
            {
                if (x_names[i] != y_names[i])
                {
                    return x_names[i].CompareTo(y_names[i]);
                }
                else
                {
                    if (i == y_names.Count)
                    {
                        return 0;
                    }
                }
            }
            return -1;
        }
    }
}
