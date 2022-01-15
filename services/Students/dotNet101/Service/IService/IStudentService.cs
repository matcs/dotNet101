using System.Collections.Generic;
using System.Threading.Tasks;
using dotNet101.Model;

namespace dotNet101.Service.IService
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudents();
        Task<Student> GetStudentById(int id);
        Task<Student> UpdateStudent(Student student);
        Task<Student> AddStudent(Student student);
        Task<object> DeleteStudent(int id);

    }
}