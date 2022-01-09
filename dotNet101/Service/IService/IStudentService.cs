using dotNet101.Model;
using System.Threading.Tasks;

namespace dotNet101.Service.IService
{
    public interface IStudentService
    {
        public Task<Student> AddStudent(Student student);
    }
}