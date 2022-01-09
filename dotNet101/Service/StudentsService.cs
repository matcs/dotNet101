using dotNet101.Data;
using dotNet101.Model;

namespace dotNet101.Service
{
    public class StudentsService
    {

        private readonly ApplicationDbContext _context;

        public StudentsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Save(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }
    }
}
