using dotNet101.Data;
using dotNet101.Model;
using dotNet101.Service.IService;
using System.Threading.Tasks;

namespace dotNet101.Service
{
    public class StudentsService : IStudentService
    {

        private readonly ApplicationDbContext _context;

        public StudentsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> AddStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            
            return student;
        }
    }
}
